using UnityEngine;

/// <summary>
/// Passive ability that increases the player's damage resistance via stat modifiers.
/// </summary>
public class Shield : PassiveAbilityMono
{
    private StatModifierByStat _shieldModifier;
    private Stat _resistValue;
    
    public override void Activate()
    {
        _resistValue = Ability.GetStat("Resist");
        _resistValue.AddModifier(GameData.InGameAttributes.PassiveAbilityEffectMultModifier);
        _shieldModifier = new StatModifierByStat(ref _resistValue, StatModifierType.Percent, this);
        GameData.InGameAttributes.PlayerResistsMult.AddModifier(_shieldModifier);
        
        Debug.Log($"Shield Activated, player resist: {GameData.InGameAttributes.PlayerResistsMult}, resistBase: {_resistValue.BaseValue}, resistCurrent {_resistValue}");
    }
}
