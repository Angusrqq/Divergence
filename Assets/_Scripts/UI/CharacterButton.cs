using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// <para>
/// <c>CharacterButton</c> class represents a button in the character selection menu.
/// </para>
/// </summary>
public class CharacterButton : SelectorItem
{
    [NonSerialized] public Character characterData;
    [NonSerialized] public TMPro.TMP_Text nameText;
    public Image ButtonImage;

    /// <summary>
    /// <para>
    /// <c>Init</c> method initializes the character button with the given character data and selector manager.
    /// </para>
    /// </summary>
    /// <param name="character"></param> // TODO: Egor add desc for param
    /// <param name="manager"></param> // TODO: Egor add desc for param
    public void Init(Character character, SelectorManager manager)
    {
        nameText = GetComponentInChildren<TMPro.TMP_Text>();
        characterData = character;
        base.Init(character, manager);
        selectorManager = manager;
        
        SetUI();
    }

    /// <summary>
    /// <para>
    /// <c>OnSelect</c> method updates the description in the selector manager when this button is selected.
    /// </para>
    /// </summary>
    /// <param name="eventData"></param> // TODO: Egor add desc for param
    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        selectorManager.UpdateDescription(characterData.description);
    }

    /// <summary>
    /// <para>
    /// <c>SetUI</c> method sets the button image and name text based on whether the character is unlocked or not.
    /// </para>
    /// </summary>
    public void SetUI()
    {
        if (GameData.unlockedCharacters.Contains(characterData))
        {
            ButtonImage.sprite = characterData.icon;
        }
        else
        {
            ButtonImage.sprite = GameData.LockedIcon;
            Debug.LogWarning("FIGURE OUT A WAY TO STORE/LOAD CONST ICONS");
        }
        
        nameText.text = characterData.name;
    }
}
