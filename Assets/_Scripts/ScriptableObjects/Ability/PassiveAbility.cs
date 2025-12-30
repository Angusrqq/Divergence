using UnityEngine;

[CreateAssetMenu(fileName = "New PassvieAbility", menuName = "Abilities/PassiveAbility")]
public class PassiveAbility : BaseAbilityScriptable
{
    [SerializeReference] public PassiveAbilityMono MonoLogic;

    public override BaseAbilityMono Behaviour => MonoLogic;
    public override HandlerType Type => HandlerType.Passive;
}
