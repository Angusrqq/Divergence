using System.Collections.Generic;

/// <summary>
/// Should store data about metaprogression, like time knowledge (some currency like gold), unlocked characters, etc.
/// Should be serializable and saved/loaded using DataSystem.
/// </summary>
[System.Serializable]
public class MetaprogressionData
{
    // TODO: Egor - Try to use private fields with getters and setters
    public int TimeKnowledge;
    public List<Character> UnlockedCharacters = new();
    public List<Ability> UnlockedAbilities = new();
    public List<UpgradeScriptable> Upgrades = new();
    public List<MapData> UnlockedMaps = new();
    public StartingAttributesSnapshot StartingAttributesInitializer = new();

    public GameRecords Records = new();
    public GameStats gameStats = new();

    // some parameters that need to be saved here

    public MetaprogressionData(int timeKnowledge, List<Character> unlockedCharacters = null, List<Ability> unlockedAbilities = null,
                            List<MapData> unlockedMaps = null, List<UpgradeScriptable> upgrades = null, StartingAttributesSnapshot startingAttributes = null,
                            GameRecords records = null, GameStats gameStats = null)
    {
        TimeKnowledge = timeKnowledge;
        UnlockedCharacters = unlockedCharacters ?? new();
        UnlockedAbilities = unlockedAbilities ?? new();
        Upgrades = upgrades ?? new();
        UnlockedMaps = unlockedMaps ?? new();
        StartingAttributesInitializer = startingAttributes ?? new();
        Records = records ?? new();
        this.gameStats = gameStats ?? new();
    }
}
