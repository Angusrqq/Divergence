using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseAbilityHandler : MonoBehaviour
{
    public int Level = 1;
    public int MaxLevel;
    public bool IsEvolved = false;

    protected BaseAbilityScriptable _source;
    
    public string Name => _source.Name;
    public string Description => _source.Description;
    public Sprite Icon => _source.Icon;
    public AudioSource AudioSource;
    public AbilityTier Tier;
    public Dictionary<string, RuntimeStatHolder> Stats;

    public void Init(BaseAbilityScriptable source)
    {
        Level = source.Level;
        MaxLevel = source.MaxLevel;
        _source = source;
        IsEvolved = source.IsEvolved;
        Tier = source.Tier;
        Stats = new();
        RecalculateStats();
        AudioSource = GetComponent<AudioSource>();

        AfterInit();
    }

    public virtual Stat GetStat(AbilityType type)
    {
        var stats = Stats.Where(s => s.Value.type == type).ToList();
        if (!stats.Any())
        {
            Debug.LogWarning($"Attempt to access the non-existent stat type '{type}' in ability '{Name}'");
            return 0;
        }
        if(stats.Count > 1) 
        {
            Debug.LogWarning($"Multiple stats with the same type '{type}' exist in ability '{Name}'. Use the stat name instead.");
        }

        return stats.First().Value.stat;
    }

    public virtual Stat GetStat(string name)
    {
        if (!Stats.ContainsKey(name))
        {
            Debug.LogWarning($"Attempt to access the non-existent stat '{name}' in ability '{Name}'");
            return 0;
        }

        return Stats[name].stat;
    }

    protected virtual void AfterInit() { }

    public virtual void Activate() { }

    public virtual void Upgrade()
    {
        Level++;
        foreach (var s in Stats)
        {
            s.Value.stat.BaseValue = _source.GetValue(s.Key, Level);
        }
    }

    public virtual void UpdateAbility() { }

    public virtual void RecalculateStats()
    {
        foreach(var s in _source.Stats)
        {
            string lookup = string.IsNullOrEmpty(s.Name) ? s.type.ToString() : s.Name;
            if (!Stats.ContainsKey(lookup))
            {
                Stats[lookup] = new (s.type, s.BaseValue);
            }
            
            Stats[lookup].stat.BaseValue = _source.GetValue(lookup, Level);
        }
    }
}
