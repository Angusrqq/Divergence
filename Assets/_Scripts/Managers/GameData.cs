using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

/// <summary>
/// <para>
/// <c>GameData</c> class is supposed to be a DDOL(Dont Destroy On Load) singleton, meaning there will be only one instance of <c>GameData</c> in the entire game and it will persist when switching scenes.
/// </para>
/// <para>
/// It stores all the data that needs to persist between scenes, like player data, unlocked characters, maps, abilities, etc.
/// It also handles the random seed for procedural generation, and keeps track of the current state of the game.
/// </para>
/// </summary>
public class GameData : MonoBehaviour
{
    // TODO: We need to do scary refactoring here because this is gonna break everything ☢️
    public static GameData instance;
    public static Player player;
    public static int currentSeed;
    public static Random.State lastValuableState;
    public static Random.State lastInvaluableState;
    public static List<Character> Characters = new();
    public static List<Character> unlockedCharacters = new();
    public static Character currentCharacter;
    public static List<BetterMapData> Maps = new();
    public static List<BetterMapData> unlockedMaps = new();
    public static BetterMapData currentMap;
    public static List<BaseAbilityScriptable> Abilities = new();
    public static List<BaseAbilityScriptable> unlockedAbilities = new();
    public static List<UpgradeScriptable> Upgrades = new();
    public static List<UpgradeScriptable> unlockedUpgrades = new();
    public static List<EnemyData> Enemies = new();
    public static InGameAtributes InGameAttributes;
    public static MetaprogressionData CurrentMetadata;

    public static Sprite LockedIcon { get; private set; } // not going to cut it, // TODO: figure out a way to store/load constant icons
    public static Tilemap TilemapToLoadMaps { get; set; }
    public static SettingsData CurrentSettings { get; private set; }

    /// <summary>
    /// <para>
    /// <c>Awake</c> method ensures that there is only one instance of <c>GameData</c> and sets it to not be destroyed on load.
    /// </para>
    /// It also initializes the static lists and variables. It also subscribes to the <c>SceneManager.sceneLoaded</c> event to reset the random seed when the game scene is loaded.
    /// </summary>
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        LockedIcon = Resources.Load<Sprite>("Icons/locked_icon");
        if (LockedIcon == null)
        {
            Debug.LogError("Locked icon not found in Resources/Icons/locked_icon");
        }
        SceneManager.sceneLoaded += OnSceneLoaded;

