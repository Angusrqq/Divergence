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
    event Action<UnityEngine.Object, float, Type> OnDamageTaken;
    event Action<UnityEngine.Object, float, Type> OnHeal;
    
    void TakeDamage(UnityEngine.Object sender, float amount, Type type = null);
    void Heal(UnityEngine.Object sender, float amount, Type type = null);
}
