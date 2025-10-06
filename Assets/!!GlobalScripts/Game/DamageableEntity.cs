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
    public float health { get; set; }
    public float maxHealth { get; set; }
    public bool canDealDamage = false;
    public float damage { get; set; }
    public event Action<UnityEngine.Object> onDeath;
    public event Action<UnityEngine.Object, float> onDamageTaken;
    public event Action<UnityEngine.Object, float> onHeal;
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
        if (health <= 0) return;
        if (health - amount <= 0)
        {
            float taken = health;
            health = 0;
            onDamageTaken?.Invoke(source, taken);
            onDeath?.Invoke(source);
            return;
        }
        health -= amount;
        onDamageTaken?.Invoke(source, amount);
    }

/// <summary>
/// <para>
/// <c>canTakeDamage</c> is a method for checking if the entity can take damage.
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
    public bool canTakeDamage() => isVulnerable && health > 0;

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
        if (health <= 0 || health >= maxHealth) return;
        if (health + amount >= maxHealth)
        {
            onHeal?.Invoke(source, maxHealth - health);
            health = maxHealth;
            return;
        }
        health += amount;
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
        this.maxHealth = maxHealth;
        this.health = maxHealth;
        if (canDealDamage)
        {
            this.canDealDamage = canDealDamage;
            this.damage = damage;
        }
    }
}
