using System;
using System.Collections.Generic;
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
    public List<BaseAbilityHandler> Abilities;
    public List<BaseAbilityHandler> Passives;
    public GameObject ParentHolder;

    private readonly List<string> _abilityNames = new();
    private readonly List<string> _passiveNames = new();

    void Update()
    {
        foreach (AbilityHandler a in Abilities)
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
    //TODO: refactor the whole ability system, currently it's a mess (im meaning not just changing the naming, but make the code more concise/readable).
    //Example: currently AddAbility adds the Instantiated version. Either rename it or make it use the base Ability class or split the functions(if splitting, where is DRY???)
    public void AddAbility(Ability ability)
    {
        if (_abilityNames.Contains(ability.name))
        {
            BaseAbilityHandler temp = GetAbilityByName(ability.name);
            if (temp.Level < temp.MaxLevel)
            {
                temp.Upgrade();
            }
            return;
        }
        
        BaseAbilityHandler abilityInstance = CreateHandler(ability.Type, ability.Name);
        abilityInstance.Init(ability);
        Abilities.Add(abilityInstance);
        _abilityNames.Add(abilityInstance.Name);
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
        if (_passiveNames.Contains(passive.Name))
        {
            BaseAbilityHandler temp = GetPassiveByName(passive.Name);
            if (temp.Level < temp.MaxLevel)
            {
                temp.Upgrade();
            }
            return;
        }
        BaseAbilityHandler passiveInstance = CreateHandler(passive.Type, passive.Name);
        passiveInstance.Init(passive);
        passiveInstance.Activate();
        Passives.Add(passiveInstance);
        _passiveNames.Add(passiveInstance.Name);
    }

    /// <summary>
    /// <para>
    /// Returns the <c>PassiveAbility</c> from the <c>Passives</c> List with the passed name.
    /// </para>
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public BaseAbilityHandler GetPassiveByName(string name)
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
    public BaseAbilityHandler GetAbilityByName(string name)
    {
        return Abilities[_abilityNames.IndexOf(name)];
    }

    public BaseAbilityHandler CreateHandler(HandlerType type, string abilityName = "Ability")
    {
        GameObject container = new(abilityName + "Handler");
        container.transform.parent = ParentHolder.transform;
        return type switch
        {
            HandlerType.BaseAbility => container.AddComponent<BaseAbilityHandler>(),
            HandlerType.Ability => container.AddComponent<AbilityHandler>(),
            HandlerType.InstantiatedAbility => container.AddComponent<InstantiatedAbilityHandler>(),
            HandlerType.Passive => container.AddComponent<PassiveAbilityHandler>(),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
        };
    }
}
