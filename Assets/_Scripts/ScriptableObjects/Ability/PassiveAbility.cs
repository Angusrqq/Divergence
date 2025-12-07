using UnityEngine;

public enum PassiveAbilityType
{
    OneTime,
    Updated // dont know why but it will just exist for now // TODO: Egor - do something with it
}

/// <summary>
/// <para>
/// <c>PassiveAbility</c> is a ScriptableObject that represents a passive ability
/// that can be added to an <c>AbilityHolder</c>.
/// </para>
/// </summary>
[CreateAssetMenu(fileName = "New PassvieAbility", menuName = "Abilities/PassiveAbility")]
public class PassiveAbility : BaseAbilityScriptable
{
    [SerializeReference] public PassiveAbilityMono MonoLogic;

    public override BaseAbilityMono Behaviour => MonoLogic;
    public PassiveAbilityType PassiveType;
    public override HandlerType Type => HandlerType.Passive;
}
