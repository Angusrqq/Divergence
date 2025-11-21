public class PassiveAbilityHandler : BaseAbilityHandler
{
    private PassiveAbilityMono _monoLogic;

    protected override void AfterInit()
    {
        PassiveAbility source = _source as PassiveAbility;
        _monoLogic = source.MonoLogic;
        base.AfterInit();
    }

    public override void Activate() => _monoLogic.Activate();

    public override void Upgrade()
    {
        base.Upgrade();
        _monoLogic.Upgrade();
    }

    public void SetMonoLogic(PassiveAbilityMono monoLogic) => _monoLogic = monoLogic;
    public override void UpdateAbility() => _monoLogic.UpdateBehaviour();
}
