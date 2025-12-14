using UnityEngine;

public class Shield : PassiveAbilityMono
{
    private StatModifierByStat _shieldModifier;
    private Stat _resistValue;
    
    public override void Activate()
    {
        _resistValue = Ability.GetStat("Resistance");
        _resistValue.AddModifier(GameData.InGameAttributes.PassiveAbilityEffectMultModifier);
        _shieldModifier = new StatModifierByStat(ref _resistValue, StatModifierType.Percent, this);
        GameData.InGameAttributes.PlayerResistsMult.AddModifier(_shieldModifier);
        
        Debug.Log($"Shield Activated, player resistance: {GameData.InGameAttributes.PlayerResistsMult}, resistBase: {_resistValue.BaseValue}, resistCurrent {_resistValue}");
    }
}
