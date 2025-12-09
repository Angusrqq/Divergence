using UnityEngine;

public class HawkEye : PassiveAbilityMono
{
    private StatModifierByStat _distanceModifier;
    private Stat _distanceValue;

    public override void Activate()
    {
        _distanceValue = Ability.GetStat("Projectile Lifetime");
        _distanceValue.AddModifier(GameData.InGameAttributes.PassiveAbilityEffectMultModifier);
        _distanceModifier = new StatModifierByStat(ref _distanceValue, StatModifierType.Percent, this);
        GameData.InGameAttributes.AbilityActiveTimeMult.AddModifier(_distanceModifier);
        
        Debug.Log($"HawkEye activated, ability active time: {GameData.InGameAttributes.AbilityActiveTimeMult}, Base active time multiplier: {_distanceValue.BaseValue}, Current ability active time: {_distanceValue}");
    }
}
