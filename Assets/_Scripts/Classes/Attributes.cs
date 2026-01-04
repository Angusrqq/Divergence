using UnityEngine;
using System;
using MessagePack;

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
    public static Stat Lives = 1f;
    public static Stat PlayerDamageMult = 1f;
    public static Stat PlayerResistsMult = 1f;
    public static Stat CritChance = 0f;
    public static Stat CritMult = 1.5f;
    public static Stat ProjectilesAdd = 0;
    public static Stat ProjectileSpeedMult = 1f;
    public static Stat PierceTargets = 0;
    public static Stat CastSpeedMult = 1f;
    public static Stat CooldownReductionMult = 1f;
    public static Stat AbilityActiveTimeMult = 1f;
    public static Stat ActiveAbilitySlots = 5;
    public static Stat PassiveAbilitySlots = 5;
    public static Stat ManuallyTriggeredAbilitySlots = 0;
    public static Stat AbilitiesPerLevel = 3;
    public static Stat PassiveAbilityEffectMult = 1f;
    public static Stat MagnetRadius = 0.5f;
    public static Stat ExperienceMultiplier = 1f;
    public static Stat Luck = 0f;

    public static StatModifierByStat HealthModifier = new(ref Health, StatModifierType.Flat, GameData.instance);
    public static StatModifierByStat MaxHealthModifier = new(ref MaxHealth, StatModifierType.Flat, GameData.instance);
    public static StatModifierByStat PlayerDamageMultModifier = new(ref PlayerDamageMult, StatModifierType.Mult, GameData.instance);
    public static StatModifierByStat PlayerResistsMultModifier = new(ref PlayerResistsMult, StatModifierType.Mult, GameData.instance);
    public static StatModifierByStat ProjectilesAddModifier = new(ref ProjectilesAdd, StatModifierType.Flat, GameData.instance);
    public static StatModifierByStat PierceTargetsModifier = new(ref PierceTargets, StatModifierType.Flat, GameData.instance);
    public static StatModifierByStat CastSpeedMultModifier = new(ref CastSpeedMult, StatModifierType.Mult, GameData.instance);
    public static StatModifierByStat CooldownReductionMultModifier = new(ref CooldownReductionMult, StatModifierType.Mult, GameData.instance);
    public static StatModifierByStat AbilitiesPerLevelModifier = new(ref AbilitiesPerLevel, StatModifierType.Flat, GameData.instance);
    public static StatModifierByStat PassiveAbilityEffectMultModifier = new(ref PassiveAbilityEffectMult, StatModifierType.Mult, GameData.instance);

    public static void ReloadStats(StartingAttributesSnapshot startingAttributes)
    {
        Health = startingAttributes.Health;
        MaxHealth = startingAttributes.MaxHealth;
        Lives = startingAttributes.Lives;

        PlayerDamageMult = startingAttributes.PlayerDamageMult;
        PlayerResistsMult = startingAttributes.PlayerResistsMult;
        CritChance = startingAttributes.CritChance;
        CritMult = startingAttributes.CritMult;

        ProjectilesAdd = startingAttributes.ProjectilesAdd;
        ProjectileSpeedMult = startingAttributes.ProjectileSpeedMult;
        PierceTargets = startingAttributes.PierceTargets;

        CastSpeedMult = startingAttributes.CastSpeedMult;
        CooldownReductionMult = startingAttributes.CooldownReductionMult;
        AbilityActiveTimeMult = startingAttributes.AbilityActiveTimeMult;

        ActiveAbilitySlots = startingAttributes.ActiveAbilitySlots;
        PassiveAbilitySlots = startingAttributes.PassiveAbilitySlots;
        ManuallyTriggeredAbilitySlots = startingAttributes.ManuallyTriggeredAbilitySlots;
        AbilitiesPerLevel = startingAttributes.AbilitiesPerLevel;
        PassiveAbilityEffectMult = startingAttributes.PassiveAbilityEffectMult;

        MagnetRadius = startingAttributes.MagnetRadius;
        ExperienceMultiplier = startingAttributes.ExperienceMultiplier;
        Luck = startingAttributes.Luck;
    }
}

[MessagePackObject]
public class StartingAttributesSnapshot
{
    [Key(0)] public Stat Health = 100;
    [Key(1)] public Stat MaxHealth = 100;
    [Key(2)] public Stat Lives = 1f;
    [Key(3)] public Stat PlayerDamageMult = 1f;
    [Key(4)] public Stat PlayerResistsMult = 1f;
    [Key(5)] public Stat CritChance = 0f;
    [Key(6)] public Stat CritMult = 1.5f;
    [Key(7)] public Stat ProjectilesAdd = 0;
    [Key(8)] public Stat ProjectileSpeedMult = 1f;
    [Key(9)] public Stat PierceTargets = 0;
    [Key(10)] public Stat CastSpeedMult = 1f;
    [Key(11)] public Stat CooldownReductionMult = 1f;
    [Key(12)] public Stat AbilityActiveTimeMult = 1f;
    [Key(13)] public Stat ActiveAbilitySlots = 5;
    [Key(14)] public Stat PassiveAbilitySlots = 5;
    [Key(15)] public Stat ManuallyTriggeredAbilitySlots = 0;
    [Key(16)] public Stat AbilitiesPerLevel = 3;
    [Key(17)] public Stat PassiveAbilityEffectMult = 1f;
    [Key(18)] public Stat MagnetRadius = 0.5f;
    [Key(19)] public Stat ExperienceMultiplier = 1f;
    [Key(20)] public Stat Luck = 0f;

