using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(DamageableEntity))]
[RequireComponent(typeof(AnimatedEntity))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]

/// <summary>
/// Base enemy class.
/// </summary>
public class Enemy : MonoBehaviour
{
    [SerializeField] private ParticleSystem m_particleSystem;
    [SerializeField] private XpCrystal xpCrystalPrefab;

    [NonSerialized] public DamageableEntity damageableEntity;
    [NonSerialized] public AnimatedEntity animatedEntity;
    [NonSerialized] public SpriteRenderer spriteRenderer;
    [NonSerialized] public EnemyData enemyData;
    [NonSerialized] public bool isRespawnable = false;
    public float maxHealth = 100f;
    public float moveSpeed = 12f;
    public float damage = 1f;
    public int xpDrop = 10;
    public Color flashColor;
    public float damageFlashDuration = 2f;

    protected Transform target;
    protected Rigidbody2D rb;

    // private Color _originalColor; // Uncomment if needed
    private ParticleSystem _particleSystemInstance;
    private Vector2 _knockbackVelocity;
    private float _knockbackDuration;

    /// <summary>
    /// Initializes the enemy with the provided data and target.
    /// </summary>
    /// <param name="data">The data for the enemy to be initialized with.</param>
    /// <param name="newTarget">The target for the enemy to move towards.</param>
    public void Init(EnemyData data, Transform newTarget)
    {
        enemyData = data;
        maxHealth = data.BaseMaxHealth;
        damage = data.Damage;
        xpDrop = data.BaseExp;
        moveSpeed = data.BaseMovementSpeed;
        target = newTarget;
        animatedEntity = GetComponent<AnimatedEntity>();
        CircleCollider2D circleCollider2D = GetComponent<CircleCollider2D>();
        circleCollider2D.radius = data.ColliderRadius;
        circleCollider2D.offset = data.ColliderOffset;
        animatedEntity.SetAnimatorController(data.AnimatorController);
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
        // _originalColor = spriteRenderer.color;
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
            if (_knockbackDuration > 0)
            {
                rb.MovePosition(rb.position + _knockbackVelocity * Time.fixedDeltaTime);
                _knockbackDuration -= Time.fixedDeltaTime;
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
            EmitParticles((transform.position - source.transform.position).normalized);
            AudioManager.instance.PlaySFX(1);

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
            XpCrystal SpawnedCrystal = Instantiate(xpCrystalPrefab, transform.position, Quaternion.identity, transform.parent);
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
        Material material = spriteRenderer.material;
        material.SetColor("_FlashColor", flashColor);

        float elapsedTime = 0f;
        while (elapsedTime < damageFlashDuration)
        {
            elapsedTime += Time.deltaTime;
            float flashAmount = Mathf.Lerp(1f, 0f, elapsedTime / damageFlashDuration);
            material.SetFloat("_FlashAmount", flashAmount);
            
            yield return null;
        }
    }

    /// <summary>
    /// <para>
    /// Applies the knockback to the enemy.
    /// </para>
    /// </summary>
    public virtual void Knockback(Vector2 velocity, float duration)
    {
        if (_knockbackDuration > 0) return;

        _knockbackVelocity = velocity;
        _knockbackDuration = duration;
    }

    void EmitParticles(Vector2 direction)
    {
        Quaternion spawnRotation = Quaternion.FromToRotation(Vector2.right, direction);
        _particleSystemInstance = Instantiate(m_particleSystem, transform.position, spawnRotation, transform.parent);
    }
}
