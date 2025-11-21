using System;
using Unity.VisualScripting;
using UnityEngine;
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

    /// <summary>
    /// Initializes global player reference.
    /// </summary>
    void Awake()
    {
        GameData.UpdatePlayerRef(this);
    }

    /// <summary>
    /// Caches components, wires events, builds character, and initializes health.
    /// </summary>
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

        // _abilityIconDisplay.UpdateActiveAbilitiesIcons(AbilityHolder.GetActiveAbilitiesList());
        // _abilityIconDisplay.UpdatePassiveAbilitiesIcons(AbilityHolder.GetPassiveAbilitiesList());
    }

    /// <summary>
    /// <para>
    /// <c>Update</c> is a method for updating the player.
    /// </para>
    /// Handles the player`s input, sets the movement vector and changes the animation.
    /// </summary>
    private void Update()
    {
        MovementVector = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
        {
            MovementVector.y += 1;
        }

        if (Input.GetKey(KeyCode.S))
        {
            MovementVector.y += -1;
        }

        if (Input.GetKey(KeyCode.A))
        {
            MovementVector.x += -1;
        }

        if (Input.GetKey(KeyCode.D))
        {
            MovementVector.x += 1;
        }

        // Normalize to prevent faster diagonal movement
        MovementVector = MovementVector.normalized;
        if (MovementVector != Vector2.zero)
        {
            if (MovementVector.x < 0)
            {
                SpriteRenderer.flipX = true;
            }
            else if (MovementVector.x > 0)
            {
                SpriteRenderer.flipX = false;
            }

            AnimatedEntity.ChangeAnimation("Run");
        }
        else
        {
            AnimatedEntity.ChangeAnimation(AnimatedEntity.AnimationsList.Default);
        }
    }

    /// <summary>
    /// <para>
    /// <c>FixedUpdate</c> is used here for updating the player`s physics.
    /// </para>
    /// Updates the player`s position.
    /// </summary>
    private void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + MovementSpeed * Time.fixedDeltaTime * MovementVector);
        UpdateHpBarPosition();
        if (_experienceDirty)
        {
            OverLevel();
        }
    }

    /// <summary>
    /// <para>
    /// <c>OnCollisionStay2D</c> is used here for handling the player`s collisions.
    /// </para>
    /// Handles the player`s collisions with other objects.
    /// <para>
    /// If the collision object has a <c>DamageableEntity</c> component and the <c>canDealDamage</c> property is true, it deals damage to the player.
    /// </para>
    /// </summary>
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

    /// <summary>
    /// <para>
    /// <c>OnDestroy</c> called when the GameObject is destroyed
    /// </para>
    /// Just unsubscribes from events
    /// </summary>
    void OnDestroy()
    {
        DamageableEntity.OnDamageTaken -= UpdateHealth;
        DamageableEntity.OnHeal -= UpdateHealth;
        DamageableEntity.OnDeath -= OnDeath;

        _onExperienceChange = null;
        OnLevelUp = null;
    }

    /// <summary>
    /// <para>
    /// <c>OnDeath</c> is called when the player dies (<c>onDeath</c> event from the <c>DamageableEntity</c> class).
    /// </para>
    /// Shows the death screen and logs the death.
    /// </summary>
    private void OnDeath(UnityEngine.Object source)
    {
        GUI.Death();
        Debug.Log($"Player died by {source}");
    }

    /// <summary>
    /// <para>
    /// <c>UpdateHealth</c> is used here for updating the health bar.
    /// </para>
    /// Updates the player`s health slider.
    /// </summary>
    /// <param name="source">Origin of the health change.</param>
    /// <param name="amount">Change amount.</param>
    void UpdateHealth(UnityEngine.Object source, float amount, Type type = null)
    {
        _healthSlider.value = DamageableEntity.Health / DamageableEntity.MaxHealth;
    }

    /// <summary>
    /// Positions the health bar UI above the player's sprite in screen space.
    /// </summary>
    void UpdateHpBarPosition()
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

    /// <summary>
    /// <para>
    /// <c>LevelUp</c> is called when the player levels up.
    /// </para>
    /// Increments the player`s level, updates the experience required to level up, and invokes the <c>OnLevelUp</c> event.
    /// </summary>
    /// <param name="source">Origin of the level up.</param>
    public void LevelUp(UnityEngine.Object source)
    {
        Level++;

        if (_experience > _experienceToLevelUp)
        {
            _experienceDirty = true;
        }
        else
        {
            _experienceDirty = false;
        }

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

    /// <summary>
    /// Adds experience to the player, leveling up if necessary.
    /// </summary>
    /// <param name="experienceSource">Origin of the experience.</param>
    /// <param name="experienceToAdd">Amount of experience to add.</param>
    public void AddExperience(UnityEngine.Object experienceSource, byte experienceToAdd)
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
        if(experienceSource.GetComponent<ExperienceCrystal>() != null)
        {
            OnCrystalPickup?.Invoke((int)gainedExperience);
        }
        Debug.Log($"Player gained {gainedExperience} experience from {experienceSource.name} | {_experience}/{_experienceToLevelUp}");
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

    /// <summary>
    /// Updates the level bar UI with the current experience and level.
    /// </summary>
    /// <param name="source">The source of the experience change.</param>
    /// <param name="experienceValue">The amount of experience gained.</param>
    public void UpdateLevelBar(UnityEngine.Object source, int experienceValue)
    {
        _levelSlider.value = _experience / _experienceToLevelUp;
        _levelLabel.text = $"lv. {Level}";
    }

    /// <summary>
    /// <para>
    /// Sets up the player character with the given character data.
    /// </para>
    /// Sets the animator controller, movement speed, max health and level of the player.
    /// Adds all the starting abilities of the character to the ability holder.
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
