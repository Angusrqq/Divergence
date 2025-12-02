using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MessagePack;

/// <summary>
/// Should store data about metaprogression, like time knowledge (some currency like gold), unlocked characters, etc.
/// Should be serializable and saved/loaded using DataSystem.
/// </summary>
[MessagePackObject]
public class MetaprogressionData
{
    // TODO: Egor - Try to use private fields with getters and setters
    [Key(0)] public int TimeKnowledge;
    [Key(1)] public List<string> UnlockedCharactersGuids = new();
    [Key(2)] public List<string> UnlockedAbilitiesGuids = new();
    [Key(3)] public List<string> UpgradesGuids = new();
    [Key(4)] public List<string> UnlockedMapsGuids = new();
    [Key(5)] public StartingAttributesSnapshot StartingAttributesInitializer = new();

    [Key(6)] public GameRecords Records = new();
    [Key(7)] public GameStats gameStats = new();

    [IgnoreMember] public List<Character> UnlockedCharacters = new();
    [IgnoreMember] public List<Ability> UnlockedAbilities = new();
    [IgnoreMember] public List<UpgradeScriptable> Upgrades = new();
    [IgnoreMember] public List<BetterMapData> UnlockedMaps = new();

    // some parameters that need to be saved here

    public MetaprogressionData(int timeKnowledge, List<Character> unlockedCharacters = null, List<Ability> unlockedAbilities = null,
                            List<BetterMapData> unlockedMaps = null, List<UpgradeScriptable> upgrades = null, StartingAttributesSnapshot startingAttributes = null,
                            GameRecords records = null, GameStats gameStats = null)
    {
        TimeKnowledge = timeKnowledge;
        UnlockedCharactersGuids = unlockedCharacters != null ? unlockedCharacters.Select(x => x.Guid).ToList() : new();
        UnlockedAbilitiesGuids = unlockedAbilities != null ? unlockedAbilities.Select(x => x.Guid).ToList() : new();
        UpgradesGuids = upgrades != null ? upgrades.Select(x => x.Guid).ToList() : new();
        UnlockedMapsGuids = unlockedMaps != null ? unlockedMaps.Select(x => x.Guid).ToList() : new();
        StartingAttributesInitializer = startingAttributes ?? new();
        Records = records ?? new();
        this.gameStats = gameStats ?? new();
        UnlockedCharacters = unlockedCharacters ?? new();
        UnlockedAbilities = unlockedAbilities ?? new();
        Upgrades = upgrades ?? new();
        UnlockedMaps = unlockedMaps ?? new();
    }

    [SerializationConstructor]
    public MetaprogressionData(int timeKnowledge, List<string> unlockedCharactersGuids,
                            List<string> unlockedAbilitiesGuids, List<string> upgradesGuids, List<string> unlockedMapsGuids,
                            StartingAttributesSnapshot startingAttributes, GameRecords records, GameStats gameStats)
    {
        TimeKnowledge = timeKnowledge;
        UnlockedCharactersGuids = unlockedCharactersGuids;
        UnlockedAbilitiesGuids = unlockedAbilitiesGuids;
        UpgradesGuids = upgradesGuids;
        UnlockedMapsGuids = unlockedMapsGuids;
        StartingAttributesInitializer = startingAttributes;
        Records = records;
        this.gameStats = gameStats;
        Load();
    }

    public void Load()
    {
        UnlockedCharacters = UnlockedCharactersGuids.Select(x => Resources.Load<Character>(x)).ToList();
        UnlockedAbilities = UnlockedAbilitiesGuids.Select(x => Resources.Load<Ability>(x)).ToList();
        Upgrades = UpgradesGuids.Select(x => Resources.Load<UpgradeScriptable>(x)).ToList();
        UnlockedMaps = UnlockedMapsGuids.Select(x => Resources.Load<BetterMapData>(x)).ToList();
    }
}
