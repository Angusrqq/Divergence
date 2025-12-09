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
    public Button ReviveButton;
    public AbilityButton AbilityButtonPrefab;
    [NonSerialized] public static bool Paused = false;
    [NonSerialized] public static bool CanPause = true;
    public List<BaseAbilityScriptable> AbilityChoices { get; private set; } = new();
    private List<AbilityButton> _abilityButtons = new();
    private List<BaseAbilityScriptable> _availableAbilities;

    /// <summary>
    /// Subscribes to the player's level-up event to trigger the level-up UI flow.
    /// </summary>
    void Start()
    {
        GameData.player.OnLevelUp += OnLevelUp;
        _availableAbilities = new(GameData.unlockedAbilities);
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

        if(GameData.InGameAttributes.Lives > 0f) ReviveButton.gameObject.SetActive(true);
        else ReviveButton.gameObject.SetActive(false);

        DeathScreenPanel.gameObject.SetActive(true);
        GameData.InGameAttributes.Lives--;
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

    public void EndRun()
    {
        GameData.CurrentMetadata.gameStats.RunsFinished += 1;
        GameData.CurrentMetadata.gameStats.TotalDamageDealt += (ulong)GameData.InGameAttributes.DamageDealt;
        GameData.CurrentMetadata.gameStats.TotalDamageTaken += (ulong)GameData.InGameAttributes.DamageTaken;
        GameData.CurrentMetadata.Records.MaxCritMult = (uint)Mathf.Max(GameData.CurrentMetadata.Records.MaxCritMult, GameData.InGameAttributes.CritMult * 100f);
        GameData.CurrentMetadata.Records.MaxDamageMult = (uint)Mathf.Max(GameData.CurrentMetadata.Records.MaxDamageMult, GameData.InGameAttributes.PlayerDamageMult * 100f);
        GameData.CurrentMetadata.Records.MaxCritChance = (uint)Mathf.Max(GameData.CurrentMetadata.Records.MaxCritChance, GameData.InGameAttributes.CritChance * 100f);
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
    public void OnLevelUp_Deprecated(UnityEngine.Object source, int level)
    {
        PauseInternal();
        AbilityChoices.Clear();
        List<BaseAbilityScriptable> toRemove = new();
        foreach(BaseAbilityScriptable ability in _availableAbilities)
        {
            BaseAbilityHandler handler = null;
            if(ability.Type == HandlerType.Passive) handler = GameData.player.AbilityHolder.GetPassiveByName(ability.Name);
            if(ability.Type == HandlerType.InstantiatedAbility) handler = GameData.player.AbilityHolder.GetAbilityByName(ability.Name);
            if(handler == null)
            {
                if(ability.Type == HandlerType.Passive && GameData.player.AbilityHolder.Passives.Count >= GameData.InGameAttributes.PassiveAbilitySlots)
                {
                    toRemove.Add(ability);
                }
                if(ability.Type == HandlerType.InstantiatedAbility && GameData.player.AbilityHolder.Abilities.Count >= GameData.InGameAttributes.ActiveAbilitySlots)
                {
                    toRemove.Add(ability);
                }
                continue;
            } 
            if(handler.Level >= handler.MaxLevel) toRemove.Add(ability);
        }

        foreach(BaseAbilityScriptable ability in toRemove) _availableAbilities.Remove(ability);

        List<BaseAbilityScriptable> availableAbilities = new(_availableAbilities);

        for (int i = 0; i < GameData.InGameAttributes.AbilitiesPerLevel; i++)
        {
            if (availableAbilities.Count == 0) break;
            BaseAbilityScriptable rolled = availableAbilities[GameData.ValuableRoll(0, availableAbilities.Count)];
            availableAbilities.Remove(rolled);
            AbilityChoices.Add(rolled);
        }
        RebuildAbilities();
        LevelUpPanel.gameObject.SetActive(true);
        if(AbilityChoices.Count == 0) CloseLevelUp();
    }

    public void OnLevelUp(UnityEngine.Object source, int level)
    {
        PauseInternal();
        AbilityChoices.Clear();
        RefreshAvailableAbilities();
        List<BaseAbilityScriptable> options = Utilities.GetRandomAbilities(_availableAbilities, GameData.InGameAttributes.Luck, (int)GameData.InGameAttributes.AbilitiesPerLevel);
        foreach(BaseAbilityScriptable ability in options) AbilityChoices.Add(ability);
        RebuildAbilities();
        LevelUpPanel.gameObject.SetActive(true);
        if(AbilityChoices.Count == 0) CloseLevelUp();
    }

    private void RefreshAvailableAbilities()
    {
        List<BaseAbilityScriptable> toRemove = new();
        foreach(BaseAbilityScriptable ability in _availableAbilities)
        {
            BaseAbilityHandler handler = null;
            if(ability.Type == HandlerType.Passive) handler = GameData.player.AbilityHolder.GetPassiveByName(ability.Name);
            if(ability.Type == HandlerType.InstantiatedAbility) handler = GameData.player.AbilityHolder.GetAbilityByName(ability.Name);
            if(handler == null)
            {
                if(ability.Type == HandlerType.Passive && GameData.player.AbilityHolder.Passives.Count >= GameData.InGameAttributes.PassiveAbilitySlots)
                {
                    toRemove.Add(ability);
                }
                if(ability.Type == HandlerType.InstantiatedAbility && GameData.player.AbilityHolder.Abilities.Count >= GameData.InGameAttributes.ActiveAbilitySlots)
                {
                    toRemove.Add(ability);
                }
                continue;
            } 
            //if(handler.Level >= handler.MaxLevel) toRemove.Add(ability);
        }

        foreach(BaseAbilityScriptable ability in toRemove) _availableAbilities.Remove(ability);
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
        
        foreach(BaseAbilityScriptable a in AbilityChoices)
        {
            AbilityButton button = Instantiate(AbilityButtonPrefab, LevelUpPanel.transform);
            bool acquired = a.Type == HandlerType.Passive ? GameData.player.AbilityHolder.GetPassiveByName(a.Name) != null : GameData.player.AbilityHolder.GetAbilityByName(a.Name) != null;
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
        GameData.player.DamageableEntity.Heal(this, GameData.player.DamageableEntity.MaxHealth, GetType());
    }
}
