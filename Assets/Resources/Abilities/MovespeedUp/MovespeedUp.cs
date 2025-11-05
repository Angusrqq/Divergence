using UnityEngine;

public class MovespeedUp : PassiveAbilityMono
{
    private StatModifierByStat _speedModifier;
    private Stat _speedValue;

    public override void Activate()
    {
        _speedValue = 0.02f;
        _speedValue.AddModifier(Attributes.PassiveAbilityEffectMultModifier);
        _speedModifier = new StatModifierByStat(ref _speedValue, StatModifierType.Percent, this);
        GameData.player.MovementSpeed.AddModifier(_speedModifier);
        Debug.Log($"MovespeedUp activated, player speed: {GameData.player.MovementSpeed}, Base speed: {_speedValue.BaseValue}, Current speed: {_speedValue}");
    }

    public override void Upgrade()
    {
        _speedValue.BaseValue *= 2.3f;
        Debug.Log($"MovespeedUp upgraded, player speed: {GameData.player.MovementSpeed}, Base speed: {_speedValue.BaseValue}, Current speed: {_speedValue}");
    }
}