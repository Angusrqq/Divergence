using System.Collections.Generic;
using System.Linq;
using MessagePack;

[MessagePackObject]
public class MetaprogressionData
{
    [Key(0)] public int TimeKnowledge;
    [Key(1)] public List<string> UnlockedCharactersGuids = new();
    [Key(2)] public List<string> UnlockedAbilitiesGuids = new();
    [Key(3)] public List<string> UpgradesGuids = new();
    [Key(4)] public List<string> UnlockedMapsGuids = new();
    [Key(5)] public StartingAttributesSnapshot StartingAttributesInitializer = new();
    [Key(6)] public GameRecords Records = new();
    [Key(7)] public GameStats gameStats = new();
    [IgnoreMember] public List<Character> UnlockedCharacters = new();
    [IgnoreMember] public List<BaseAbilityScriptable> UnlockedAbilities = new();
    [IgnoreMember] public List<UpgradeScriptable> Upgrades = new();
    [IgnoreMember] public List<BetterMapData> UnlockedMaps = new();

    public MetaprogressionData(
        int timeKnowledge,
        List<Character> unlockedCharacters = null,
        List<BaseAbilityScriptable> unlockedAbilities = null,
        List<BetterMapData> unlockedMaps = null,
        List<UpgradeScriptable> upgrades = null,
        StartingAttributesSnapshot startingAttributes = null,
        GameRecords records = null,
        GameStats gameStats = null
    )
    {
        TimeKnowledge = timeKnowledge;

        UnlockedCharactersGuids =
            unlockedCharacters != null
                ? unlockedCharacters.Select(x => x.Guid).ToList()
                : new();

        UnlockedAbilitiesGuids =
            unlockedAbilities != null
                ? unlockedAbilities.Select(x => x.Guid).ToList()
                : new();

        UnlockedMapsGuids =
            unlockedMaps != null
                ? unlockedMaps.Select(x => x.Guid).ToList()
                : new();

        UpgradesGuids =
            upgrades != null
                ? upgrades.Select(x => x.Guid).ToList()
                : new();

        StartingAttributesInitializer = startingAttributes ?? new();
        Records = records ?? new();
        this.gameStats = gameStats ?? new();

        UnlockedCharacters = unlockedCharacters ?? new();
        UnlockedAbilities = unlockedAbilities ?? new();
        UnlockedMaps = unlockedMaps ?? new();
        Upgrades = upgrades ?? new();
    }

    [SerializationConstructor]
    public MetaprogressionData(
        int timeKnowledge,
        List<string> unlockedCharactersGuids,
        List<string> unlockedAbilitiesGuids,
        List<string> upgradesGuids,
        List<string> unlockedMapsGuids,
        StartingAttributesSnapshot startingAttributes,
        GameRecords records,
        GameStats gameStats
    )
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
        UnlockedCharacters = UnlockedCharactersGuids.Select(characterGuid =>
            GameData.Characters.Find(character => character.Guid == characterGuid)).ToList();

        UnlockedAbilities = UnlockedAbilitiesGuids.Select(abilityGuid =>
            GameData.Abilities.Find(ability => ability.Guid == abilityGuid)).ToList();

        Upgrades = UpgradesGuids.Select(upgradeGuid =>
            GameData.Upgrades.Find(upgrade => upgrade.Guid == upgradeGuid)).ToList();

        UnlockedMaps = UnlockedMapsGuids.Select(mapGuid =>
            GameData.Maps.Find(map => map.Guid == mapGuid)).ToList();
    }

    public void UpdateGuids()
    {
        UnlockedCharactersGuids = GameData.unlockedCharacters.Select(x => x.Guid).ToList();
        UnlockedAbilitiesGuids = GameData.unlockedAbilities.Select(x => x.Guid).ToList();
        UpgradesGuids = GameData.unlockedUpgrades.Select(x => x.Guid).ToList();
        UnlockedMapsGuids = GameData.unlockedMaps.Select(x => x.Guid).ToList();
    }
}
