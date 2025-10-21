using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// TODO: Evgeniy - Refactor this
/// <summary>
/// <para>
/// <c>GameData</c> class is supposed to be a DDOL(Dont Destroy On Load) singleton, meaning there will be only one instance of <c>GameData</c> in the entire game and it will persist when switching scenes.
/// </para>
/// <para>
/// It stores all the data that needs to persist between scenes, like player data, unlocked characters, maps, abilities, etc.
/// It also handles the random seed for procedural generation, and keeps track of the current state of the game.
/// </para>
/// It should have methods to save and load the data from a file, but thats too much for now since im the only one working.
/// </summary>
public class GameData : MonoBehaviour
{
    //TODO: damn this is gonna be a lot
    // some way of map data storage(todo in Map.cs)
    //TODO: save/load system
    //TODO: setting current character/map/abilities in menu
    //TODO: character selection in menu and display character info
    //TODO: map selection in menu and display map info
    //TODO: ability tree in main menu <<< hard one, so optional for now
    //TODO: AUDIO << hard one, not optional
    public static GameData instance;
    public static Player player;
    public static int currentSeed;
    public static Random.State lastState;
    [SerializeField] private List<Character> _Characters;
    public static List<Character> Characters;
    public static List<Character> unlockedCharacters;
    public static Character currentCharacter;
    [SerializeField] private List<Map> _Maps;
    public static List<Map> Maps;
    public static List<Map> unlockedMaps;
    public static Map currentMap;
    [SerializeField] private List<Ability> _Abilities;
    public static List<Ability> Abilities;
    public static List<Ability> unlockedAbilities;
    [SerializeField] private List<EnemyData> _Enemies;
    public static List<EnemyData> Enemies;
    public static Sprite LockedIcon { get; private set; } // not going to cut it, //TODO: figure out a way to store/load constant icons

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
        LockedIcon = Resources.Load<Sprite>("Assets/Icons/locked_icon.png");
        SceneManager.sceneLoaded += OnSceneLoaded;
        Abilities = _Abilities;
        unlockedAbilities = Abilities;
        Maps = _Maps;
        unlockedMaps = Maps;
        Characters = _Characters;
        unlockedCharacters = Characters;
        Enemies = _Enemies;
    }

    /// <summary>
    /// <para>
    /// <c>ResetRandomToSeed</c> method resets the Unity random number generator to the current seed and stores the last state of the random number generator.
    /// </para>
    /// </summary>
    public static void ResetRandomToSeed()
    {
        Random.InitState(currentSeed);
        lastState = Random.state;
    }

    /// <summary>
    /// <para>
    /// <c>SetSeed</c> method sets the current seed to the passed value and optionally resets the random number generator to that seed.
    /// </para>
    /// </summary>
    /// <param name="seed"></param>
    /// <param name="resetRandom"></param>
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
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    public static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene Loaded: {scene.name}");

        if (scene.name == "Game")
        {
            ResetRandomToSeed();
            Debug.Log("Reset random to seed " + currentSeed);
        }

        if (scene.name == "MainMenu")
        {
            Random.InitState((int)System.DateTime.Now.Ticks); // reset random to current time
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
    public static void ChooseMap(Map map)
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
}
