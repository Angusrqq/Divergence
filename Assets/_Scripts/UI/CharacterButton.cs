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
    /// <param name="character"><c>Character</c> to be associated with this button</param>
    /// <param name="manager"><c>SelectorManager</c> parent that will contain this button</param>
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
    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        selectorManager.UpdateDescription(characterData.Description);
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
            ButtonImage.sprite = characterData.Icon;
        }
        else
        {
            ButtonImage.sprite = GameData.LockedIcon;
            Debug.LogWarning("FIGURE OUT A WAY TO STORE/LOAD CONST ICONS");
        }
        
        nameText.text = characterData.name;
    }
}
