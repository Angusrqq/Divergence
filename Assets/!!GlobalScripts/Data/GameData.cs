using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameData : MonoBehaviour
{
    public static GameData instance;
    public static Player player;
    public static int currentSeed;
    public static Random.State lastState;
    [SerializeField] private List<Ability> _allAbilities;
    public static List<Ability> allAbilities;
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
        allAbilities = _allAbilities;
        unlockedAbilities = allAbilities;
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
