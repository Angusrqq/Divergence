using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// <c>MainMenu</c> class manages the main menu scene, including character selection, game start, and menu navigation.
/// </para>
/// </summary>
[RequireComponent(typeof(Canvas))]
public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _divergenceMeterObject;
    [SerializeField] private SelectorManager _characterSelectorManager;
    [SerializeField] private SelectorManager _mapSelectorManager;
    [SerializeField] private SelectorItemWithInfo _selectorItemPrefab;

    private DivergenceMeter _divergenceMeter;
    private Canvas _mainMenuCanvas;
    private Coroutine _idleAnim;

    /// <summary>
    /// <para>
    /// <c>Awake</c> method initializes components when the object is created.
    /// </para>
    /// </summary>
    void Awake()
    {
        _divergenceMeter = _divergenceMeterObject.GetComponent<DivergenceMeter>();
        _mainMenuCanvas = GetComponent<Canvas>();
    }

    /// <summary>
    /// <para>
    /// <c>Start</c> method builds the character selector and starts the divergence meter idle animation.
    /// </para>
    /// </summary>
    void Start()
    {
        BuildSelector(GameData.unlockedCharacters, _characterSelectorManager);
        BuildSelector(GameData.unlockedMaps, _mapSelectorManager);
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
        _mainMenuCanvas.enabled = false;

        if (_idleAnim != null)
        {
            StopCoroutine(_idleAnim);
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
            infoButtons[0].GetComponent<UnityEngine.UI.Selectable>().Select();
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
