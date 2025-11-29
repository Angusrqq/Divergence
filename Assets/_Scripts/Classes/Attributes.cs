using UnityEngine;
using System;

public enum AttributeId
{
    MagnetRadius
}

/// <summary>
/// StartingAttributes static class holds the default starting values for various player attributes.
/// <para>
/// Used to initialize the player's attributes at the beginning of the game.
/// </para>
/// <para>
/// The values of attributes should change only through main menu upgrades, unlocks, etc., or loaded from a save file.
/// </para>
/// Also holds corresponding <see cref="StatModifierByStat"/> instances for each attribute to facilitate modifications during gameplay.
/// </summary>
public static class StartingAttributes
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
    public static Stat PassiveAbilityEffectMult = 1f;
    public static Stat PierceTargets = 0;
    public static Stat MagnetRadius = 0.5f;
    public static Stat ExperienceMultiplier = 1f;
    public static Stat ProjectileSpeedMult = 1f;
    public static Stat AbilityActiveTimeMult = 1f;
    public static Stat CritChance = 0f;
    public static Stat CritMult = 1.5f;
    public static Stat Lives = 1f;

    public static void ReloadStats(StartingAttributesSnapshot startingAttributes)
    {
        Health = startingAttributes.Health;
        MaxHealth = startingAttributes.MaxHealth;
        PlayerDamageMult = startingAttributes.PlayerDamageMult;
        PlayerResistsMult = startingAttributes.ProjectilesAdd;
        ProjectilesAdd = startingAttributes.ProjectilesAdd;
        CastSpeedMult = startingAttributes.CastSpeedMult;
        CooldownReductionMult = startingAttributes.CooldownReductionMult;
        ActiveAbilitySlots = startingAttributes.ActiveAbilitySlots;
        AbilitiesPerLevel = startingAttributes.AbilitiesPerLevel;
        PassiveAbilitySlots = startingAttributes.PassiveAbilitySlots;
        ManuallyTriggeredAbilitySlots = startingAttributes.ManuallyTriggeredAbilitySlots;
        PassiveAbilityEffectMult = startingAttributes.PassiveAbilityEffectMult;
        PierceTargets = startingAttributes.PierceTargets;
        MagnetRadius = startingAttributes.MagnetRadius;
        ExperienceMultiplier = startingAttributes.ExperienceMultiplier;
        ProjectileSpeedMult = startingAttributes.ProjectileSpeedMult;
        AbilityActiveTimeMult = startingAttributes.AbilityActiveTimeMult;
        CritChance = startingAttributes.CritChance;
        CritMult = startingAttributes.CritMult;
        Lives = startingAttributes.Lives;
    }

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
}


public class StartingAttributesSnapshot
{
    public Stat Health = 100;
    public Stat MaxHealth = 100;
    public Stat PlayerDamageMult = 1f;
    public Stat PlayerResistsMult = 1f;
    public Stat ProjectilesAdd = 0;
    public Stat CastSpeedMult = 1f;
    public Stat CooldownReductionMult = 1f;
    public Stat ActiveAbilitySlots = 5;
    public Stat AbilitiesPerLevel = 3;
    public Stat PassiveAbilitySlots = 5;
    public Stat ManuallyTriggeredAbilitySlots = 0;
    public Stat PassiveAbilityEffectMult = 1f;
    public Stat PierceTargets = 0;
    public Stat MagnetRadius = 0.5f;
    public Stat ExperienceMultiplier = 1f;
    public Stat ProjectileSpeedMult = 1f;
    public Stat AbilityActiveTimeMult = 1f;
    public Stat CritChance = 0f;
    public Stat CritMult = 1.5f;
    public Stat Lives = 1f;

    public StartingAttributesSnapshot()
    {
        Health = StartingAttributes.Health;
        MaxHealth = StartingAttributes.MaxHealth;
        PlayerDamageMult = StartingAttributes.PlayerDamageMult;
        PlayerResistsMult = StartingAttributes.ProjectilesAdd;
        ProjectilesAdd = StartingAttributes.ProjectilesAdd;
        CastSpeedMult = StartingAttributes.CastSpeedMult;
        CooldownReductionMult = StartingAttributes.CooldownReductionMult;
        ActiveAbilitySlots = StartingAttributes.ActiveAbilitySlots;
        AbilitiesPerLevel = StartingAttributes.AbilitiesPerLevel;
        PassiveAbilitySlots = StartingAttributes.PassiveAbilitySlots;
        ManuallyTriggeredAbilitySlots = StartingAttributes.ManuallyTriggeredAbilitySlots;
        PassiveAbilityEffectMult = StartingAttributes.PassiveAbilityEffectMult;
        PierceTargets = StartingAttributes.PierceTargets;
        MagnetRadius = StartingAttributes.MagnetRadius;
        ExperienceMultiplier = StartingAttributes.ExperienceMultiplier;
        ProjectileSpeedMult = StartingAttributes.ProjectileSpeedMult;
        AbilityActiveTimeMult = StartingAttributes.AbilityActiveTimeMult;
        CritChance = StartingAttributes.CritChance;
        CritMult = StartingAttributes.CritMult;
        Lives = StartingAttributes.Lives;
    }
}


