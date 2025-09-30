using System.Collections.Generic;
using UnityEngine;

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

    public void PlayGame()
    {
        mainMenuCanvas.enabled = false;
        dmIdleScript.OnDisable();
        dmIdleScript.enabled = false;
        dmAnimScript.enabled = true;
    }

    public void SetCharacter()
    {
        GameData.ChooseCharacter((Character)characterSelectorManager.currentSelectedItem);
    }

    public void SetMap()
    {
        throw new System.NotImplementedException();
        //GameData.ChooseMap(map);
    }

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
    public static void Quit()
    {
        // Save parameters here
        Application.Quit();
    }
}
