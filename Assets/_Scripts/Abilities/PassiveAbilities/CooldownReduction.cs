using UnityEngine;

public class CooldownReduction : PassiveAbilityMono
{
    private StatModifierByStat _cooldownModifier;
    private Stat _cooldownValue;

    public override void Activate()
    {
        _cooldownValue = Ability.GetStat("Cooldown Reduction");
        _cooldownValue.AddModifier(GameData.InGameAttributes.PassiveAbilityEffectMultModifier);
        _cooldownModifier = new StatModifierByStat(ref _cooldownValue, StatModifierType.Percent, this);
        GameData.InGameAttributes.CooldownReductionMult.AddModifier(_cooldownModifier);

        Debug.Log($"CooldownReduction activated, player cooldown reduction: {GameData.InGameAttributes.CooldownReductionMult}, Base cooldown reduction: {_cooldownValue.BaseValue}, Current cooldown reduction: {_cooldownValue}");
    }
}
