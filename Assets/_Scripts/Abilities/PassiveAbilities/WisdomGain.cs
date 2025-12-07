using UnityEngine;

public class WisdomGain : PassiveAbilityMono
{
    private StatModifierByStat _expModifier; // TODO: Rename to _experienceWisdomModifier
    private Stat _expValue;

    public override void Activate()
    {
        _expValue = Ability.GetStat("Experience multiplier");
        _expValue.AddModifier(GameData.InGameAttributes.PassiveAbilityEffectMultModifier);
        _expModifier = new StatModifierByStat(ref _expValue, StatModifierType.Mult, this);
        GameData.InGameAttributes.ExperienceMultiplier.AddModifier(_expModifier);
        Debug.Log($"WisdomGain activated, player exp: {GameData.InGameAttributes.ExperienceMultiplier}, Base exp multiplier: {_expValue.BaseValue}, Current exp multiplier: {_expValue}");
    }

    // public override void Upgrade()
    // {
    //     _expModifier.Value *= 2f;
    //     Debug.Log($"WisdomGain upgraded, player exp: {GameData.InGameAttributes.ExperienceMultiplier}, Base exp multiplier: {_expValue.BaseValue}, Current exp multiplier: {_expValue}");
    // }
}
