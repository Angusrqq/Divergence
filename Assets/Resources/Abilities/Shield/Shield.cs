using UnityEngine;

/// <summary>
/// Passive ability that increases the player's damage resistance via stat modifiers.
/// </summary>
public class Shield : PassiveAbilityMono
{
    private StatModifierByStat shieldModifier;
    private Stat resistValue;
    
    /// <summary>
    /// Applies the initial resistance modifier scaled by passive ability effect multipliers.
    /// </summary>
    public override void Activate()
    {
        resistValue = -0.1f;
        resistValue.AddModifier(Attributes.PassiveAbilityEffectMultModifier);
        shieldModifier = new(ref resistValue, StatModifierType.Percent, this);
        Attributes.PlayerResistsMult.AddModifier(shieldModifier);
        Debug.Log($"Shield Activated, player resist: {Attributes.PlayerResistsMult}, resistBase: {resistValue.BaseValue}, resistCurrent {resistValue}");
    }

    /// <summary>
    /// Strengthens the shield by increasing the magnitude of the resistance.
    /// </summary>
    public override void Upgrade()
    {
        resistValue -= 0.05f;
        Debug.Log($"Shield Upgraded, player resist: {Attributes.PlayerResistsMultModifier}, resistBase: {resistValue.BaseValue}, resistCurrent {resistValue}");
    }
}
