
public class CriticalStrike : PassiveAbilityMono
{
    private Stat _critChance;
    private StatModifierByStat _critChanceModifier;

    public override void Activate()
    {
        _critChance = Ability.GetStat("Crit Chance");
        _critChanceModifier = new StatModifierByStat(ref _critChance, StatModifierType.Flat, this);
        GameData.InGameAttributes.CritChance.AddModifier(_critChanceModifier);
    }
}
