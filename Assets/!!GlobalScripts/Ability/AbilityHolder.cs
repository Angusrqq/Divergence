using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHolder : MonoBehaviour
{
    public List<Ability> Abilities;
    private List<string> _abilityNames;
    public List<PassiveAbility> Passives;
    private List<string> _passiveNames;

    void FixedUpdate()
    {
        foreach (Ability a in Abilities)
        {
            a.FixedUpdateAbility();
        }
    }

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

    public PassiveAbility GetPassiveByName(string name)
    {
        return Passives[_passiveNames.IndexOf(name)];
    }

    public Ability GetAbilityByName(string name)
    {
        return Abilities[_abilityNames.IndexOf(name)];
    }
}

public enum AbilityState
{
    ready,
    active,
    cooldown
}
