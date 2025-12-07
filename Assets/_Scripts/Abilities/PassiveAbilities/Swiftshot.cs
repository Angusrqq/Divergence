using UnityEngine;

public class Swiftshot : PassiveAbilityMono
{
    private StatModifierByStat _speedModifier;
    private Stat _speedValue;

    public override void Activate()
    {
        _speedValue = Ability.GetStat("Projectile speed");
        _speedValue.AddModifier(GameData.InGameAttributes.PassiveAbilityEffectMultModifier);
        _speedModifier = new StatModifierByStat(ref _speedValue, StatModifierType.Percent, this);
        GameData.InGameAttributes.ProjectileSpeedMult.AddModifier(_speedModifier);
        
        Debug.Log($"Swiftshot activated, projectile speed: {GameData.InGameAttributes.ProjectileSpeedMult}, Base speed: {_speedValue.BaseValue}, Current speed: {_speedValue}");
    }
}
