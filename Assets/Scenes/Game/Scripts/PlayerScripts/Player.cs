using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// <para>
/// <c>Player</c> is a class for handling the player.
/// </para>
/// Handles the player`s movement, animations, abilities, health, experience, etc.
/// <para>
///TODO: Not fully universal yet (for character specific stuff), since there are still many things that need to be implemented
/// </para>
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(DamageableEntity))]
[RequireComponent(typeof(AbilityHolder))]
[RequireComponent(typeof(AnimatedEntity))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AbilityHolder))]
public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    [NonSerialized] public DamageableEntity damageableEntity;
    [NonSerialized] public AnimatedEntity animatedEntity;
    [NonSerialized] public SpriteRenderer spriteRenderer;
    [NonSerialized] public Character characterData;
    [NonSerialized] public int level = 0;
    [NonSerialized] public int exp = 0;
    [NonSerialized] public int expNext = 100;
    [NonSerialized] public Action<UnityEngine.Object, int> onExpChange;
    [SerializeField] private float movementSpeed = 12f;
    [NonSerialized] public AbilityHolder abilityHolder;
    public float maxHealth = 100f;
    public GameObject HealthBar;
    private Slider healthSlider;
    public Vector2 movementVector;

    public GUI GUI;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        damageableEntity = GetComponent<DamageableEntity>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animatedEntity = GetComponent<AnimatedEntity>();
        healthSlider = HealthBar.GetComponent<Slider>();
        abilityHolder = GetComponent<AbilityHolder>();

        damageableEntity.onDamageTaken += UpdateHealth;
        damageableEntity.onHeal += UpdateHealth;
        GameData.UpdatePlayerRef(this);
        characterData = GameData.currentCharacter ? GameData.currentCharacter : GameData.Characters[0];
        BuildCharacter();
        if (damageableEntity == null)
        {
            Debug.LogError($"{this.GetType()} at {gameObject} has no DamageableEntity component");
        }
        damageableEntity.onDeath += OnDeath;
        damageableEntity.Init(maxHealth);
    }

    /// <summary>
    /// <para>
    /// <c>Update</c> is a method for updating the player.
    /// </para>
    /// Handles the player`s input, sets the movement vector and changes the animation.
    /// </summary>
    private void Update()
    {
        movementVector = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
        {
            movementVector.y += 1;
        }

        if (Input.GetKey(KeyCode.S))
        {
            movementVector.y += -1;
        }

        if (Input.GetKey(KeyCode.A))
        {
            movementVector.x += -1;
        }

        if (Input.GetKey(KeyCode.D))
        {
            movementVector.x += 1;
        }

        // Normalize to prevent faster diagonal movement
        movementVector = movementVector.normalized;
        if (movementVector != Vector2.zero)
        {
            if (movementVector.x < 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (movementVector.x > 0)
            {
                spriteRenderer.flipX = false;
            }
            animatedEntity.ChangeAnimation("Run");
        }
        else
        {
            animatedEntity.ChangeAnimation(AnimatedEntity.AnimationsList.Default);
        }
    }

    /// <summary>
    /// <para>
    /// <c>FixedUpdate</c> is used here for updating the player`s physics.
    /// </para>
    /// Updates the player`s position.
    /// </summary>
    private void FixedUpdate() // Use FixedUpdate for physics-related updates
    {
        rb.MovePosition(rb.position + movementVector * movementSpeed * Time.fixedDeltaTime);
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
            if (collision_dentity.canDealDamage)
            {
                damageableEntity.TakeDamage(collision.gameObject, collision_dentity.damage);
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
        healthSlider.value = damageableEntity.health / damageableEntity.maxHealth;
    }
    
    /// <summary>
    /// <para>
    /// <c>OnDestroy</c> called when the GameObject is destroyed
    /// </para>
    /// Just unsubscribes from events
    /// </summary>
    void OnDestroy()
    {
        damageableEntity.onDamageTaken -= UpdateHealth;
        damageableEntity.onHeal -= UpdateHealth;
        damageableEntity.onDeath -= OnDeath;

        onExpChange = null;
    }

    public void LevelUp()
    {
        level++;
        Debug.Log($"Player {gameObject.name} leveled up to level {level}");
    }

    public void AddExp(UnityEngine.Object source, int exp_to_add)
    {
        if (expNext - exp <= exp_to_add)
        {
            exp = exp_to_add - (expNext - exp);
            expNext += Mathf.RoundToInt(expNext * 1.1f);
            LevelUp();
            return;
        }
        else exp += exp_to_add;

        onExpChange?.Invoke(source, exp_to_add);
        Debug.Log($"Player {gameObject.name} gained {exp_to_add} exp from {source.name}");
    }

    public void TakeExp(UnityEngine.Object source, int exp_to_take)
    {
        throw new NotImplementedException();
    }

    private void BuildCharacter()
    {
        animatedEntity.SetAnimatorController(characterData.animatorController);

        movementSpeed = characterData.movementSpeed;
        maxHealth = characterData.maxHealth;
        level = characterData.startLevel;

        foreach (Ability a in characterData.startingAbilities)
        {
            abilityHolder.AddAbility(a);
        }
    }
}
