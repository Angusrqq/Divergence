using System;
using UnityEngine;

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