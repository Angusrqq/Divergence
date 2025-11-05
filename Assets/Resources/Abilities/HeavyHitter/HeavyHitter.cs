using UnityEngine;

public class HeavyHitter : PassiveAbilityMono
{
    private StatModifierByStat _hitModifier;
    private Stat _hitValue;

    public override void Activate()
    {
        _hitValue = 0.333f;
        _hitValue.AddModifier(Attributes.PassiveAbilityEffectMultModifier);
        _hitModifier = new StatModifierByStat(ref _hitValue, StatModifierType.Percent, this);
        Attributes.PlayerDamageMult.AddModifier(_hitModifier);
        Debug.Log($"HeavyHitter activated, player damage: {Attributes.PlayerDamageMult}, Base damage multiplier: {_hitValue.BaseValue}, Current damage multiplier: {_hitValue}");
    }

    public override void Upgrade()
    {
        _hitValue.BaseValue *= 1.5f;
        Debug.Log($"HeavyHitter upgraded, player damage: {Attributes.PlayerDamageMult}, Base damage multiplier: {_hitValue.BaseValue}, Current damage multiplier: {_hitValue}");
    }
}