    public StartingAttributesSnapshot()
    {
        Health = StartingAttributes.Health;
        MaxHealth = StartingAttributes.MaxHealth;
        Lives = StartingAttributes.Lives;

        PlayerDamageMult = StartingAttributes.PlayerDamageMult;
        PlayerResistsMult = StartingAttributes.PlayerResistsMult;
        CritChance = StartingAttributes.CritChance;
        CritMult = StartingAttributes.CritMult;

        ProjectilesAdd = StartingAttributes.ProjectilesAdd;
        ProjectileSpeedMult = StartingAttributes.ProjectileSpeedMult;
        PierceTargets = StartingAttributes.PierceTargets;

        CastSpeedMult = StartingAttributes.CastSpeedMult;
        CooldownReductionMult = StartingAttributes.CooldownReductionMult;
        AbilityActiveTimeMult = StartingAttributes.AbilityActiveTimeMult;

        ActiveAbilitySlots = StartingAttributes.ActiveAbilitySlots;
        PassiveAbilitySlots = StartingAttributes.PassiveAbilitySlots;
        ManuallyTriggeredAbilitySlots = StartingAttributes.ManuallyTriggeredAbilitySlots;
        AbilitiesPerLevel = StartingAttributes.AbilitiesPerLevel;
        PassiveAbilityEffectMult = StartingAttributes.PassiveAbilityEffectMult;

        MagnetRadius = StartingAttributes.MagnetRadius;
        ExperienceMultiplier = StartingAttributes.ExperienceMultiplier;
        Luck = StartingAttributes.Luck;
    }