        //loading all data from resources, idk if this is the best way tho
        foreach (BaseAbilityScriptable ability in Resources.LoadAll<BaseAbilityScriptable>("ObjectsData/Abilities"))
        {
            Abilities.Add(ability);
            if (ability.IsUnlocked)
            {
                unlockedAbilities.Add(ability);
            }
        }
        foreach (BetterMapData map in Resources.LoadAll<BetterMapData>("ObjectsData/Maps"))
        {
            Maps.Add(map);
            if(map.IsUnlocked)
            {
                unlockedMaps.Add(map);
            }
        }
        foreach (Character character in Resources.LoadAll<Character>("ObjectsData/Characters"))
        {
            Characters.Add(character);
            if(character.IsUnlocked)
            {
                unlockedCharacters.Add(character);
            }
        }
        foreach (UpgradeScriptable upgrade in Resources.LoadAll<UpgradeScriptable>("ObjectsData/Upgrades"))
        {
            Upgrades.Add(upgrade);
            if (upgrade.IsUnlocked)
            {
                unlockedUpgrades.Add(upgrade);
            }
        }
        foreach (EnemyData enemy in Resources.LoadAll<EnemyData>("ObjectsData/Enemies"))
        {
            Enemies.Add(enemy);
        }
    }

    void Start()
    {
        CurrentSettings = InitializeSettings();
        CurrentMetadata = InitializeProgData();
    }

    /// <summary>
    /// <para>
    /// <c>ResetRandomToSeed</c> method resets the Unity random number generator to the current seed and stores the last state of the random number generator.
    /// </para>
    /// </summary>
    public static void ResetRandomToSeed()
    {
        Random.InitState(currentSeed);
        lastValuableState = Random.state;
        lastInvaluableState = Random.state;
    }

    //  USE THESE FUNCTIONS WHEN YOU WANT TO KEEP TRACK OF THE IMPORTANT EVENTS (like rolling abilities after levelling up).
    /// <summary>
    /// Regular int <c>Random.Range</c> function, but saves random states
    /// </summary>
    public static int ValuableRoll(int minInclusive, int maxExclusive)
    {
        lastInvaluableState = Random.state;
        Random.state = lastValuableState;
        int res = Random.Range(minInclusive, maxExclusive);
        lastValuableState = Random.state;
        Random.state = lastInvaluableState;
        return res;
    }

    /// <summary>
    /// Regular float <c>Random.Range</c> function, but saves random states
    /// </summary>
    public static float ValuableRoll(float minInclusive, float maxInclusive)
    {
        lastInvaluableState = Random.state;
        Random.state = lastValuableState;
        float res = Random.Range(minInclusive, maxInclusive);
        lastValuableState = Random.state;
        Random.state = lastInvaluableState;
        return res;
    }

    /// <summary>
    /// <para>
    /// <c>SetSeed</c> method sets the current seed to the passed value and optionally resets the random number generator to that seed.
    /// </para>
    /// </summary>
    public static void SetSeed(int seed, bool resetRandom = false)
    {
        currentSeed = seed;

        if (resetRandom)
        {
            ResetRandomToSeed();
        }

        Debug.Log($"Set seed to {currentSeed}");
    }

    /// <summary>
    /// <para>
    /// <c>OnSceneLoaded</c> method is called when a new scene is loaded
    /// </para>
    /// </summary>
    public static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene Loaded: {scene.name}");

        if (scene.name == "Game")
        {
            ResetRandomToSeed();

            currentMap = currentMap != null ? currentMap : Maps[0];
            GameObject temp = Instantiate(currentMap.mapPrefab);
            Camera.main.transform.parent.GetComponentInChildren<CinemachineConfiner2D>().BoundingShape2D = temp.GetComponent<Collider2D>();
            InGameAttributes = new(); // reconstruct to use the new values from starting attributes
            Debug.Log("Reset random to seed " + currentSeed);
        }

        if (scene.name == "MainMenu")
        {
            Random.InitState((int)System.DateTime.Now.Ticks); // Reset random to current time
            Debug.Log("Reset random to current time");
        }
    }

    /// <summary>
    /// <para>
    /// <c>ChooseCharacter</c> method sets the current character to the passed character.
    /// </para>
    /// Used in the character selection menu.
    /// </summary>
    /// <param name="character"></param>
    public static void ChooseCharacter(Character character)
    {
        currentCharacter = character;
        Debug.Log($"Chosen character: {character.name}");
    }

    /// <summary>
    /// <para>
    /// <c>ChooseMap</c> method sets the current map to the passed map.
    /// </para>
    /// Used in the map selection menu.
    /// </summary>
    /// <param name="map"></param>
    public static void ChooseMap(BetterMapData map)
    {
        currentMap = map;
        Debug.Log($"Chosen map: {map.name}");
    }

    /// <summary>
    /// <para>
    /// <c>UpdatePlayerRef</c> method updates the static reference to the player.
    /// </para>
    /// Used in the player script.
    /// </summary>
    /// <param name="_player"></param>
    public static void UpdatePlayerRef(Player _player)
    {
        player = _player;
    }

    /// <summary>
    /// Called when the object is destroyed.
    /// Unsubscribes from the <c>SceneManager.sceneLoaded</c> event to prevent memory leaks.
    /// </summary>
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private static SettingsData InitializeSettings()
    {
        SettingsData data = new();
        data = DataSystem.LoadSettingsData() ?? data;
        SetGameSettings(data);

        return data;
    }

    private static MetaprogressionData InitializeProgData()
    {
        MetaprogressionData data = DataSystem.LoadProgData() ?? new MetaprogressionData(0);
        return data;
    }

    public static void SaveMetaData(MetaprogressionData data = null)
    {
        if (data != null)
        {
            DataSystem.SaveProgData(data);
        }
        else
        {
            DataSystem.SaveProgData(CurrentMetadata);
        }
    }

    private static void SetGameSettings(SettingsData data)
    {
        Screen.SetResolution(data.ScreenWidth, data.ScreenHeight, (FullScreenMode)System.Enum.Parse(typeof(FullScreenMode), data.FullScreen));
        Application.targetFrameRate = data.RefreshRate;

        AudioManager.instance.SetMasterVolume(data.MasterVolume);
        AudioManager.instance.SetMusicVolume(data.MusicVolume);
        AudioManager.instance.SetSFXVolume(data.SfxVolume);
    }

    public static void UpdateSettings(SettingsData data, bool refreshSettings = false, bool save = false)
    {
        if (refreshSettings)
        {
            SetGameSettings(data);
        }

        if (save)
        {
            DataSystem.SaveSettingsData(data);
        }

        CurrentSettings = data;
    }
}
