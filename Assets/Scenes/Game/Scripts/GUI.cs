using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GUI : MonoBehaviour
{

    public Image PauseScreenPanel;
    [NonSerialized] public static bool paused = false;
    [NonSerialized] public static bool canPause = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && canPause)
        {
            if (paused)
            {
                Continue();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Continue()
    {
        togglePause();
        PauseScreenPanel.gameObject.SetActive(false);
    }

    public void Pause()
    {
        togglePause();
        PauseScreenPanel.gameObject.SetActive(true);
    }

    public void togglePause()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
            paused = false;
        }
        else
        {
            Time.timeScale = 0f;
            paused = true;
        }
    }

    public void BackToMenu()
    {
        togglePause();
        SceneManager.LoadScene("MainMenu");
    }

    public void Reset()
    {
        togglePause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
