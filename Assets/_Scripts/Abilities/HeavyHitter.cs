using UnityEngine;

public class HeavyHitter : PassiveAbilityMono
{
    private StatModifierByStat _hitModifier;
    private Stat _hitValue;

    public override void Activate()
    {
        _hitValue = 0.333f;
        _hitValue.AddModifier(GameData.InGameAttributes.PassiveAbilityEffectMultModifier);
        _hitModifier = new StatModifierByStat(ref _hitValue, StatModifierType.Percent, this);
        GameData.InGameAttributes.PlayerDamageMult.AddModifier(_hitModifier);
        Debug.Log($"HeavyHitter activated, player damage: {GameData.InGameAttributes.PlayerDamageMult}, Base damage multiplier: {_hitValue.BaseValue}, Current damage multiplier: {_hitValue}");
    }

    public override void Upgrade()
    {
        _hitModifier.Value *= 1.5f;
        Debug.Log($"HeavyHitter upgraded, player damage: {GameData.InGameAttributes.PlayerDamageMult}, Base damage multiplier: {_hitValue.BaseValue}, Current damage multiplier: {_hitValue}");
    }
}
