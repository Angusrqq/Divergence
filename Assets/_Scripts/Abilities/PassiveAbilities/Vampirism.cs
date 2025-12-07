using System;

public class Vampirism : PassiveAbilityMono
{
    [NonSerialized] public Stat chance = 0.2f;

    public override void Activate()
    {
        GameData.player.AbilityHolder.OnEnemyHit += OnEnemyHit;
    }

    private void OnEnemyHit(Type type, Enemy enemy, float Damage, InstantiatedAbilityMono projectile)
    {
        if (GameData.LowValue < chance)
        {
            GameData.player.DamageableEntity.Heal(this, Damage * Ability.GetStat("Healing by damage"), GetType());
        }
    }
}
