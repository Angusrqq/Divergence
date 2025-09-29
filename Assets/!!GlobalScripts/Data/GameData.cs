using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
    /// <summary>
    /// <para>
    /// OK, the big <c>GameData</c> class is supposed to be a DDOL(Dont Destroy On Load) singleton, meaning there will be only one instance of <c>GameData</c> in the entire game and it will persist when switching scenes.
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
    //TODO: some way of map data storage(todo in Map.cs)
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
    [SerializeField] private static List<Map> _Maps;
    public static List<Map> Maps;
    public static List<Map> unlockedMaps;
    public static Map currentMap;
    [SerializeField] private List<Ability> _Abilities;
    public static List<Ability> Abilities;
    public static List<Ability> unlockedAbilities;

    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        Abilities = _Abilities;
        unlockedAbilities = Abilities;
    }

    public static void ResetRandomToSeed()
    {
        Random.InitState(currentSeed);
        lastState = Random.state;
    }

    public static void SetSeed(int seed, bool resetRandom = false)
    {
        currentSeed = seed;
        if (resetRandom) ResetRandomToSeed();
        Debug.Log($"Set seed to {currentSeed}");
    }

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

    public static void UpdatePlayerRef(Player _player)
    {
        player = _player;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
