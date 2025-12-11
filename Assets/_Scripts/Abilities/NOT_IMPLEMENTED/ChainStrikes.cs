using System;
using System.Collections.Generic;

//TODO: implement
// maybe some kind of ui elements to tell player what combo they're on 
public class ChainStrikes : PassiveAbilityMono
{
    private List<InstantiatedAbilityMono> trackedProjectiles = new();
    private int comboCounter = 0;
    private StatModifier damageModifier;

    private void OnProjectileFired(Type abilityType, InstantiatedAbilityMono projectile)
    {
        trackedProjectiles.Add(projectile);
        projectile.OnDeath += OnProjectileDeath;
    }

    private void OnProjectileHitEnemy(Type type, Enemy enemy, float damage, InstantiatedAbilityMono projectile)
    {
        if (!trackedProjectiles.Contains(projectile)) return;
        if (comboCounter < Ability.GetStat("Max Combo"))
        {
            comboCounter++;
            damageModifier.Value = comboCounter * Ability.GetStat("Combo Damage Bonus");
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
}
