using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Main player controller responsible for input, movement, health/XP tracking,
/// UI updates, and reacting to gameplay events (damage, death, level up).
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(DamageableEntity))]
[RequireComponent(typeof(AbilityHolder))]
[RequireComponent(typeof(AnimatedEntity))]
[RequireComponent(typeof(SpriteRenderer))]
public class Player : MonoBehaviour
{
    [SerializeField] private AbilityIconDisplay _playerAbilityIconDisplay;
    [SerializeField] private InputActionReference _movementAction;

    [NonSerialized] public DamageableEntity DamageableEntity;
    [NonSerialized] public AnimatedEntity AnimatedEntity;
    [NonSerialized] public SpriteRenderer SpriteRenderer;
    [NonSerialized] public Character CharacterData;
    [NonSerialized] public int Level = 0;
    [NonSerialized] public Action<UnityEngine.Object, int> OnLevelUp;
    [NonSerialized] public AbilityHolder AbilityHolder;
    public float MaxHealth = 100f;
    public RectTransform HealthBar;
    public RectTransform LevelBar;
    public Vector2 MovementVector;
    public GUI GUI;
    public Stat MovementSpeed = 12f;

    private Rigidbody2D _rb;
    private Slider _healthSlider;
    private Slider _levelSlider;
    private TMPro.TMP_Text _levelLabel;
    private UnityEngine.Object _lastSource = null;
    private float _experience;
    private int _experienceToLevelUp = 5;
    private Action<UnityEngine.Object, int> _onExperienceChange;
    public Action<int> OnCrystalPickup;
    private bool _experienceDirty = false;

    public PlayerMagnet Magnet { get; private set; }
    public AbilityIconDisplay PlayerAbilityIconDisplay => _playerAbilityIconDisplay;

    void Awake()
    {
        GameData.UpdatePlayerReference(this);
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        Magnet = GetComponentInChildren<PlayerMagnet>();
        DamageableEntity = GetComponent<DamageableEntity>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        AnimatedEntity = GetComponent<AnimatedEntity>();
        _healthSlider = HealthBar.GetComponent<Slider>();
        _levelSlider = LevelBar.GetComponent<Slider>();
        _levelLabel = LevelBar.GetComponentInChildren<TMPro.TMP_Text>();
        AbilityHolder = GetComponent<AbilityHolder>();

        DamageableEntity.OnDamageTaken += UpdateHealth;
        DamageableEntity.OnHeal += UpdateHealth;
        _onExperienceChange += UpdateLevelBar;

        CharacterData = GameData.currentCharacter ? GameData.currentCharacter : GameData.Characters[0];
        BuildCharacter();

        if (DamageableEntity == null)
        {
            Debug.LogError($"{GetType()} at {gameObject} has no DamageableEntity component");
        }

        DamageableEntity.OnDeath += OnDeath;
        DamageableEntity.Init(MaxHealth);
        DamageableEntity.OnDamageTaken += (source, amount, type) => GameData.InGameAttributes.DamageTaken += amount;
    }

    private void Update()
    {
        MovementVector = _movementAction.action.ReadValue<Vector2>();
        if (MovementVector != Vector2.zero)
        {
            SpriteRenderer.flipX = MovementVector.x != 0 ? MovementVector.x < 0 : SpriteRenderer.flipX;
            AnimatedEntity.ChangeAnimation("Run");
        }
        else
        {
            AnimatedEntity.ChangeAnimation(AnimatedEntity.AnimationsList.Default);
        }
    }

