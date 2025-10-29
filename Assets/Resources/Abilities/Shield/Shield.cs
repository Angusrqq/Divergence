public class Shield : PassiveAbilityMono
{
    public override void Activate()
    {
        Attributes.PlayerResistsMult -= 0.1f * Attributes.PassiveAbilityEffectMult;
    }

    public override void Upgrade()
    {
        Attributes.PlayerResistsMult -= 0.05f * Attributes.PassiveAbilityEffectMult;
    }
}
