using System;
using System.Collections.Generic;
using UnityEngine;
//TODO: implement
// maybe some kind of ui elements to tell player what combo they're on 
public class ComboStrikes : PassiveAbilityMono
{
    private List<InstantiatedAbilityMono> trackedProjectiles = new();
    private int comboCounter = 0;
    private int maxCombo = 5;
    private float percentDamagePerCombo = 0.01f;
    private StatModifier damageModifier;

    private void OnProjectileFired(Type abilityType, InstantiatedAbilityMono projectile)
    {
        trackedProjectiles.Add(projectile);
        projectile.OnDeath += OnProjectileDeath;
    }

    private void OnProjectileHitEnemy(Type type, Enemy enemy, float damage, InstantiatedAbilityMono projectile)
    {
        if (!trackedProjectiles.Contains(projectile)) return;
        if (comboCounter < maxCombo)
        {
            comboCounter++;
            damageModifier.Value = comboCounter * percentDamagePerCombo;
        } 
        projectile.OnDeath -= OnProjectileDeath;
        trackedProjectiles.Remove(projectile);
    }

    private void OnProjectileDeath(InstantiatedAbilityMono projectile)
    {
        if (!trackedProjectiles.Contains(projectile)) return;
        comboCounter = 0;
        projectile.OnDeath -= OnProjectileDeath;
        trackedProjectiles.Remove(projectile);
    }

    public override void Activate()
    {
        damageModifier = new(0f, StatModifierType.Percent, this);
        GameData.InGameAttributes.PlayerDamageMult.AddModifier(damageModifier);
        GameData.player.AbilityHolder.OnProjectileFired += OnProjectileFired;
        GameData.player.AbilityHolder.OnEnemyHit += OnProjectileHitEnemy;
    }

    public override void Upgrade()
    {
        maxCombo += 3;
        percentDamagePerCombo += 0.02f;
    }
}
