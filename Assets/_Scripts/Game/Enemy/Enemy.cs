using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// <para>
/// Base enemy class.
/// </para>
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(DamageableEntity))]
[RequireComponent(typeof(AnimatedEntity))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Enemy : MonoBehaviour
{
    [NonSerialized] public DamageableEntity damageableEntity;
    [NonSerialized] public AnimatedEntity animatedEntity;
    [NonSerialized] public SpriteRenderer spriteRenderer;
    [NonSerialized] public EnemyData enemyData;
    protected Transform target;
    protected Rigidbody2D rb;
    public float maxHealth = 100f;
    public float moveSpeed = 12f;
    public float damage = 1f;
    public int xpDrop = 10;
    private Color originalColor;
    public Color flashColor;
    public float damageFlashDuration = 0.1f;
    [NonSerialized] public bool isRespawnable = false;
    [SerializeField] private XpCrystal xpCrystalPrefab;
    Vector2 knockbackVelocity;
    float knockbackDuration;

    public void Init(EnemyData data, Transform newTarget)
    {
        enemyData = data;
        maxHealth = data.baseMaxHealth;
        damage = data.damage;
        xpDrop = data.baseExp;
        moveSpeed = data.baseMovementSpeed;
        target = newTarget;
        animatedEntity = GetComponent<AnimatedEntity>();
        CircleCollider2D circleCollider2D = GetComponent<CircleCollider2D>();
        circleCollider2D.radius = data.colliderRadius;
        circleCollider2D.offset = data.colliderOffset;
        animatedEntity.SetAnimatorController(data.animatorController);
    }

    /// <summary>
    /// <para>
    /// <c>Awake</c> is a method for initializing the enemy.
    /// </para>
    /// Initializes the damageable entity, sets the max health, damage and knockback velocity and duration.
    /// </summary>
    protected virtual void Awake()
    {
        damageableEntity = GetComponent<DamageableEntity>();

        if (damageableEntity == null)
        {
            Debug.LogError($"{GetType()} at {gameObject} has no DamageableEntity component");
        }

        damageableEntity.OnDeath += OnDeath;
        damageableEntity.Init(maxHealth, true, damage);
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    /// <summary>
    /// <para>
    /// <c>FixedUpdate</c> is a method for updating the enemy.
    /// </para>
    /// Moves the enemy towards the target and applies the knockback if there is one.
    /// </summary>
    protected virtual void FixedUpdate()
    {
        if (target != null)
        {
            if (knockbackDuration > 0)
            {
                rb.MovePosition(rb.position + knockbackVelocity * Time.fixedDeltaTime);
                knockbackDuration -= Time.fixedDeltaTime;
                return;
            }

            Vector2 direction = (target.transform.position - transform.position).normalized;
            if (direction != Vector2.zero)
            {
                if (direction.x < 0)
                {
                    spriteRenderer.flipX = true;
                }
                else if (direction.x > 0)
                {
                    spriteRenderer.flipX = false;
                }
                animatedEntity.ChangeAnimation("Run");
            }
            else
            {
                animatedEntity.ChangeAnimation(AnimatedEntity.AnimationsList.Default);
            }
            rb.MovePosition(rb.position + moveSpeed * Time.fixedDeltaTime * direction);
        }
    }

    /// <summary>
    /// <para>
    /// Speaks for itself
    /// </para>
    /// </summary>
    public virtual void SetTarget(Transform newTarget) => target = newTarget;

    /// <summary>
    /// <para>
    /// Modified method from the DamageableEntity class.
    /// </para>
    /// Starts the <c>DamageFlash</c> coroutine and applies the knockback if there is one.
    /// </summary>
    public virtual void TakeDamage(GameObject source, float amount, float knockbackForce = 0f, float knockbackDuration = 0f)
    {
        if (damageableEntity.CanTakeDamage())
        {
            StartCoroutine(DamageFlash());

            if (knockbackForce > 0)
            {
                Vector2 knockbackDirection = (transform.position - source.transform.position).normalized;
                Knockback(knockbackDirection * knockbackForce, knockbackDuration);
            }

            damageableEntity.TakeDamage(source, amount);
        }
    }

    /// <summary>
    /// <para>
    /// Just the method from the DamageableEntity class.
    /// </para>
    /// </summary>
    public virtual void Heal(UnityEngine.Object source, float amount) => damageableEntity.Heal(source, amount);

    /// <summary>
    /// <para>
    /// Updates the max health of the enemy.
    /// </para>
    /// </summary>
    protected virtual void SetMaxHealth(float amount)
    {
        damageableEntity.MaxHealth = amount;
        maxHealth = amount;
    }

    /// <summary>
    /// <para>
    /// Don`t know where we will need this but seems wait wtf why we need this we have start and awake?????. am i retarded?????
    /// </para>
    /// </summary>
    protected virtual void OnSpawn() { }

    /// <summary>
    /// <para>
    /// Called when the enemy dies (<c>OnDeath</c> event from the <c>DamageableEntity</c> class).
    /// </para>
    /// </summary>
    protected virtual void OnDeath(UnityEngine.Object source)
    {
        // Debug.Log($"Enemy at {gameObject} died by {source}");

        if (xpCrystalPrefab != null)
        {
            XpCrystal SpawnedCrystal = Instantiate(xpCrystalPrefab, transform.position, Quaternion.identity);
            SpawnedCrystal.SetXpValue(xpDrop);
        }

        Destroy(gameObject);
    }

    /// <summary>
    /// Unsubscribes from the <c>OnDeath</c> event from the <c>DamageableEntity</c> class when the enemy is destroyed.
    /// </summary>
    protected virtual void OnDestroy()
    {
        damageableEntity.OnDeath -= OnDeath;
    }

    /// <summary>
    /// <para>
    /// Flashes the enemy`s sprite.
    /// </para>
    /// </summary>
    IEnumerator DamageFlash()
    {
        spriteRenderer.color = flashColor;
        yield return new WaitForSeconds(damageFlashDuration);
        spriteRenderer.color = originalColor;
    }

    /// <summary>
    /// <para>
    /// Applies the knockback to the enemy.
    /// </para>
    /// </summary>
    public virtual void Knockback(Vector2 velocity, float duration)
    {
        if (knockbackDuration > 0) return;

        knockbackVelocity = velocity;
        knockbackDuration = duration;
    }
}
