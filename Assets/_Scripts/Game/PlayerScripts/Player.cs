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
/// <para>
/// TODO: Not fully universal yet (for character specific stuff), since there are still many things that need to be implemented
/// </para>
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

        GameData.UpdatePlayerRef(this);
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
                DamageableEntity.TakeDamage(collision.gameObject, collision_dentity.Damage);
            }
        }
    }

    /// <summary>
    /// <para>
    /// <c>UpdateHealth</c> is used here for updating the health bar.
    /// </para>
    /// Updates the player`s health slider.
    /// </summary>
    void UpdateHealth(UnityEngine.Object source, float amount)
    {
        _healthSlider.value = DamageableEntity.Health / DamageableEntity.MaxHealth;
    }

    void UpdateHpBarPosition()//TODO: figure out this fucking bullshit
    {
        //how the fuck do i do that in orthographic camera or whatever the fucking the mode is
        //HealthBar.position = Camera.main.WorldToScreenPoint(transform.position);//worldtoscreenpoint doesnt work in orthographic camera?? what am i supposed to do
        //Debug.Log(Camera.main.WorldToScreenPoint(transform.position));
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
    }

    /// <summary>
    /// <para>
    /// Levels up the player, increases the experience required for the next level, and logs the level up event.
    /// </para>
    /// </summary>
    public void LevelUp()
    {
        Level++;
        ExpNext += Mathf.RoundToInt(ExpNext * 1.1f);
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
            LevelUp();
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
        AnimatedEntity.SetAnimatorController(CharacterData.AnimatorController);

        _movementSpeed = CharacterData.movementSpeed;
        MaxHealth = CharacterData.maxHealth;
        Level = CharacterData.startLevel;

        foreach (Ability ability in CharacterData.startingAbilities)
        {
            AbilityHolder.AddAbility(ability);
        }
    }
}
