using UnityEngine;
using System;

/// <summary>
/// <para>
/// <c>DamageableEntity</c> is a class for handling the health and damage of an entity (do not confuse with the Unity`s ECS (Entity Component System) entity).
/// </para>
/// Should be used like a component or a module for objects that can take damage and/or heal.
/// </summary>
public class DamageableEntity : MonoBehaviour, IDamageable
{
    public float Health { get; set; }
    public float MaxHealth { get; set; }
    public bool canDealDamage = false;
    public float Damage { get; set; }
    public event Action<UnityEngine.Object> OnDeath;
    public event Action<UnityEngine.Object, float> OnDamageTaken;
    public event Action<UnityEngine.Object, float> OnHeal;
    public bool isVulnerable = true;
    public bool canHeal = true;

    /// <summary>
    /// <para>
    /// <c>TakeDamage</c> is a method for taking damage to the entity.
    /// </para>
    /// Checks if the entity is vulnerable and if it has health left.
    /// <para>
    /// If the entity has health left, it takes the damage and invokes the <c>onDamageTaken</c> event.
    /// </para>
    /// If the entity has no health left, it invokes the <c>onDeath</c> event.
    /// </summary>
    public void TakeDamage(UnityEngine.Object source, float amount)
    {
        if (!isVulnerable) return;
        if (Health <= 0) return;

        if (Health - amount <= 0)
        {
            float taken = Health;
            Health = 0;
            OnDamageTaken?.Invoke(source, taken);
            OnDeath?.Invoke(source);
            return;
        }
        
        Health -= amount;
        OnDamageTaken?.Invoke(source, amount);
    }

    /// <summary>
    /// <para>
    /// <c>СanTakeDamage</c> is a method for checking if the entity can take damage.
    /// </para>
    /// Checks if the entity is vulnerable and if it has health left.
    /// <para>
    /// If the entity is vulnerable and has health left, it returns true.
    /// </para>
    /// Should be used if you want to do something before taking damage.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the entity can take damage, <c>false</c> otherwise.
    /// </returns>
    public bool CanTakeDamage() => isVulnerable && Health > 0;

    /// <summary>
    /// <para>
    /// <c>Heal</c> is a method for healing the entity.
    /// </para>
    /// Checks if the entity can heal and if it has health left.
    /// <para>
    /// If the entity can heal and has health left, it heals the entity and invokes the <c>onHeal</c> event.
    /// </para>
    /// </summary>
    public void Heal(UnityEngine.Object source, float amount)
    {
        if (!canHeal) return;
        if (Health <= 0 || Health >= MaxHealth) return;

        if (Health + amount >= MaxHealth)
        {
            OnHeal?.Invoke(source, MaxHealth - Health);
            Health = MaxHealth;
            return;
        }

        Health += amount;
    }

    /// <summary>
    /// <para>
    /// <c>Init</c> is a method for initializing the entity.
    /// </para>
    /// <para>
    /// Sets the max health, health, can deal damage and damage of the entity.
    /// </para>
    /// </summary>
    public void Init(float maxHealth, bool canDealDamage = false, float damage = 0)
    {
        MaxHealth = maxHealth;
        Health = maxHealth;
        
        if (canDealDamage)
        {
            this.canDealDamage = canDealDamage;
            Damage = damage;
        }
    }
}
