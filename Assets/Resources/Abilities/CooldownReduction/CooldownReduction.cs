using UnityEngine;

public class CooldownReduction : PassiveAbilityMono
{
    private StatModifierByStat _cooldownModifier;
    private Stat _cooldownValue;

    public override void Activate()
    {
        _cooldownValue = -0.15f;
        _cooldownValue.AddModifier(Attributes.PassiveAbilityEffectMultModifier);
        _cooldownModifier = new StatModifierByStat(ref _cooldownValue, StatModifierType.Percent, this);
        Attributes.CooldownReductionMult.AddModifier(_cooldownModifier);
        Debug.Log($"CooldownReduction activated, player cooldown reduction: {Attributes.CooldownReductionMult}, Base cooldown reduction: {_cooldownValue.BaseValue}, Current cooldown reduction: {_cooldownValue}");
    }

    public override void Upgrade()
    {
        _cooldownValue.BaseValue -= 0.05f;
        Debug.Log($"CooldownReduction upgraded, player cooldown reduction: {Attributes.CooldownReductionMult}, Base cooldown reduction: {_cooldownValue.BaseValue}, Current cooldown reduction: {_cooldownValue}");
    }
}
