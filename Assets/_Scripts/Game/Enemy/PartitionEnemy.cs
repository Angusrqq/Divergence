using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// <para>
/// Base enemy class.
/// </para>
/// </summary>
[RequireComponent(typeof(DamageableEntity))]
[RequireComponent(typeof(AnimatedEntity))]
[RequireComponent(typeof(SpriteRenderer))]
public class PartitionEnemy : MonoBehaviour
{
    [NonSerialized] public DamageableEntity damageableEntity;
    [NonSerialized] public AnimatedEntity animatedEntity;
    [NonSerialized] public SpriteRenderer spriteRenderer;
    [NonSerialized] public EnemyData enemyData;
    [SerializeField] private ParticleSystem m_particleSystem;
    private ParticleSystem m_psInstance;
    protected Transform target;
    public float maxHealth = 100f;
    public float moveSpeed = 12f;
    public float damage = 1f;
    public int xpDrop = 10;
    private Color originalColor;
    public Color flashColor;
    public float damageFlashDuration = 2f;
    [NonSerialized] public bool isRespawnable = false;
    [SerializeField] private XpCrystal xpCrystalPrefab;
    Vector2 knockbackVelocity;
    float knockbackDuration;

    public void Init(EnemyData data, Transform newTarget)
    {
        enemyData = data;
        maxHealth = data.BaseMaxHealth;
        damage = data.Damage;
        xpDrop = data.BaseExp;
        moveSpeed = data.BaseMovementSpeed;
        target = newTarget;
        animatedEntity = GetComponent<AnimatedEntity>();
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
                transform.SetPositionAndRotation((Vector2)transform.position + knockbackVelocity * Time.fixedDeltaTime, Quaternion.identity);
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
            transform.SetPositionAndRotation((Vector2)transform.position + moveSpeed * Time.fixedDeltaTime * direction, Quaternion.identity);
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
        float elapsedTime = 0f;
        material.SetColor("_FlashColor", flashColor);
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
        if (knockbackDuration > 0) return;

        knockbackVelocity = velocity;
        knockbackDuration = duration;
    }

    void EmitParticles(Vector2 direction)
    {
        Quaternion spawnRotation = Quaternion.FromToRotation(Vector2.right, direction);
        m_psInstance = Instantiate(m_particleSystem, transform.position, spawnRotation, transform.parent);
    }
}
