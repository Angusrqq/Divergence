using UnityEngine;

public class ProjectileSpeed : PassiveAbilityMono
{
    private StatModifierByStat _speedModifier;
    private Stat _speedValue;

    public override void Activate()
    {
        _speedValue = 0.3f;
        _speedValue.AddModifier(GameData.InGameAttributes.PassiveAbilityEffectMultModifier);
        _speedModifier = new StatModifierByStat(ref _speedValue, StatModifierType.Percent, this);
        GameData.InGameAttributes.ProjectileSpeedMult.AddModifier(_speedModifier);
        Debug.Log($"ProjectileSpeed activated, projectile speed: {GameData.InGameAttributes.ProjectileSpeedMult}, Base speed: {_speedValue.BaseValue}, Current speed: {_speedValue}");
    }

    public override void Upgrade()
    {
        _speedValue.BaseValue += 0.2f;
        Debug.Log($"ProjectileSpeed upgraded, projectile speed: {GameData.InGameAttributes.ProjectileSpeedMult}, Base speed: {_speedValue.BaseValue}, Current speed: {_speedValue}");
    }
}
