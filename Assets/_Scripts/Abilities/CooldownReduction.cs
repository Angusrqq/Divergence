using UnityEngine;

public class CooldownReduction : PassiveAbilityMono
{
    private StatModifierByStat _cooldownModifier;
    private Stat _cooldownValue;

    public override void Activate()
    {
        _cooldownValue = -0.15f;
        _cooldownValue.AddModifier(GameData.InGameAttributes.PassiveAbilityEffectMultModifier);
        _cooldownModifier = new StatModifierByStat(ref _cooldownValue, StatModifierType.Percent, this);
        GameData.InGameAttributes.CooldownReductionMult.AddModifier(_cooldownModifier);
        Debug.Log($"CooldownReduction activated, player cooldown reduction: {GameData.InGameAttributes.CooldownReductionMult}, Base cooldown reduction: {_cooldownValue.BaseValue}, Current cooldown reduction: {_cooldownValue}");
    }

    public override void Upgrade()
    {
        _cooldownModifier.Value -= 0.05f;
        Debug.Log($"CooldownReduction upgraded, player cooldown reduction: {GameData.InGameAttributes.CooldownReductionMult}, Base cooldown reduction: {_cooldownValue.BaseValue}, Current cooldown reduction: {_cooldownValue}");
    }
}
