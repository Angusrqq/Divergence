using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

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

    public enum AbilityTier
    {
        Tier1 = 1,
        Tier2,
        Tier3,
        Tier4,
        Tier5
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
        var abilitylist = unlockedAbilities.Where(x => x.Tier == tier).ToList();
        if(abilitylist.Count == 0) 
            return null; // no ability found in this tier
        
        return abilitylist[GameData.ValuableRoll(0, abilitylist.Count)];
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
                Debug.Log("Could not find ability in tier: " + tier);
            }
            pool.Remove(ability);
            result.Add(ability);
        }

        return result;
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
