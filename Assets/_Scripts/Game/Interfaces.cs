using System;

/// <summary>
/// <para>
/// <c>IDamageable</c> interface defines the contract for any object that can take damage and be healed.
/// </para>
/// </summary>
public interface IDamageable
{
    float Health { get; set; }
    float MaxHealth { get; set; }
    event Action<UnityEngine.Object> OnDeath;
    event Action<UnityEngine.Object, float> OnDamageTaken;
    event Action<UnityEngine.Object, float> OnHeal;
    void TakeDamage(UnityEngine.Object sender, float amount);
    void Heal(UnityEngine.Object sender, float amount);
}
