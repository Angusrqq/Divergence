using UnityEngine;
using System;

public class DamageableEntity : MonoBehaviour, IDamageable
{
    public event Action<UnityEngine.Object> OnDeath;
    public event Action<UnityEngine.Object, float, Type> OnDamageTaken;
    public event Action<UnityEngine.Object, float, Type> OnHeal;
    public bool IsVulnerable = true;
    public bool CanHeal = true;
    public bool CanDealDamage = false;

    public float Health { get; set; }
    public float MaxHealth { get; set; }
    public float Damage { get; set; }

    public void TakeDamage(UnityEngine.Object source, float damageAmount, Type type = null)
    {
        if (!IsVulnerable) return;
        if (Health <= 0) return;

        if (Health - damageAmount <= 0)
        {
            float taken = Health;
            Health = 0;

            OnDamageTaken?.Invoke(source, taken, type);
            OnDeath?.Invoke(source);

            return;
        }

        Health -= damageAmount;
        OnDamageTaken?.Invoke(source, damageAmount, type);
    }

    public bool CanTakeDamage()
    {
        return IsVulnerable && Health > 0;
    }

    public void Heal(UnityEngine.Object source, float amount, Type type)
    {
        if (!CanHeal) return;
        if (Health >= MaxHealth) return;

        if (Health + amount >= MaxHealth)
        {
            OnHeal?.Invoke(source, MaxHealth - Health, type);
            Health = MaxHealth;

            return;
        }

        Health += amount;
        OnHeal?.Invoke(source, amount, type);
    }

    public void Init(float maxHealth, bool canDealDamage = false, float damage = 0)
    {
        MaxHealth = maxHealth;
        Health = maxHealth;
        
        if (canDealDamage)
        {
            CanDealDamage = canDealDamage;
            Damage = damage;
        }
    }
}
