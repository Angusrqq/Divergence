using UnityEngine;

public class HawkEye : PassiveAbilityMono
{
    private StatModifierByStat _distanceModifier;
    private Stat _distanceValue;

    public override void Activate()
    {
        _distanceValue = 0.05f;
        _distanceValue.AddModifier(GameData.InGameAttributes.PassiveAbilityEffectMultModifier);
        _distanceModifier = new StatModifierByStat(ref _distanceValue, StatModifierType.Percent, this);
        GameData.InGameAttributes.AbilityActiveTimeMult.AddModifier(_distanceModifier);
        Debug.Log($"HawkEye activated, ability active time: {GameData.InGameAttributes.AbilityActiveTimeMult}, Base active time multiplier: {_distanceValue.BaseValue}, Current ability active time: {_distanceValue}");
    }

    public override void Upgrade()
    {
        _distanceModifier.Value *= 2f;
        Debug.Log($"HawkEye upgraded, active time: {GameData.InGameAttributes.AbilityActiveTimeMult}, Base active time multiplier: {_distanceValue.BaseValue}, Current active time multiplier: {_distanceValue}");
    }
}
