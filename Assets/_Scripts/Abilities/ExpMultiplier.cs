using UnityEngine;

public class ExpMultiplier : PassiveAbilityMono
{
    private StatModifierByStat _expModifier;
    private Stat _expValue;

    public override void Activate()
    {
        _expValue = 0.25f;
        _expValue.AddModifier(GameData.InGameAttributes.PassiveAbilityEffectMultModifier);
        _expModifier = new StatModifierByStat(ref _expValue, StatModifierType.Mult, this);
        GameData.InGameAttributes.ExperienceMultiplier.AddModifier(_expModifier);
        Debug.Log($"ExpMultiplier activated, player exp: {GameData.InGameAttributes.ExperienceMultiplier}, Base exp multiplier: {_expValue.BaseValue}, Current exp multiplier: {_expValue}");
    }

    public override void Upgrade()
    {
        _expModifier.Value *= 2f;
        Debug.Log($"ExpMultiplier upgraded, player exp: {GameData.InGameAttributes.ExperienceMultiplier}, Base exp multiplier: {_expValue.BaseValue}, Current exp multiplier: {_expValue}");
    }
}