    [SerializationConstructor]
    public StartingAttributesSnapshot(
        float key1, float key2, float key3, float key4, float key5,
        float key6, float key7, float key8, float key9, float key10,
        float key11, float key12, float key13, float key14, float key15,
        float key16, float key17, float key18, float key19, float key20,
        float key21
    )
    {
        Health = key1;
        MaxHealth = key2;
        Lives = key3;

        PlayerDamageMult = key4;
        PlayerResistsMult = key5;
        CritChance = key6;
        CritMult = key7;

        ProjectilesAdd = key8;
        ProjectileSpeedMult = key9;
        PierceTargets = key10;

        CastSpeedMult = key11;
        CooldownReductionMult = key12;
        AbilityActiveTimeMult = key13;

        ActiveAbilitySlots = key14;
        PassiveAbilitySlots = key15;
        ManuallyTriggeredAbilitySlots = key16;
        AbilitiesPerLevel = key17;
        PassiveAbilityEffectMult = key18;

        MagnetRadius = key19;
        ExperienceMultiplier = key20;
        Luck = key21;
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
    public Stat Lives = 1f;
    public Stat PlayerDamageMult = 1f;
    public Stat PlayerResistsMult = 1f;
    public Stat CritChance = 0f;
    public Stat CritMult = 1.5f;
    public Stat ProjectilesAdd = 0;
    public Stat ProjectileSpeedMult = 1f;
    public Stat PierceTargets = 0;
    public Stat CastSpeedMult = 1f;
    public Stat CooldownReductionMult = 1f;
    public Stat AbilityActiveTimeMult = 1f;
    public Stat ActiveAbilitySlots = 5;
    public Stat PassiveAbilitySlots = 5;
    public Stat ManuallyTriggeredAbilitySlots = 0;
    public Stat AbilitiesPerLevel = 0;
    public Stat PassiveAbilityEffectMult = 1f;
    public Stat ExperienceMultiplier = 1f;
    public Stat Luck = 0f;
    public float DamageDealt = 0;
    public float DamageTaken = 0;
    public StatModifierByStat HealthModifier;
    public StatModifierByStat MaxHealthModifier;
    public StatModifierByStat PlayerDamageMultModifier;
    public StatModifierByStat PlayerResistsMultModifier;
    public StatModifierByStat CritChanceModifier;
    public StatModifierByStat CritMultModifier;
    public StatModifierByStat ProjectilesAddModifier;
    public StatModifierByStat ProjectileSpeedMultModifier;
    public StatModifierByStat PierceTargetsModifier;
    public StatModifierByStat CastSpeedMultModifier;
    public StatModifierByStat CooldownReductionMultModifier;
    public StatModifierByStat AbilityActiveTimeMultModifier;
    public StatModifierByStat AbilitiesPerLevelModifier;
    public StatModifierByStat PassiveAbilityEffectMultModifier;
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
    public InGameAtributes(
        Stat health = null,
        Stat maxHealth = null,
        Stat lives = null,
        Stat playerDamageMult = null,
        Stat playerResistsMult = null,
        Stat critChance = null,
        Stat critMult = null,
        Stat projectilesAdd = null,
        Stat projectileSpeedMult = null,
        Stat pierceTargets = null,
        Stat castSpeedMult = null,
        Stat cooldownReductionMult = null,
        Stat abilityActiveTimeMult = null,
        Stat passiveAbilityEffectMult = null,
        Stat magnetRadius = null,
        Stat experienceMultiplier = null,
        Stat luck = null
    )
    {
        Health = health ?? StartingAttributes.Health;
        MaxHealth = maxHealth ?? StartingAttributes.MaxHealth;
        Lives = lives ?? StartingAttributes.Lives;

        PlayerDamageMult = playerDamageMult ?? StartingAttributes.PlayerDamageMult;
        PlayerResistsMult = playerResistsMult ?? StartingAttributes.PlayerResistsMult;
        CritChance = critChance ?? StartingAttributes.CritChance;
        CritMult = critMult ?? StartingAttributes.CritMult;

        ProjectilesAdd = projectilesAdd ?? StartingAttributes.ProjectilesAdd;
        ProjectileSpeedMult = projectileSpeedMult ?? StartingAttributes.ProjectileSpeedMult;
        PierceTargets = pierceTargets ?? StartingAttributes.PierceTargets;

        CastSpeedMult = castSpeedMult ?? StartingAttributes.CastSpeedMult;
        CooldownReductionMult = cooldownReductionMult ?? StartingAttributes.CooldownReductionMult;
        AbilityActiveTimeMult = abilityActiveTimeMult ?? StartingAttributes.AbilityActiveTimeMult;

        PassiveAbilityEffectMult = passiveAbilityEffectMult ?? StartingAttributes.PassiveAbilityEffectMult;
        _magnetRadius = magnetRadius ?? StartingAttributes.MagnetRadius;
        ExperienceMultiplier = experienceMultiplier ?? StartingAttributes.ExperienceMultiplier;
        Luck = luck ?? StartingAttributes.Luck;

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
        ProjectileSpeedMultModifier = new(ref ProjectileSpeedMult, StatModifierType.Mult, GameData.instance, true);
        PierceTargetsModifier = new(ref PierceTargets, StatModifierType.Flat, GameData.instance);

        CastSpeedMultModifier = new(ref CastSpeedMult, StatModifierType.Mult, GameData.instance, true);
        CooldownReductionMultModifier = new(ref CooldownReductionMult, StatModifierType.Mult, GameData.instance, true);
        AbilityActiveTimeMultModifier = new(ref AbilityActiveTimeMult, StatModifierType.Mult, GameData.instance, true);

        AbilitiesPerLevelModifier = new(ref AbilitiesPerLevel, StatModifierType.Flat, GameData.instance);
        PassiveAbilityEffectMultModifier = new(ref PassiveAbilityEffectMult, StatModifierType.Mult, GameData.instance, true);
    }
}

[MessagePackObject]
public class GameStats
{
    [Key(0)] public ulong TotalTime = 0;
    [Key(1)] public ulong TotalRuns = 0;
    [Key(2)] public ulong RunsFinished = 0;
    [Key(3)] public ulong TotalCurrency = 0;
    [Key(4)] public ulong TotalKills = 0;
    [Key(5)] public ulong TotalDamageDealt = 0;
    [Key(6)] public ulong TotalDamageTaken = 0;

    public enum StatType { Time, Runs, Deaths, Currency, Kills, DamageDealt, DamageTaken, RunsFinished };
    public string GetStat(StatType type)
    {
        return type switch
        {
            StatType.Time => TotalTime.ToString(),
            StatType.Runs => TotalRuns.ToString(),
            StatType.Currency => TotalCurrency.ToString(),
            StatType.Kills => TotalKills.ToString(),
            StatType.DamageDealt => TotalDamageDealt.ToString(),
            StatType.DamageTaken => TotalDamageTaken.ToString(),
            StatType.RunsFinished => RunsFinished.ToString(),

            _ => "0"
        };
    }

    public GameStats(
        ulong totalTime,
        ulong totalRuns,
        ulong totalRunsFinished,
        ulong totalCurrency,
        ulong totalKills,
        ulong totalDamageDealt,
        ulong totalDamageTaken
    )
    {
        TotalTime = totalTime;
        TotalRuns = totalRuns;
        TotalCurrency = totalCurrency;
        TotalKills = totalKills;
        TotalDamageDealt = totalDamageDealt;
        TotalDamageTaken = totalDamageTaken;
        RunsFinished = totalRunsFinished;
    }

    public GameStats() { }
}

[MessagePackObject]
public class GameRecords
{
    [Key(0)] public uint MaxLevel = 0;
    [Key(1)] public uint MaxCurrency = 0;
    [Key(2)] public float MaxCritChance = 0f;
    [Key(3)] public float MaxCritMult = 0f;
    [Key(4)] public float MaxDamageMult = 0f;
    [Key(5)] public uint MaxDamageDealt = 0;

    public enum RecordType
    {
        Level,
        Currency,
        CritChance,
        CritMult,
        DamageMult,
        DamageDealt
    }

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
