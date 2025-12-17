using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// <para>
/// <c>MainMenu</c> class manages the main menu scene, including character selection, game start, and menu navigation.
/// </para>
/// </summary>
[RequireComponent(typeof(Canvas))]
public class MainMenu : MonoBehaviour
{
    [SerializeField] private DivergenceMeter _divergenceMeter;
    [SerializeField] private SelectorManager _characterSelectorManager;
    [SerializeField] private SelectorManager _mapSelectorManager;
    [SerializeField] private SelectorManagerUnlockables _abilityUnlockablesSelectorManager;
    [SerializeField] private SelectorManagerUnlockables _characterUnlockableSelectorManager;
    [SerializeField] private SelectorManagerUnlockables _mapUnlockableSelectorManager;
    [SerializeField] private SelectorManagerUpgrades _upgradeSelectorManager;
    [SerializeField] private SelectorItemWithInfo _selectorItemPrefab;
    [SerializeField] private SelectorUnlockable _selectorUnlockablePrefab;
    [SerializeField] private SelectorUpgrade _selectorUpgradePrefab;
    [SerializeField] private GameObject _mainMenuButtons;
    private Coroutine _idleAnim;

    /// <summary>
    /// <para>
    /// <c>Start</c> method builds the character selector and starts the divergence meter idle animation.
    /// </para>
    /// </summary>
    void Start()
    {
        BuildSelector(GameData.unlockedCharacters, _characterSelectorManager);
        BuildSelector(GameData.Maps, _mapSelectorManager);
        BuildSelectorUnlockables(GameData.Abilities, GameData.unlockedAbilities, _abilityUnlockablesSelectorManager);
        BuildSelectorUnlockables(GameData.Characters, GameData.unlockedCharacters, _characterUnlockableSelectorManager);
        BuildSelectorUnlockables(GameData.Maps, GameData.unlockedMaps, _mapUnlockableSelectorManager);
        BuildSelectorUpgrades(GameData.Upgrades, GameData.unlockedUpgrades, _upgradeSelectorManager);
        _idleAnim = StartCoroutine(_divergenceMeter.IdleAnimation());
    }

    /// <summary>
    /// <para>
    /// <c>FixedUpdate</c> method checks if the divergence meter animation has ended and loads the game scene.
    /// </para>
    /// </summary>
    void FixedUpdate()
    {
        if (DivergenceMeter.AnimationEnded)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
        }
    }

    /// <summary>
    /// <para>
    /// <c>PlayGame</c> method is called when the play button is clicked.
    /// </para>
    /// <para>
    /// Disables the main menu canvas, stops the idle animation, and starts the divergence meter play animation.
    /// </para>
    /// </summary>
    public void PlayGame()
    {
        _mainMenuButtons.SetActive(false);

        if (_idleAnim != null)
        {
            StopCoroutine(_idleAnim);
            Debug.Log("Stopping idle animation");
            _idleAnim = null;
        }

        StartCoroutine(_divergenceMeter.PlayAnimation());
    }

    /// <summary>
    /// <para>
    /// <c>SetCharacter</c> method is called when the confirm button on character selector menu is clicked.
    /// </para>
    /// <para>
    /// Sets the selected character in <c>GameData</c>.
    /// </para>
    /// </summary>
    public void SetCharacter()
    {
        GameData.ChooseCharacter((Character)_characterSelectorManager.CurrentSelectedData);
    }

    /// <summary>
    /// <para>
    /// <c>SetMap</c> method is not implemented yet because map data storage is not yet determined.
    /// </para>
    /// <para>
    /// Should be called when the confirm button on map selector menu is clicked.
    /// </para>
    /// </summary>
    public void SetMap()
    {
        GameData.ChooseMap((BetterMapData)_mapSelectorManager.CurrentSelectedData);
    }

    /// <summary>
    /// <para>
    /// <c>BuildSelector</c> method is called in <c>Start()</c>.
    /// </para>
    /// <para>
    /// Creates a <c>SelectorItemWithInfo</c> for each object in passed collection of type T (<c>BaseScriptableObjectInfo</c>) and selects the first character by default.
    /// </para>
    /// </summary>
    public void BuildSelector<T>(ICollection<T> objectInfos, SelectorManager selectorManager) where T : BaseScriptableObjectInfo
    {
        List<SelectorItem> infoButtons = new();
        foreach (T objectInfo in objectInfos)
        {
            SelectorItemWithInfo infoButton = Instantiate(_selectorItemPrefab, selectorManager.contentContainer);
            infoButton.Init(objectInfo, selectorManager);
            infoButtons.Add(infoButton);
        }
        if (infoButtons.Count > 0)
        {
            infoButtons[0].OnSelect(null); // Select first item by default
            infoButtons[0].GetComponent<Selectable>().Select();
        }
    }

    //TODO: Refactor all the shit about unlockable selectors
    //TODO: Evgeniy jenek refactor delay davai
    public void BuildSelectorUnlockables<T>(ICollection<T> objectInfos, ICollection<T> unlockedObjectInfos, SelectorManagerUnlockables selectorManager) where T : BaseScriptableObjectUnlockable
    {
        List<SelectorUnlockable> infoButtons = new();
        foreach (T objectInfo in objectInfos)
        {
            SelectorUnlockable infoButton = Instantiate(_selectorUnlockablePrefab, selectorManager.contentContainer);
            infoButton.Init(objectInfo, selectorManager);
            infoButton.IsUnlocked = unlockedObjectInfos.Contains(objectInfo);
            infoButton.ButtonImage.sprite = unlockedObjectInfos.Contains(objectInfo) ? infoButton.ButtonImage.sprite : GameData.LockedIcon; // TODO: change to locked sprite
            infoButton.nameText.text = unlockedObjectInfos.Contains(objectInfo) ? infoButton.Data.Name : "???";
            infoButtons.Add(infoButton);
        }
        if (infoButtons.Count > 0)
        {
            infoButtons[0].OnSelect(null); // Select first item by default
            infoButtons[0].GetComponent<Selectable>().Select();
        }
    }

    public void BuildSelectorUpgrades<T>(ICollection<T> objectInfos, ICollection<T> unlockedObjectInfos, SelectorManagerUnlockables selectorManager) where T : UpgradeScriptable
    {
        List<SelectorUnlockable> infoButtons = new();
        foreach (T objectInfo in objectInfos)
        {
            SelectorUpgrade infoButton = Instantiate(_selectorUpgradePrefab, selectorManager.contentContainer);
            infoButton.Init(objectInfo, selectorManager);
            infoButton.IsUnlocked = unlockedObjectInfos.Contains(objectInfo);
            infoButton.ButtonImage.sprite = unlockedObjectInfos.Contains(objectInfo) ? infoButton.ButtonImage.sprite : GameData.LockedIcon; // TODO: change to locked sprite
            infoButton.nameText.text = unlockedObjectInfos.Contains(objectInfo) ? infoButton.Data.Name : "???";
            infoButtons.Add(infoButton);
        }
        if (infoButtons.Count > 0)
        {
            infoButtons[0].OnSelect(null); // Select first item by default
            infoButtons[0].GetComponent<Selectable>().Select();
        }
    }

    /// <summary>
    /// <para>
    /// <c>Quit</c> method is called when the quit button is clicked.
    /// </para>
    /// <para>
    /// Quits the application. Parameters should be saved before quitting.
    /// </para>
    /// </summary>
    public static void Quit()
    {
        // TODO: Save parameters here
        Application.Quit();
    }
}
