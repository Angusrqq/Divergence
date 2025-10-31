using UnityEngine;

public class Shield : PassiveAbilityMono
{
    private StatModifierByStat shieldModifier;
    private Stat resistValue;
    public override void Activate()
    {
        resistValue = -0.1f;
        resistValue.AddModifier(Attributes.PassiveAbilityEffectMultModifier);
        shieldModifier = new(ref resistValue, StatModifierType.Percent, this);
        Attributes.PlayerResistsMult.AddModifier(shieldModifier);
        Debug.Log($"Shield Activated, player resist: {Attributes.PlayerResistsMult}, resistBase: {resistValue.BaseValue}, resistCurrent {resistValue}");
    }

    public override void Upgrade()
    {
        resistValue -= 0.05f;
        Debug.Log($"Shield Upgraded, player resist: {Attributes.PlayerResistsMultModifier}, resistBase: {resistValue.BaseValue}, resistCurrent {resistValue}");
    }
}
