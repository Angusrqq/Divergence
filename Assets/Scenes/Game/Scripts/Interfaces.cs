using System;
using UnityEngine;

/// <summary>
/// <para>
/// <c>IDamageable</c> interface defines the contract for any object that can take damage and be healed.
/// </para>
/// </summary>
public interface IDamageable
{
    float health { get; set; }
    float maxHealth { get; set; }
    event Action<UnityEngine.Object> onDeath;
    event Action<UnityEngine.Object, float> onDamageTaken;
    event Action<UnityEngine.Object, float> onHeal;
    void TakeDamage(UnityEngine.Object sender, float amount);
    void Heal(UnityEngine.Object sender, float amount);
}