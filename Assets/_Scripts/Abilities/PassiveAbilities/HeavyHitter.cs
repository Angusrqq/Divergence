using UnityEngine;

public class HeavyHitter : PassiveAbilityMono
{
    private StatModifierByStat _hitModifier;
    private Stat _damage;

    public override void Activate()
    {
        _damage = Ability.GetStat("Damage");
        _damage.AddModifier(GameData.InGameAttributes.PassiveAbilityEffectMultModifier);
        _hitModifier = new StatModifierByStat(ref _damage, StatModifierType.Percent, this);
        GameData.InGameAttributes.PlayerDamageMult.AddModifier(_hitModifier);
        Debug.Log($"HeavyHitter activated, player damage: {GameData.InGameAttributes.PlayerDamageMult}, Base damage multiplier: {_damage.BaseValue}, Current damage multiplier: {_damage}");
    }

    // public override void Upgrade()
    // {
    //     _hitModifier.Value *= 1.5f;
    //     Debug.Log($"HeavyHitter upgraded, player damage: {GameData.InGameAttributes.PlayerDamageMult}, Base damage multiplier: {_damage.BaseValue}, Current damage multiplier: {_damage}");
    // }
}
