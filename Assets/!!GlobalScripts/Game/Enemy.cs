using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(DamageableEntity))]
[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour
{
    [NonSerialized]
    public DamageableEntity damageableEntity;
    protected Transform target;
    protected Rigidbody2D rb;
    public float moveSpeed = 2f;
    public float maxHealth = 100f;
    public float damage = 10f;
    [NonSerialized]
    public bool isRespawnable = false;
    protected virtual void Awake()
    {
        damageableEntity = GetComponent<DamageableEntity>();
        if (damageableEntity == null)
        {
            Debug.LogError($"{this.GetType()} at {gameObject} has no DamageableEntity component");
        }
        damageableEntity.onDeath += OnDeath;
        damageableEntity.Init(maxHealth, true, damage);
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void FixedUpdate()
    {
        if (target != null)
        {
            Vector2 direction = (target.transform.position - transform.position).normalized;
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
        }
    }

    public virtual void SetTarget(Transform newTarget) => target = newTarget;
    public virtual void TakeDamage(UnityEngine.Object source, float amount) => damageableEntity.TakeDamage(source, amount);
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
        Destroy(gameObject);
    }

    protected virtual void OnDestroy()
    {
        damageableEntity.onDeath -= OnDeath;
    }
}
