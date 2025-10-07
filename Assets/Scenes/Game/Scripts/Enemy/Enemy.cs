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
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Enemy : MonoBehaviour
{
    [NonSerialized] public DamageableEntity damageableEntity;
    protected Transform target;
    protected Rigidbody2D rb;
    public float moveSpeed = 2f;
    public float maxHealth = 100f;
    public float damage = 10f;
    public SpriteRenderer spriteRenderer;
    private Color originalColor;
    public Color flashColor;
    public float damageFlashDuration = 0.1f;
    [NonSerialized] public bool isRespawnable = false;
    public float xpForKill = 10f; // TODO: add xp system
    public GameObject xpCrystalPrefab;
    Vector2 knockbackVelocity;
    float knockbackDuration;

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
        damageableEntity.onDeath += OnDeath;
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
        if (damageableEntity.СanTakeDamage())
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
        damageableEntity.maxHealth = amount;
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
        Debug.Log($"Enemy at {gameObject} died by {source}");
        // do something before destroying

        if (xpCrystalPrefab != null)
            Instantiate(xpCrystalPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    protected virtual void OnDestroy()
    {
        damageableEntity.onDeath -= OnDeath;
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

// Надо написать/понять что мы можем сделать наиболее эффективно, чтобы работало

// Как мы будем спавнить врагов?
// Как мы будем инициализировать врагов?
// Где мы будем хранить данные о каждом враге? PLS NO CHAT GEPETE (or little bit)
