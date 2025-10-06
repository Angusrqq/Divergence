using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// This script is responsible for the main menu of the game.
/// </para>
/// </summary>
public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject divergenceMeter;
    private divergenceMeter_anim dmAnimScript;
    private DivergenceMeter_Idle dmIdleScript;
    private Canvas mainMenuCanvas;
    [SerializeField] private SelectorManager characterSelectorManager;
    [SerializeField] private CharacterButton characterButtonPrefab;
    void Awake()
    {
        dmAnimScript = divergenceMeter.GetComponent<divergenceMeter_anim>();
        dmIdleScript = divergenceMeter.GetComponent<DivergenceMeter_Idle>();
        mainMenuCanvas = GetComponent<Canvas>();
    }

    void Start()
    {
        BuildCharacterSelector();
    }

    void FixedUpdate()
    {
        if (dmAnimScript.isEnded)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
        }
    }

/// <summary>
/// This function is called when the play button is clicked.
/// </summary>
    public void PlayGame()
    {
        mainMenuCanvas.enabled = false;
        dmIdleScript.OnDisable();
        dmIdleScript.enabled = false;
        dmAnimScript.enabled = true;
    }

/// <summary>
/// This function is called when the confirm button on character selector menu is clicked.
/// </summary>
    public void SetCharacter()
    {
        GameData.ChooseCharacter((Character)characterSelectorManager.currentSelectedItem);
    }
/// <summary>
/// This function (is not implemented yet because we don`t know yet how to store map data) is (SHOULD BE!!!) called when the confirm button on map selector menu is clicked.
/// </summary>
    public void SetMap()
    {
        throw new System.NotImplementedException();
        //GameData.ChooseMap(map);
    }

/// <summary>
/// <para>
/// This function is called in <c>Start()</c>
/// </para>
/// Creates a <c>CharacterButton</c> for each of all the characters that are unlocked.
/// </summary>
    public void BuildCharacterSelector()
    {
        List<SelectorItem> characterButtons = new List<SelectorItem>();
        foreach (Character character in GameData.unlockedCharacters)
        {
            CharacterButton charButton = Instantiate(characterButtonPrefab, characterSelectorManager.contentContainer);
            charButton.Init(character, characterSelectorManager);
            characterButtons.Add(charButton);
        }
        if (characterButtons.Count > 0)
        {
            characterButtons[0].OnSelect(null); // select first character by default
            characterButtons[0].GetComponent<UnityEngine.UI.Selectable>().Select();
        }
    }

/// <summary>
/// This function is called when the quit button is clicked.
/// </summary>
    public static void Quit()
    {
        // Save parameters here
        Application.Quit();
    }
}
