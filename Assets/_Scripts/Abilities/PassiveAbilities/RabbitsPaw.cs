public class RabbitsPaw : PassiveAbilityMono
{
    private StatModifierByStat _luckModifier;
    private Stat _luck;

    public override void Activate()
    {
        _luck = Ability.GetStat("Luck");
        _luckModifier = new StatModifierByStat(ref _luck, StatModifierType.Flat, this);
        GameData.InGameAttributes.Luck.AddModifier(_luckModifier);
    }
}
