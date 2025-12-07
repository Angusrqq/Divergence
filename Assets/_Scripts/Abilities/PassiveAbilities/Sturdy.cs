using UnityEngine;

public class Sturdy : PassiveAbilityMono
{
    private StatModifierByStat _healthModifier;
    private Stat _healthValue;

    public override void Activate()
    {
        _healthValue = Ability.GetStat("Max health");
        _healthValue.AddModifier(GameData.InGameAttributes.PassiveAbilityEffectMultModifier);
        _healthModifier = new StatModifierByStat(ref _healthValue, StatModifierType.Percent, this);
        GameData.InGameAttributes.MaxHealth.AddModifier(_healthModifier);
        // GameData.player.MaxHealth.AddModifier(_healthModifier);
        Debug.Log($"Sturdy activated, player max health: {GameData.InGameAttributes.MaxHealth}, Base max health multiplier: {_healthValue.BaseValue}, Current max health multiplier: {_healthValue}");
    }

    // public override void Upgrade()
    // {
    //     _healthModifier.Value += 0.25f;
    //     Debug.Log($"Sturdy upgraded, player max health: {GameData.InGameAttributes.MaxHealth}, Base max health multiplier: {_healthValue.BaseValue}, Current max health multiplier: {_healthValue}");
    // }
}
