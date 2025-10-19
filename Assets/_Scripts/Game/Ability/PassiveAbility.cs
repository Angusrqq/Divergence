using System;
using UnityEngine;

/// <summary>
/// <para>
/// <c>PassiveAbility</c> is a ScriptableObject that represents a passive ability
/// that can be added to an <c>AbilityHolder</c>.
/// </para>
/// </summary>
public class PassiveAbility : ScriptableObject
{
    [NonSerialized] public bool IsEvolved = false;

    public string Name;
    public string Description;
    public int Level = 1;
    public int MaxLevel;
    public PassiveAbilityType Type;

    public virtual void Activate() { }
    public virtual void Upgrade() { }
    public virtual void UpdateAbility() { }
}

public enum PassiveAbilityType
{
    OneTime,
    Updated // dont know why but it will just exist for now
}
