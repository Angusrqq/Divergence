using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GUI : MonoBehaviour
{
    public Image PauseScreenPanel;
    public Image LevelUpPanel;
    public Image DeathScreenPanel;
    public Button ReviveButton;
    public AbilityButton AbilityButtonPrefab;
    [NonSerialized] public static bool Paused = false;
    [NonSerialized] public static bool CanPause = true;

    private List<AbilityButton> _abilityButtons = new();
    private List<BaseAbilityScriptable> _availableAbilities;
    private InputAction _escapeAction;

    public List<BaseAbilityScriptable> AbilityChoices { get; private set; } = new();

    void Start()
    {
        GameData.player.OnLevelUp += OnLevelUp;
        _escapeAction = GameData.PlayerInputAsset.FindAction("Escape");
        InputActionMap _escapeActionMap = _escapeAction.actionMap;
        _escapeAction.performed += HandlePause;
        _escapeActionMap.Enable();
        _availableAbilities = new(GameData.unlockedAbilities);
    }

    private void HandlePause(InputAction.CallbackContext context)
    {
        if (!CanPause) return;
        if (!context.performed) return;

        if (Paused)
        {
            Continue();
        }
        else
        {
            Pause();
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

        if (GameData.InGameAttributes.Lives > 0f)
        {
            ReviveButton.gameObject.SetActive(true);
        }
        else
        {
            ReviveButton.gameObject.SetActive(false);
        }

        DeathScreenPanel.gameObject.SetActive(true);
        GameData.InGameAttributes.Lives--;
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

    public void OnLevelUp(UnityEngine.Object source, int level)
    {
        PauseInternal();
        AbilityChoices.Clear();
        RefreshAvailableAbilities();

        var rabbitsPaw = GameData.player.AbilityHolder.GetPassiveByName("Rabbit's paw");
        if (rabbitsPaw != null)
        {
            AbilityChoices.Add(rabbitsPaw.Source);
        }

        // Chance to roll a guaranteed upgrade
        if (GameData.ValuableValue <= 0.65f && AbilityChoices.Count == 0)
        {
            var allAbilities = GameData.player.AbilityHolder.GetAllAbilities();
            BaseAbilityScriptable rolledUpgrade = allAbilities[GameData.ValuableRoll(0, allAbilities.Count)].Source;

            AbilityChoices.Add(rolledUpgrade);
        }

        List<BaseAbilityScriptable> options = Utilities.GetRandomAbilities(
            unlockedAbilities: _availableAbilities.Except(AbilityChoices).ToList(),
            luck: GameData.InGameAttributes.Luck,
            amount: (int)GameData.InGameAttributes.AbilitiesPerLevel - AbilityChoices.Count
        );
        
        foreach (BaseAbilityScriptable ability in options)
        {
            AbilityChoices.Add(ability);
        }

        RebuildAbilities();
        LevelUpPanel.gameObject.SetActive(true);

        if (AbilityChoices.Count == 0)
        {
            CloseLevelUp();
        }
    }

    private void RefreshAvailableAbilities()
    {
        List<BaseAbilityScriptable> toRemove = new();

        foreach (BaseAbilityScriptable ability in _availableAbilities)
        {
            BaseAbilityHandler handler = GameData.player.AbilityHolder.GetHandlerForAbility(ability);

            if (handler == null)
            {
                if (ability.Type == HandlerType.Passive && GameData.player.AbilityHolder.Passives.Count >= GameData.InGameAttributes.PassiveAbilitySlots)
                {
                    toRemove.Add(ability);
                }
                if (ability.Type == HandlerType.InstantiatedAbility && GameData.player.AbilityHolder.Abilities.Count >= GameData.InGameAttributes.ActiveAbilitySlots)
                {
                    toRemove.Add(ability);
                }

                continue;
            } 
        }

        foreach (BaseAbilityScriptable ability in toRemove)
        {
            _availableAbilities.Remove(ability);
        }
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
        
        foreach(BaseAbilityScriptable ability in AbilityChoices)
        {
            AbilityButton button = Instantiate(AbilityButtonPrefab, LevelUpPanel.transform);
            bool acquired = GameData.player.AbilityHolder.GetHandlerForAbility(ability) != null;

            button.Init(ability);
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
        // Some ability specific code here 
        GameData.player.DamageableEntity.Heal(this, GameData.player.DamageableEntity.MaxHealth, GetType());
    }

    private void OnDestroy()
    {
        _escapeAction.performed -= HandlePause;
    }
}
