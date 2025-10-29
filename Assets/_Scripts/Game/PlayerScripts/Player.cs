using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(DamageableEntity))]
[RequireComponent(typeof(AbilityHolder))]
[RequireComponent(typeof(AnimatedEntity))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AbilityHolder))]

// TODO: Egor - Write normal documentation, ts doesnt work
/// <summary>
/// <para>
/// <c>Player</c> is a class for handling the player.
/// </para>
/// Handles the player`s movement, animations, abilities, health, experience, etc.
/// </summary>
public class Player : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 12f;

    [NonSerialized] public DamageableEntity DamageableEntity;
    [NonSerialized] public AnimatedEntity AnimatedEntity;
    [NonSerialized] public SpriteRenderer SpriteRenderer;
    [NonSerialized] public Character CharacterData;
    [NonSerialized] public int Level = 0;
    [NonSerialized] public int Exp = 0;
    [NonSerialized] public int ExpNext = 100;
    [NonSerialized] public Action<UnityEngine.Object, int> OnExpChange;
    [NonSerialized] public Action<UnityEngine.Object, int> OnLevelUp;
    [NonSerialized] public AbilityHolder AbilityHolder;
    public float MaxHealth = 100f;
    public RectTransform HealthBar;
    public RectTransform LevelBar;
    public Vector2 MovementVector;
    public GUI GUI;

    private Rigidbody2D _rb;
    private Slider _healthSlider;
    private Slider _levelSlider;
    private TMPro.TMP_Text _levelLabel;

    public PlayerMagnet Magnet { get; private set; }

    void Awake()
    {
        GameData.UpdatePlayerRef(this);
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
        OnExpChange += UpdateLevelBar;

        CharacterData = GameData.currentCharacter ? GameData.currentCharacter : GameData.Characters[0];
        BuildCharacter();

        if (DamageableEntity == null)
        {
            Debug.LogError($"{GetType()} at {gameObject} has no DamageableEntity component");
        }

        DamageableEntity.OnDeath += OnDeath;
        DamageableEntity.Init(MaxHealth);
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
        _rb.MovePosition(_rb.position + _movementSpeed * Time.fixedDeltaTime * MovementVector);
        UpdateHpBarPosition();
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
                float damage = collision_dentity.Damage * Attributes.PlayerResistsMult;
                DamageableEntity.TakeDamage(collision.gameObject, damage);
            }
        }
    }

    /// <summary>
    /// <para>
    /// <c>UpdateHealth</c> is used here for updating the health bar.
    /// </para>
    /// Updates the player`s health slider.
    /// </summary>
    void UpdateHealth(UnityEngine.Object source, float amount, Type type = null)
    {
        _healthSlider.value = DamageableEntity.Health / DamageableEntity.MaxHealth;
    }

    void UpdateHpBarPosition()
    {
        float offset = -0.5f;
        Vector2 pos = new Vector2(transform.position.x, -(SpriteRenderer.bounds.size.y / 2) + transform.position.y + offset);
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
    /// <c>OnDestroy</c> called when the GameObject is destroyed
    /// </para>
    /// Just unsubscribes from events
    /// </summary>
    void OnDestroy()
    {
        DamageableEntity.OnDamageTaken -= UpdateHealth;
        DamageableEntity.OnHeal -= UpdateHealth;
        DamageableEntity.OnDeath -= OnDeath;

        OnExpChange = null;
        OnLevelUp = null;
    }

    /// <summary>
    /// <para>
    /// Levels up the player, increases the experience required for the next level, and logs the level up event.
    /// </para>
    /// </summary>
    public void LevelUp(UnityEngine.Object source)
    {
        Level++;
        ExpNext += Mathf.RoundToInt(ExpNext * 1.1f);
        OnLevelUp?.Invoke(source, Level);
        Debug.Log($"Player {gameObject.name} leveled up to level {Level}");
    }

    /// <summary>
    /// <para>
    /// Adds experience points to the player.
    /// </para>
    /// If the experience points added are enough to level up the player, levels up the player and logs the level up.
    /// </summary>
    /// <param name="source">The source of the experience points.</param>
    /// <param name="exp_to_add">The amount of experience points to add.</param>
    public void AddExp(UnityEngine.Object source, int exp_to_add)
    {
        if (ExpNext - Exp <= exp_to_add)
        {
            Exp = exp_to_add - (ExpNext - Exp);
            LevelUp(source);
            return;
        }
        else
        {
            Exp += exp_to_add;
        }

        OnExpChange?.Invoke(source, exp_to_add);
        Debug.Log($"Player {gameObject.name} gained {exp_to_add} exp from {source.name}");
    }

    public void UpdateLevelBar(UnityEngine.Object source, int expValue)
    {
        _levelSlider.value = (float)Exp / ExpNext;
        _levelLabel.text = $"lv. {Level}";
    }

    // TODO: Uncomment if needed
    // public void TakeExp(UnityEngine.Object source, int exp_to_take)
    // {
    //     throw new NotImplementedException();
    // }

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

        _movementSpeed = CharacterData.MovementSpeed;
        MaxHealth = CharacterData.MaxHealth;
        Level = CharacterData.StartLevel;

        foreach (Ability ability in CharacterData.StartingAbilities)
        {
            AbilityHolder.AddAbility(ability);
        }
    }
}
