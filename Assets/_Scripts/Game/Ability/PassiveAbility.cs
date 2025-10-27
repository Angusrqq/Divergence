using System;
using UnityEngine;

/// <summary>
/// <para>
/// <c>PassiveAbility</c> is a ScriptableObject that represents a passive ability
/// that can be added to an <c>AbilityHolder</c>.
/// </para>
/// </summary>

[CreateAssetMenu(fileName = "New PassvieAbility", menuName = "Abilities/PassiveAbility")]
public class PassiveAbility : BaseAbility
{
    public PassiveAbilityType Type;
    [SerializeReference] public PassiveAbilityMono MonoLogic;

    public override void Activate() => MonoLogic.Activate();
    public override void Upgrade()
    {
        base.Upgrade();
        MonoLogic.Upgrade();
    }
    public override void UpdateAbility() => MonoLogic.UpdateBehaviour();
}

public enum PassiveAbilityType
{
    OneTime,
    Updated // dont know why but it will just exist for now
}
