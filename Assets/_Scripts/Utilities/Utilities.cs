using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public enum EnemyType
{
    Normal,
    Boss
}

public enum AbilityTier
{
    Tier1 = 1,
    Tier2,
    Tier3,
    Tier4,
    Tier5
}

public enum AbilityType
{
    Regular,
    Chance,
    Custom,
    Percent
}

public enum UniversalAbilityType
{
    None,
    Damage,
    KnockbackForce,
    KnockbackDuration,
    Projectiles,
    ActiveTime,
    Cooldown,
    Size
}

[Serializable]
public struct AbilityStatDefinition
{
    public string Name;
    public AbilityType type;
    public float BaseValue;
    public AnimationCurve Scaling;
    public UniversalAbilityType UniversalType;
}

public struct RuntimeStatHolder
{
    public AbilityType type;
    public UniversalAbilityType universalType;
    public Stat stat;

    public RuntimeStatHolder(AbilityType type, Stat stat, UniversalAbilityType universalType = UniversalAbilityType.None) =>
        (this.type, this.stat, this.universalType) = (type, stat, universalType);
}

public static class Utilities
{
    public static async Task AsyncAnimation<T>(float speed, T start, T end, Action<T, T, float> LerpCallback = null)
    {
        float elapsedTime = 0f;
        while (elapsedTime <= 1f)
        {
            elapsedTime += Time.deltaTime * speed;
            LerpCallback.Invoke(start, end, elapsedTime);
            await Awaitable.NextFrameAsync();
        }
    }

    public static readonly Dictionary<AbilityTier, float> BaseTierWeights = new()
    {
        { AbilityTier.Tier1, 70f },
        { AbilityTier.Tier2, 20f },
        { AbilityTier.Tier3, 9f },
        { AbilityTier.Tier4, 0.99f },
        { AbilityTier.Tier5, 0.01f }
    };

    public static Dictionary<AbilityTier, float> GetModifiedWeights(float luck)
    {
        Dictionary<AbilityTier, float> modifiedWeights = new();
        foreach (var weight in BaseTierWeights)
        {
            modifiedWeights.Add(weight.Key, ModifyWeight(weight.Value, luck));
        }
        return modifiedWeights;
    }

    public const float EXPONENT_FACTOR = 0.01f;

    public static float ModifyWeight(float weight, float luck)
    {
        float exponent = 1f / (1f + (luck * EXPONENT_FACTOR));
        return Mathf.Pow(weight, exponent);
    }

    public static AbilityTier GetRandomTier(float luck)
    {
        var weights = GetModifiedWeights(luck);

        float totalWeight = weights.Values.Sum();
        float roll = GameData.ValuableValue * totalWeight;

        foreach (var weight in weights)
        {
            roll -= weight.Value;
            if (roll <= 0f)
            {
                return weight.Key;
            }
        }

        // should never happen
        Debug.LogError("Could not get random tier");
        return AbilityTier.Tier1;
    }

    public static BaseAbilityScriptable GetAbilityFromTier(List<BaseAbilityScriptable> unlockedAbilities, AbilityTier tier)
    {
        int t = (int)tier;
        while(t >= 0)
        {
            var abilitylist = unlockedAbilities.Where(x => (int)x.Tier == t).ToList();
            if(abilitylist.Count > 0) 
                return abilitylist[GameData.ValuableRoll(0, abilitylist.Count)];
            Debug.LogWarning("Could not find ability in tier: " + t);
            t--;
        }
        Debug.LogError($"Could not find ability in tiers: from {tier} to {AbilityTier.Tier1}");
        return null;
    }

    public static List<BaseAbilityScriptable> GetRandomAbilities(List<BaseAbilityScriptable> unlockedAbilities, float luck, int amount = 1)
    {
        List<BaseAbilityScriptable> result = new();
        List<BaseAbilityScriptable> pool = new(unlockedAbilities);

        for (int i = 0; i < amount; i++)
        {
            AbilityTier tier = GetRandomTier(luck);
            BaseAbilityScriptable ability = GetAbilityFromTier(pool, tier);
            if(ability == null)
            {
                if(pool.Count == 0) return result;

                ability = pool[GameData.ValuableRoll(0, pool.Count)];
            }
            pool.Remove(ability);
            result.Add(ability);
        }

        return result;
    }

    public static string FormatAbilityValue(float value, AbilityType type, string name, int floatingPoints = 1)
    {
        return type switch
        {
            AbilityType.Regular => FormatFloat(value, floatingPoints),
            AbilityType.Chance => $"{FormatFloat(value * 100f, floatingPoints)}%",
            AbilityType.Percent => (value > 0f ? "+" : "") + FormatFloat(value * 100f, floatingPoints) + "%",
            AbilityType.Custom => name switch
                { "Resist" => "+" + FormatFloat(Mathf.Abs(value) * 100f, floatingPoints) + "%",
                _ => FormatFloat(value * 100f, floatingPoints) + "%" },
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
        };
    }

    public static string FormatFloat(float value, int floatingPoints = 1)
    {
        string format = $"F{floatingPoints}";
        return value % 1 == 0 ? ((int)value).ToString() : value.ToString(format);
    }

    public static Color GetTierColor(AbilityTier tier)
    {
        return tier switch
        {
            AbilityTier.Tier1 => new Color32(168, 168, 168, 255),
            AbilityTier.Tier2 => new Color32(76, 175, 80, 255),
            AbilityTier.Tier3 => new Color32(90, 50, 134, 255),
            AbilityTier.Tier4 => new Color32(255, 215, 0, 255),
            AbilityTier.Tier5 => Color.black,
            _ => Color.white
        };
    }
    

    public static Type GetHandlerTypeByEnum(HandlerType type)
    {
        return type switch
        {
            HandlerType.BaseAbility => typeof(BaseAbilityHandler),
            HandlerType.Ability => typeof(AbilityHandler),
            HandlerType.InstantiatedAbility => typeof(InstantiatedAbilityHandler),
            HandlerType.Passive => typeof(BaseAbilityHandler),// funny low iq shit
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
        };
    }
}
