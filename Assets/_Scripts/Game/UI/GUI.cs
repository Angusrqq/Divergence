using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// <para>
/// Handles in-game GUI.
/// </para>
/// Contains functions for buttons and handling pausing.
/// </summary>
public class GUI : MonoBehaviour
{
    public Image PauseScreenPanel;
    public Image LevelUpPanel;
    public Image DeathScreenPanel;
    public AbilityButton AbilityButtonPrefab;
    [NonSerialized] public static bool Paused = false;
    [NonSerialized] public static bool CanPause = true;
    public List<BaseAbility> AbilityChoices { get; private set; } = new();
    private List<AbilityButton> _abilityButtons = new();

    /// <summary>
    /// Subscribes to the player's level-up event to trigger the level-up UI flow.
    /// </summary>
    void Start()
    {
        GameData.player.OnLevelUp += OnLevelUp;
    }

    /// <summary>
    /// Handles pausing when Escape is pressed.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && CanPause)
        {
            if (Paused)
            {
                Continue();
            }
            else
            {
                Pause();
            }
        }
    }

    /// <summary>
    /// <para>
    /// <c>Continue</c> is called when the Continue button is pressed.
    /// </para> Continues the game from pause.
    /// </summary>
    public void Continue()
    {
        TogglePause(false);
        PauseScreenPanel.gameObject.SetActive(false);
    }

    /// <summary>
    /// Toggles pause and activates pause menu.
    /// </summary>
    public void Pause()
    {
        TogglePause(true);
        PauseScreenPanel.gameObject.SetActive(true);
    }

    /// <summary>
    /// <para>
    /// <c>Death</c> is called when the player dies.
    /// </para> Shows the death screen.
    /// </summary>
    public void Death()
    {
        PauseInternal();
        DeathScreenPanel.gameObject.SetActive(true);
    }

    /// <summary>
    /// <para>
    /// <c>UnpauseInternal</c> is used to unpause the game from non-GUI scripts.
    /// </para> Unpauses the game.
    /// </summary>
    public void UnpauseInternal()
    {
        TogglePause(false);
        CanPause = true;
    }

    /// <summary>
    /// <para>
    /// <c>PauseInternal</c> is used to pause the game from non-GUI scripts.
    /// </para> Pauses the game.
    /// </summary>
    public void PauseInternal()
    {
        TogglePause(true);
        CanPause = false;
    }

    /// <summary>
    /// <para>
    /// <c>TogglePause</c> toggles the pause state.
    /// </para> Pauses or unpauses the game.
    /// </summary>
    /// <param name="isPausing">True to pause (set <see cref="Time.timeScale"/> to 0), false to unpause.</param>
    public void TogglePause(bool isPausing)
    {
        if (isPausing != Paused)
        {
            if (Time.timeScale == 0f)
            {
                Time.timeScale = 1f;
                Paused = false;
            }
            else
            {
                Time.timeScale = 0f;
                Paused = true;
            }
        }
    }

    /// <summary>
    /// <para>
    /// <c>BackToMenu</c> is called when the Back to Menu button is pressed.
    /// </para> Loads the Main Menu scene.
    /// </summary>
    public void BackToMenu()
    {
        TogglePause(false);
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// <para>
    /// <c>Reset</c> is called when the Reset button is pressed.
    /// </para> Reloads the current scene.
    /// </summary>
    public void Reset()
    {
        TogglePause(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// <para>
    /// <c>LevelUp</c> is called when the player levels up.
    /// </para> Shows the level up screen.
    /// </summary>
    public void OnLevelUp(UnityEngine.Object source, int level)
    {
        PauseInternal();
        AbilityChoices.Clear();
        List<BaseAbility> unlockedAbilities = new(GameData.unlockedAbilities);
        for (int i = 0; i < Attributes.AbilitiesPerLevel; i++)
        {
            if (unlockedAbilities.Count == 0) break;
            BaseAbility rolled = unlockedAbilities[GameData.ValuableRoll(0, unlockedAbilities.Count)];
            unlockedAbilities.Remove(rolled);
            AbilityChoices.Add(rolled);
        }
        RebuildAbilities();
        LevelUpPanel.gameObject.SetActive(true);
    }

    /// <summary>
    /// Closes the level-up UI and returns the game to an unpaused state.
    /// </summary>
    /// <remarks>
    /// Typically invoked by a UI button after the player confirms an ability choice.
    /// Calls <see cref="UnpauseInternal"/> and hides <see cref="LevelUpPanel"/>.
    /// </remarks>
    public void CloseLevelUp()
    {
        UnpauseInternal();
        LevelUpPanel.gameObject.SetActive(false);
    }

    /// <summary>
    /// Rebuilds the level-up ability selection UI from <see cref="AbilityChoices"/>.
    /// Destroys any existing buttons and instantiates new <see cref="AbilityButton"/> entries.
    /// </summary>
    private void RebuildAbilities()
    {
        foreach (AbilityButton button in _abilityButtons)
        {
            Destroy(button.gameObject);
        }
        _abilityButtons.Clear();
        
        foreach(BaseAbility a in AbilityChoices)
        {
            AbilityButton button = Instantiate(AbilityButtonPrefab, LevelUpPanel.transform);
            button.Init(a);
            _abilityButtons.Add(button);
        }
    }

    /// <summary>
    /// <para>
    /// <c>Resurrect</c> is called when the player chooses to resurrect from the death screen.
    /// </para>
    /// Not fully implemented - some ability specific code should be added here.
    /// </summary>
    public void Revive()
    {
        TogglePause(false);
        CanPause = true;
        DeathScreenPanel.gameObject.SetActive(false);
        //Some ability specific code here 
    }
}
