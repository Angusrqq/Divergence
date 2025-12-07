using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class BaseAbilityScriptable : BaseScriptableObjectUnlockable
{
    public int Level = 1;
    public int MaxLevel;
    public bool IsEvolved = false;
    public virtual HandlerType Type => HandlerType.BaseAbility;
    public AbilityTier Tier;
    
    public virtual BaseAbilityMono Behaviour {get; protected set;}

    public List<AbilityStatDefinition> Stats;


    public float GetValue(AbilityType type, int level)
    {
        var entry = Stats.Find(s => s.type == type);
        if(entry.Scaling == null)
            return entry.BaseValue;
        
        return entry.Scaling.Evaluate(level);
    }

    public float GetValue(string Name, int level)
    {
        var entry = Stats.Find(s => s.Name == Name);
        if(entry.Scaling == null)
            return entry.BaseValue;
        
        return entry.Scaling.Evaluate(level);
    }

    public virtual List<AbilityStatDefinition> GetAllStatsOfType(AbilityType type) => Stats.Where(s => s.type == type).ToList();
}
