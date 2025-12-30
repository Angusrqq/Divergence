using System.Collections.Generic;
using System.Linq;

public class BaseAbilityScriptable : BaseScriptableObjectUnlockable
{
    public AbilityTier Tier;
    public int Level = 1;
    public int MaxLevel;
    public List<AbilityStatDefinition> Stats;
    public bool IsEvolved = false;

    public virtual HandlerType Type => HandlerType.BaseAbility;
    public virtual BaseAbilityMono Behaviour { get; protected set; }

    public float GetValue(AbilityType type, int level)
    {
        var entry = Stats.Find(stat => stat.type == type);
        if (entry.Scaling == null)
        {
            return entry.BaseValue;
        }

        return entry.Scaling.Evaluate(level);
    }

    public float GetValue(string Name, int level)
    {
        var entry = Stats.Find(stat => stat.Name == Name);
        if (entry.Scaling == null)
        {
            return entry.BaseValue;
        }

        return entry.Scaling.Evaluate(level);
    }

    public virtual List<AbilityStatDefinition> GetAllStatsOfType(AbilityType type)
    {
        return Stats.Where(stat => stat.type == type).ToList();
    }
}
