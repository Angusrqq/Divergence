using UnityEngine;
using UnityEngine.SceneManagement;

public class GameData : MonoBehaviour
{
    public static GameData instance;
    public static int currentSeed;
    public static Random.State lastState;
    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
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
            Random.InitState((int)System.DateTime.Now.Ticks); //reset random to current time
            Debug.Log("Reset random to current time");
        }
    }
}
