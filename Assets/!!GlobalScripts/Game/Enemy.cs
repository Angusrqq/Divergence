using UnityEngine;
using System;
using System.Collections;

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

    public virtual void SetTarget(Transform newTarget) => target = newTarget;
    public virtual void TakeDamage(GameObject source, float amount, float knockbackForce = 0f, float knockbackDuration = 0f)
    {
        if (damageableEntity.canTakeDamage())
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
    public virtual void Heal(UnityEngine.Object source, float amount) => damageableEntity.Heal(source, amount);

    protected virtual void SetMaxHealth(float amount)
    {
        damageableEntity.maxHealth = amount;
        maxHealth = amount;
    }

    protected virtual void OnSpawn() { }

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
    IEnumerator DamageFlash()
    {
        spriteRenderer.color = flashColor;
        yield return new WaitForSeconds(damageFlashDuration);
        spriteRenderer.color = originalColor;
    }

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
