using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GUI : MonoBehaviour
{
    public Image PauseScreenPanel;
    public Image LevelUpPanel;
    public Image DeathScreenPanel;
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
        TogglePause(false);
        PauseScreenPanel.gameObject.SetActive(false);
    }

    public void Pause()
    {
        TogglePause(true);
        PauseScreenPanel.gameObject.SetActive(true);
    }

    public void Death()
    {
        PauseInternal();
        DeathScreenPanel.gameObject.SetActive(true);
    }

    public void UnpauseInternal()
    {
        TogglePause(false);
        canPause = true;
    }

    public void PauseInternal()
    {
        TogglePause(true);
        canPause = false;
    }

    public void TogglePause(bool isPausing)
    {
        if (isPausing != paused)
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

    }

    public void BackToMenu()
    {
        TogglePause(false);
        SceneManager.LoadScene("MainMenu");
    }

    public void Reset()
    {
        TogglePause(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnLevelUp()
    {
        TogglePause(true);
        canPause = false;
        LevelUpPanel.gameObject.SetActive(true);
    }

    public void Resurrect()
    {
        TogglePause(false);
        canPause = true;
        DeathScreenPanel.gameObject.SetActive(false);
        //Some ability specific code here 
    }
}
