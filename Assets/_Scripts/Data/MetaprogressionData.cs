using System.Collections.Generic;

/// <summary>
/// TODO: Egor - Yo. not implemented at all.
/// Should store data about metaprogression, like time knowledge (some currency like gold), unlocked characters, etc.
/// Should be serializable and saved/loaded using DataSystem.
/// </summary>
[System.Serializable]
public class MetaprogressionData
{
    public int TimeKnowledge;
    public List<Character> UnlockedCharacters = new();
    public List<Ability> UnlockedAbilities = new();
    //TODO: Implement Upgrade class
    // public List<Upgrade> Upgrades = new();
    public List<MapData> UnlockedMaps = new();

    // some parameters that need to be saved here

    public MetaprogressionData(int timeKnowledge, List<Character> unlockedCharacters = null, List<Ability> unlockedAbilities = null, List<MapData> unlockedMaps = null)
    {
        TimeKnowledge = timeKnowledge;
        UnlockedCharacters = unlockedCharacters ?? new List<Character>();
        UnlockedAbilities = unlockedAbilities ?? new List<Ability>();
        UnlockedMaps = unlockedMaps ?? new List<MapData>();
    }
}

