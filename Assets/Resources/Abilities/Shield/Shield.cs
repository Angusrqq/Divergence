using UnityEngine;

/// <summary>
/// Passive ability that increases the player's damage resistance via stat modifiers.
/// </summary>
public class Shield : PassiveAbilityMono
{
    private StatModifierByStat _shieldModifier;
    private Stat _resistValue;
    
    /// <summary>
    /// Applies the initial resistance modifier scaled by passive ability effect multipliers.
    /// </summary>
    public override void Activate()
    {
        _resistValue = -0.1f;
        _resistValue.AddModifier(Attributes.PassiveAbilityEffectMultModifier);
        _shieldModifier = new(ref _resistValue, StatModifierType.Percent, this);
        Attributes.PlayerResistsMult.AddModifier(_shieldModifier);
        Debug.Log($"Shield Activated, player resist: {Attributes.PlayerResistsMult}, resistBase: {_resistValue.BaseValue}, resistCurrent {_resistValue}");
    }

    /// <summary>
    /// Strengthens the shield by increasing the magnitude of the resistance.
    /// </summary>
    public override void Upgrade()
    {
        _resistValue -= 0.05f;
        Debug.Log($"Shield Upgraded, player resist: {Attributes.PlayerResistsMultModifier}, resistBase: {_resistValue.BaseValue}, resistCurrent {_resistValue}");
    }
}
