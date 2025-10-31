using UnityEngine;
using System;

public enum AttributeId
{
    MagnetRadius
}

/// <summary>
/// <para>
/// <c>Attributes</c> is a static class that contains the attributes of the player.
/// </para>
/// </summary>
public static class Attributes
{
    public static Stat Health = 100;
    public static Stat MaxHealth = 100;
    public static Stat PlayerDamageMult = 1f;
    public static Stat PlayerResistsMult = 1f;
    public static Stat ProjectilesAdd = 0;
    public static Stat CastSpeedMult = 1f;
    public static Stat CooldownReductionMult = 1f;
    public static Stat ActiveAbilitySlots = 5;
    public static Stat AbilitiesPerLevel = 3;
    public static Stat PassiveAbilitySlots = 5;
    public static Stat ManuallyTriggeredAbilitySlots = 0;
    public static Stat PassiveAbilityEffectMult = 0f;
    public static Stat PierceTargets = 0;

    public static StatModifierByStat HealthModifier = new(ref Health, StatModifierType.Flat, GameData.instance);
    public static StatModifierByStat MaxHealthModifier = new(ref MaxHealth, StatModifierType.Flat, GameData.instance);
    public static StatModifierByStat PlayerDamageMultModifier = new(ref PlayerDamageMult, StatModifierType.Mult, GameData.instance);
    public static StatModifierByStat PlayerResistsMultModifier = new(ref PlayerResistsMult, StatModifierType.Mult, GameData.instance);
    public static StatModifierByStat ProjectilesAddModifier = new(ref ProjectilesAdd, StatModifierType.Flat, GameData.instance);
    public static StatModifierByStat CastSpeedMultModifier = new(ref CastSpeedMult, StatModifierType.Mult, GameData.instance);
    public static StatModifierByStat CooldownReductionMultModifier = new(ref CooldownReductionMult, StatModifierType.Mult, GameData.instance);
    public static StatModifierByStat AbilitiesPerLevelModifier = new(ref AbilitiesPerLevel, StatModifierType.Flat, GameData.instance);
    public static StatModifierByStat PassiveAbilityEffectMultModifier = new(ref PassiveAbilityEffectMult, StatModifierType.Mult, GameData.instance);
    public static StatModifierByStat PierceTargetsModifier = new(ref PierceTargets, StatModifierType.Flat, GameData.instance);
    public static event Action<AttributeId, Stat> OnAttributeChanged;

    private static Stat _magnetRadius = 0.5f;

    public static Stat MagnetRadius
    {
        get => _magnetRadius;
        set
        {
            if (Mathf.Approximately(_magnetRadius, value)) return;
            _magnetRadius = value;
            OnAttributeChanged?.Invoke(AttributeId.MagnetRadius, _magnetRadius);
        }
    }
}
