using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public static void PlayGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }
    public static void Quit()
    {
        // Save parameters here
        Application.Quit();
    }
}
