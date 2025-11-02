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
    [SerializeField] private SelectorItemWithInfo _characterButtonPrefab;

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
        BuildCharacterSelector();
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
        throw new System.NotImplementedException();
        //GameData.ChooseMap(map);
    }

    /// <summary>
    /// <para>
    /// <c>BuildCharacterSelector</c> method is called in <c>Start()</c>.
    /// </para>
    /// <para>
    /// Creates a <c>CharacterButton</c> for each unlocked character and selects the first character by default.
    /// </para>
    /// </summary>
    public void BuildCharacterSelector()
    {
        List<SelectorItem> characterButtons = new();
        foreach (Character character in GameData.unlockedCharacters)
        {
            SelectorItemWithInfo charButton = Instantiate(_characterButtonPrefab, _characterSelectorManager.contentContainer);
            charButton.Init(character, _characterSelectorManager);
            characterButtons.Add(charButton);
        }
        if (characterButtons.Count > 0)
        {
            characterButtons[0].OnSelect(null); // Select first character by default
            characterButtons[0].GetComponent<UnityEngine.UI.Selectable>().Select();
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