/// <summary>
/// InGameAtributes class represents the player's attributes during gameplay.
/// <para>
/// It holds instances of <see cref="Stat"/> for each attribute, initialized with either the starting values from <see cref="StartingAttributes"/> or custom values.
/// </para>
/// </summary>
public class InGameAtributes
{
    public Stat Health = 100;
    public Stat MaxHealth = 100;
    public Stat PlayerDamageMult = 1f;
    public Stat PlayerResistsMult = 1f;
    public Stat ProjectilesAdd = 0;
    public Stat ProjectileSpeedMult = 1f;
    public Stat AbilityActiveTimeMult = 1f;
    public Stat CastSpeedMult = 1f;
    public Stat CooldownReductionMult = 1f;
    public Stat ActiveAbilitySlots = 5;
    public Stat AbilitiesPerLevel = 0;
    public Stat PassiveAbilitySlots = 5;
    public Stat ManuallyTriggeredAbilitySlots = 0;
    public Stat PassiveAbilityEffectMult = 1f;
    public Stat PierceTargets = 0;
    public Stat ExperienceMultiplier = 1f;
    public Stat CritChance = 0f;
    public Stat CritMult = 1.5f;
    public Stat Lives = 1f;

    public StatModifierByStat HealthModifier;
    public StatModifierByStat MaxHealthModifier;
    public StatModifierByStat PlayerDamageMultModifier;
    public StatModifierByStat PlayerResistsMultModifier;
    public StatModifierByStat ProjectilesAddModifier;
    public StatModifierByStat ProjectileSpeedMultModifier;
    public StatModifierByStat AbilityActiveTimeMultModifier;
    public StatModifierByStat CastSpeedMultModifier;
    public StatModifierByStat CooldownReductionMultModifier;
    public StatModifierByStat AbilitiesPerLevelModifier;
    public StatModifierByStat PassiveAbilityEffectMultModifier;
    public StatModifierByStat PierceTargetsModifier;
    public event Action<AttributeId, Stat> OnAttributeChanged;

    private Stat _magnetRadius = 0.5f;

    public Stat MagnetRadius
    {
        get => _magnetRadius;
        set
        {
            if (Mathf.Approximately(_magnetRadius, value)) return;
            _magnetRadius = value;
            OnAttributeChanged?.Invoke(AttributeId.MagnetRadius, _magnetRadius);
        }
    }

    /// <summary>
    /// <c>InGameAtributes</c> constructor initializes a new instance of the InGameAtributes class with the specified or default attribute values.
    /// <para>
    /// It takes optional parameters for each attribute and assigns them to the corresponding <see cref="Stat"/> instance.
    /// </para>
    /// </summary>
    public InGameAtributes(Stat health = null, Stat maxHealth = null, Stat playerDamageMult = null, Stat playerResistsMult = null, Stat projectilesAdd = null,
    Stat castSpeedMult = null, Stat cooldownReductionMult = null, Stat passiveAbilityEffectMult = null, Stat pierceTargets = null, Stat magnetRadius = null,
    Stat experienceMultiplier = null, Stat abilityActiveTimeMult = null, Stat projectileSpeedMult = null, Stat critChance = null, Stat critMult = null, Stat lives = null)
    {
        Health = health ?? StartingAttributes.Health;
        MaxHealth = maxHealth ?? StartingAttributes.MaxHealth;
        PlayerDamageMult = playerDamageMult ?? StartingAttributes.PlayerDamageMult;
        PlayerResistsMult = playerResistsMult ?? StartingAttributes.PlayerResistsMult;
        ProjectilesAdd = projectilesAdd ?? StartingAttributes.ProjectilesAdd;
        CastSpeedMult = castSpeedMult ?? StartingAttributes.CastSpeedMult;
        CooldownReductionMult = cooldownReductionMult ?? StartingAttributes.CooldownReductionMult;
        PassiveAbilityEffectMult = passiveAbilityEffectMult ?? StartingAttributes.PassiveAbilityEffectMult;
        PierceTargets = pierceTargets ?? StartingAttributes.PierceTargets;
        _magnetRadius = magnetRadius ?? StartingAttributes.MagnetRadius;
        ExperienceMultiplier = experienceMultiplier ?? StartingAttributes.ExperienceMultiplier;
        ProjectileSpeedMult = projectileSpeedMult ?? StartingAttributes.ProjectileSpeedMult;
        AbilityActiveTimeMult = abilityActiveTimeMult ?? StartingAttributes.AbilityActiveTimeMult;
        CritChance = critChance ?? StartingAttributes.CritChance;
        CritMult = critMult ?? StartingAttributes.CritMult;
        Lives = lives ?? StartingAttributes.Lives;

        ActiveAbilitySlots = StartingAttributes.ActiveAbilitySlots;
        PassiveAbilitySlots = StartingAttributes.PassiveAbilitySlots;
        ManuallyTriggeredAbilitySlots = StartingAttributes.ManuallyTriggeredAbilitySlots;
        AbilitiesPerLevel = StartingAttributes.AbilitiesPerLevel;

        CreateModifiers();
    }

