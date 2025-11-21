using UnityEngine;

public class CriticalStrike : PassiveAbilityMono
{
    private StatModifier _critChanceModifier;
    public override void Activate()
    {
        _critChanceModifier = new StatModifier(0.1f, StatModifierType.Flat, this);
        GameData.InGameAttributes.CritChance.AddModifier(_critChanceModifier);
    }

    public override void Upgrade()
    {
        _critChanceModifier.Value += 0.05f;
    }
}
