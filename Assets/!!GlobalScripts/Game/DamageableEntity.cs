using UnityEngine;
using System;

public class DamageableEntity : MonoBehaviour, IDamageable
{
    public float health { get; set; }
    public float maxHealth { get; set; }
    public bool canDealDamage = false;
    public float damage { get; set; }
    public event Action<UnityEngine.Object> onDeath;
    public event Action<UnityEngine.Object, float> onDamageTaken;
    public event Action<UnityEngine.Object, float> onHeal;
    public bool isVulnerable = true;
    public bool canHeal = true;
    public void TakeDamage(UnityEngine.Object source, float amount)
    {
        if (!isVulnerable) return;
        if (health <= 0) return;
        if (health - amount <= 0)
        {
            health = 0;
            onDamageTaken?.Invoke(source, amount);
            onDeath?.Invoke(source);
            return;
        }
        health -= amount;
        onDamageTaken?.Invoke(source, amount);
    }

    public void Heal(UnityEngine.Object source, float amount)
    {
        if (!canHeal) return;
        if (health <= 0 || health >= maxHealth) return;
        if (health + amount >= maxHealth)
        {
            onHeal?.Invoke(source, maxHealth - health);
            health = maxHealth;
            return;
        }
        health += amount;
    }

    public void Init(float maxHealth, bool canDealDamage = false, float damage = 0)
    {
        this.maxHealth = maxHealth;
        this.health = maxHealth;
        if (canDealDamage)
        {
            this.canDealDamage = canDealDamage;
            this.damage = damage;
        }
    }
}
