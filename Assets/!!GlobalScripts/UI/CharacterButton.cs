using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterButton : SelectorItem
{
    [NonSerialized] public Character characterData;
    [NonSerialized] public TMPro.TMP_Text nameText;
    public Image ButtonImage;

    public void Init(Character character, SelectorManager manager)
    {
        nameText = GetComponentInChildren<TMPro.TMP_Text>();
        characterData = character;
        base.Init(character, manager);
        selectorManager = manager;
        SetUI();
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        selectorManager.UpdateDescription(characterData.description);
    }
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
