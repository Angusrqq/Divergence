using UnityEngine;

public class Fleetfoot : PassiveAbilityMono
{
    private StatModifierByStat _speedModifier;
    private Stat _speedValue;

    public override void Activate()
    {
        _speedValue = Ability.GetStat("Movement speed");
        _speedValue.AddModifier(GameData.InGameAttributes.PassiveAbilityEffectMultModifier);
        _speedModifier = new StatModifierByStat(ref _speedValue, StatModifierType.Percent, this);
        GameData.player.MovementSpeed.AddModifier(_speedModifier);

        Debug.Log($"Fleetfoot activated, player speed: {GameData.player.MovementSpeed}, Base speed: {_speedValue.BaseValue}, Current speed: {_speedValue}");
    }
}