    private void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + MovementSpeed * Time.fixedDeltaTime * MovementVector);
        UpdateHpBarPosition();

        if (_experienceDirty) OverLevel();
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out DamageableEntity collision_dentity))
        {
            if (collision_dentity.CanDealDamage)
            {
                float damage = collision_dentity.Damage * GameData.InGameAttributes.PlayerResistsMult;
                DamageableEntity.TakeDamage(collision.gameObject, damage);
            }
        }
    }

    void OnDestroy()
    {
        DamageableEntity.OnDamageTaken -= UpdateHealth;
        DamageableEntity.OnHeal -= UpdateHealth;
        DamageableEntity.OnDeath -= OnDeath;

        _onExperienceChange = null;
        OnLevelUp = null;
    }

    private void OnDeath(UnityEngine.Object source)
    {
        GUI.Death();

        Debug.Log($"Player died by {source}");
    }

    /// <summary>
    /// <c>UpdateHealth</c> is used here for updating the health bar.
    /// </summary>
    private void UpdateHealth(UnityEngine.Object source, float amount, Type type = null)
    {
        _healthSlider.value = DamageableEntity.Health / DamageableEntity.MaxHealth;
    }

    /// <summary>
    /// Positions the health bar UI below the player's sprite in screen space.
    /// </summary>
    private void UpdateHpBarPosition()
    {
        float offset = -0.5f;
        Vector2 pos = new(transform.position.x, -(SpriteRenderer.bounds.size.y / 2) + transform.position.y + offset);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(pos);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            HealthBar.parent.transform as RectTransform,
            screenPos,
            Camera.main,
            out Vector2 localPos
        );

        HealthBar.localPosition = localPos;
    }

    public void LevelUp(UnityEngine.Object source)
    {
        Level++;

        _experienceDirty = _experience > _experienceToLevelUp;

        // Special walls
        if (Level == 20)
        {
            _experienceToLevelUp += 10 + 300;
        }
        else if (Level == 40)
        {
            _experienceToLevelUp += 13 + 1200;
        }
        // Ranges
        else if (Level <= 19)
        {
            _experienceToLevelUp += 10;
        }
        else if (Level <= 39)
        {
            _experienceToLevelUp += 13;
        }
        // Level >= 41
        else
        {
            _experienceToLevelUp += 16;
        }

        OnLevelUp?.Invoke(source, Level);
        Debug.Log($"Player leveled up to level {Level}");
    }

    public void AddExperience(UnityEngine.Object experienceSource, float experienceToAdd)
    {
        float gainedExperience = experienceToAdd * GameData.InGameAttributes.ExperienceMultiplier;

        if (_experienceToLevelUp - _experience <= gainedExperience)
        {
            _experience = gainedExperience - (_experienceToLevelUp - _experience);

            LevelUp(experienceSource);
        }
        else
        {
            _experience += gainedExperience;
        }

        _onExperienceChange?.Invoke(experienceSource, (int)gainedExperience);

        if (experienceSource.GetComponent<ExperienceCrystal>() != null)
        {
            OnCrystalPickup?.Invoke((int)gainedExperience);
        }
    }

    /// <summary>
    /// Called when the player's experience goes over the required amount for the next level.
    /// Resets the experience counter to 0 and levels up the player using <see cref="LevelUp"/>, then invokes the <see cref="_onExperienceChange"/> event.
    /// </summary>
    private void OverLevel()
    {
        _experience -= _experienceToLevelUp;
        LevelUp(_lastSource);

        _onExperienceChange?.Invoke(_lastSource, _experienceToLevelUp);
    }

    public void UpdateLevelBar(UnityEngine.Object source, int experienceValue)
    {
        _levelSlider.value = _experience / _experienceToLevelUp;
        _levelLabel.text = $"lv. {Level}";
    }

    /// <summary>
    /// Sets up the player character with the given character data.
    /// </summary>
    private void BuildCharacter()
    {
        AnimatedEntity.SetAnimatorController(CharacterData.CharacterAnimatorController);

        MovementSpeed = CharacterData.MovementSpeed;
        MaxHealth = CharacterData.MaxHealth;
        Level = CharacterData.StartLevel;

        foreach (BaseAbilityScriptable ability in CharacterData.StartingAbilities)
        {
            if (ability.GetType() == typeof(InstantiatedAbilityScriptable))
            {
                AbilityHolder.AddAbility((InstantiatedAbilityScriptable)ability);
            }
            
            if (ability.GetType() == typeof(PassiveAbility))
            {
                AbilityHolder.AddPassive((PassiveAbility)ability);
            }
        }
    }

    public void TriggerOnCrystalPickup(int expGained) => OnCrystalPickup?.Invoke(expGained);
}
