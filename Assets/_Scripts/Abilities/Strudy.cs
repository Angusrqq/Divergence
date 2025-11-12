using UnityEngine;

public class Strudy : PassiveAbilityMono
{
    private StatModifierByStat _healthModifier;
    private Stat _healthValue;

    public override void Activate()
    {
        _healthValue = 0.5f;
        _healthValue.AddModifier(GameData.InGameAttributes.PassiveAbilityEffectMultModifier);
        _healthModifier = new StatModifierByStat(ref _healthValue, StatModifierType.Percent, this);
        GameData.InGameAttributes.MaxHealth.AddModifier(_healthModifier);
        // GameData.player.MaxHealth.AddModifier(_healthModifier);
        Debug.Log($"Strudy activated, player max health: {GameData.InGameAttributes.MaxHealth}, Base max health multiplier: {_healthValue.BaseValue}, Current max health multiplier: {_healthValue}");
    }

    public override void Upgrade()
    {
        _healthModifier.Value += 0.25f;
        Debug.Log($"Strudy upgraded, player max health: {GameData.InGameAttributes.MaxHealth}, Base max health multiplier: {_healthValue.BaseValue}, Current max health multiplier: {_healthValue}");
    }
}
