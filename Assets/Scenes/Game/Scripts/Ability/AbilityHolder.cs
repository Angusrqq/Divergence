using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// <c>AbilityHolder</c> is a class that holds and handles the abilities of a character.
/// </para>
/// Can add abilities and passives, and handle their upgrades.
/// </summary>
public class AbilityHolder : MonoBehaviour
{
    public List<Ability> Abilities;
    private List<string> _abilityNames = new();
    public List<PassiveAbility> Passives;
    private List<string> _passiveNames = new();

    /// <summary>
    /// <para>
    /// Calls <c>FixedUpdateAbility</c> for each ability in <c>Abilities</c> List.
    /// </para>
    /// </summary>
    void FixedUpdate()
    {
        foreach (Ability a in Abilities)
        {
            a.FixedUpdateAbility();
        }
    }

    /// <summary>
    /// <para>
    /// Adds passed <c>Ability</c> to the <c>Abilities</c> List if <c>_abilityNames</c> does not contain the name of added ability.
    /// </para>
    /// else, if the ability is already present and its level is less than its max level, upgrades the ability.
    /// </summary>
    /// <param name="a"></param>
    public void AddAbility(Ability a)
    {
        if (_abilityNames.Contains(a.name))
        {
            Ability T = GetAbilityByName(a.name);
            if (T.level < T.maxLevel)
            {
                T.Upgrade();
            }
            return;
        }
        Abilities.Add(a);
        _abilityNames.Add(a.name);
    }

    /// <summary>
    /// <para>
    /// Acts the same as <c>AddAbility</c>, but for <c>PassiveAbility</c>.
    /// </para>
    /// <para>
    /// Adds passed <c>PassiveAbility</c> to the <c>Passives</c> List if <c>_passiveNames</c> does not contain the name of added passive.
    /// And calls <c>PassiveAbility.Activate</c> on the added passive.
    /// </para> else, if the passive is already present and its level is less than its max level, upgrades the passive.
    /// </summary>
    /// <param name="passive"></param>
    public void AddPassive(PassiveAbility passive)
    {
        if (_passiveNames.Contains(passive.name))
        {
            PassiveAbility T = GetPassiveByName(passive.name);
            if (T.level < T.maxLevel)
            {
                T.Upgrade();
            }
            return;
        }
        passive.Activate();
        Passives.Add(passive);
    }

    /// <summary>
    /// <para>
    /// Returns the <c>PassiveAbility</c> from the <c>Passives</c> List with the passed name.
    /// </para>
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public PassiveAbility GetPassiveByName(string name)
    {
        return Passives[_passiveNames.IndexOf(name)];
    }

    /// <summary>
    /// <para>
    /// Returns the <c>Ability</c> from the <c>Abilities</c> List with the passed name.
    /// </para>
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Ability GetAbilityByName(string name)
    {
        return Abilities[_abilityNames.IndexOf(name)];
    }
}

/// <summary>
/// <para>
/// <c>AbilityState</c> is an enum that represents the state of an ability.
/// </para>
/// </summary>
public enum AbilityState
{
    ready,
    active,
    cooldown
}
