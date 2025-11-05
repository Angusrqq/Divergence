using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum AbilityState
{
    ready,
    active,
    cooldown
}

/// <summary>
/// <para>
/// <c>AbilityHolder</c> is a class that holds and handles the abilities of a character.
/// </para>
/// Can add abilities and passives, and handle their upgrades.
/// </summary>
public class AbilityHolder : MonoBehaviour
{
    public List<BaseAbility> Abilities;
    public List<BaseAbility> Passives;

    private readonly List<string> _abilityNames = new();
    private readonly List<string> _passiveNames = new();

    void Update()
    {
        foreach (Ability a in Abilities.Cast<Ability>())
        {
            a.UpdateAbility();
        }
    }

    /// <summary>
    /// <para>
    /// Adds passed <c>Ability</c> to the <c>Abilities</c> List if <c>_abilityNames</c> does not contain the name of added ability.
    /// </para>
    /// else, if the ability is already present and its level is less than its max level, upgrades the ability.
    /// </summary>
    /// <param name="a"></param>
    public void AddAbility(BaseAbility a)
    {
        if (_abilityNames.Contains(a.name))
        {
            BaseAbility T = GetAbilityByName(a.name);
            if (T.Level < T.MaxLevel)
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
    public void AddPassive(BaseAbility passive)
    {
        if (_passiveNames.Contains(passive.name))
        {
            BaseAbility T = GetPassiveByName(passive.name);
            if (T.Level < T.MaxLevel)
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
    public BaseAbility GetPassiveByName(string name)
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
    public BaseAbility GetAbilityByName(string name)
    {
        return Abilities[_abilityNames.IndexOf(name)];
    }
}