    /// <summary>
    /// <c>CreateModifiers</c> method initializes the <see cref="StatModifierByStat"/> instances for each attribute.
    /// </summary>
    private void CreateModifiers()
    {
        HealthModifier = new(ref Health, StatModifierType.Flat, GameData.instance);
        MaxHealthModifier = new(ref MaxHealth, StatModifierType.Flat, GameData.instance);
        PlayerDamageMultModifier = new(ref PlayerDamageMult, StatModifierType.Mult, GameData.instance, true);
        PlayerResistsMultModifier = new(ref PlayerResistsMult, StatModifierType.Mult, GameData.instance);
        ProjectilesAddModifier = new(ref ProjectilesAdd, StatModifierType.Flat, GameData.instance);
        CastSpeedMultModifier = new(ref CastSpeedMult, StatModifierType.Mult, GameData.instance, true);
        CooldownReductionMultModifier = new(ref CooldownReductionMult, StatModifierType.Mult, GameData.instance, true);
        AbilitiesPerLevelModifier = new(ref AbilitiesPerLevel, StatModifierType.Flat, GameData.instance);
        PassiveAbilityEffectMultModifier = new(ref PassiveAbilityEffectMult, StatModifierType.Mult, GameData.instance, true);
        PierceTargetsModifier = new(ref PierceTargets, StatModifierType.Flat, GameData.instance);
        ProjectileSpeedMultModifier = new(ref ProjectileSpeedMult, StatModifierType.Mult, GameData.instance, true);
        AbilityActiveTimeMultModifier = new(ref AbilityActiveTimeMult, StatModifierType.Mult, GameData.instance, true);
    }
}

public class GameStats
{
    public ulong TotalTime = 0;
    public ulong TotalRuns = 0;
    public ulong RunsFinished = 0;
    //public ulong TotalDeaths = 0;
    public ulong TotalCurrency = 0;
    public ulong TotalKills = 0;
    public ulong TotalDamageDealt = 0;
    public ulong TotalDamageTaken = 0;

    public enum StatType { Time, Runs, Deaths, Currency, Kills, DamageDealt, DamageTaken, RunsFinished };

    public string GetStat(StatType type)
    {
        return type switch
        {
            StatType.Time => TotalTime.ToString(),
            StatType.Runs => TotalRuns.ToString(),
            //StatType.Deaths => TotalDeaths.ToString(),
            StatType.Currency => TotalCurrency.ToString(),
            StatType.Kills => TotalKills.ToString(),
            StatType.DamageDealt => TotalDamageDealt.ToString(),
            StatType.DamageTaken => TotalDamageTaken.ToString(),
            StatType.RunsFinished => RunsFinished.ToString(),
            _ => "0"
        };
    }

    public GameStats(ulong totalTime, ulong totalRuns, ulong totalCurrency, ulong totalKills, ulong totalDamageDealt, ulong totalDamageTaken, ulong totalRunsFinished)
    {
        TotalTime = totalTime;
        TotalRuns = totalRuns;
        //TotalDeaths = totalDeaths;
        TotalCurrency = totalCurrency;
        TotalKills = totalKills;
        TotalDamageDealt = totalDamageDealt;
        TotalDamageTaken = totalDamageTaken;
        RunsFinished = totalRunsFinished;
    }
    public GameStats(){}
}

public class GameRecords
{
    public uint MaxLevel = 0;
    public uint MaxCurrency = 0;
    public float MaxCritChance = 0f;
    public float MaxCritMult = 0f;
    public float MaxDamageMult = 0f;
    public uint MaxDamageDealt = 0;

    public enum RecordType { Level, Currency, CritChance, CritMult, DamageMult, DamageDealt };

    public string GetRecord(RecordType type)
    {
        return type switch
        {
            RecordType.Level => MaxLevel.ToString(),
            RecordType.Currency => MaxCurrency.ToString(),
            RecordType.CritChance => MaxCritChance.ToString("F2"),
            RecordType.CritMult => MaxCritMult.ToString("F2"),
            RecordType.DamageMult => MaxDamageMult.ToString("F2"),
            RecordType.DamageDealt => MaxDamageDealt.ToString(),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
        };
    }

    public GameRecords(uint maxLevel, uint maxCurrency, float maxCritChance, float maxCritMult, float maxDamageMult, uint maxDamageDealt)
    {
        MaxLevel = maxLevel;
        MaxCurrency = maxCurrency;
        MaxCritChance = maxCritChance;
        MaxCritMult = maxCritMult;
        MaxDamageMult = maxDamageMult;
        MaxDamageDealt = maxDamageDealt;
    }
    public GameRecords() { }
}

