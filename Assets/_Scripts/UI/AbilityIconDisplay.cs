using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityIconDisplay : MonoBehaviour
{
    [SerializeField] private RectTransform _activeAbilitiesIconsRoot;
    [SerializeField] private RectTransform _passiveAbilitiesIconsRoot;
    [SerializeField] private Image _IconHolderPrefab;

    private List<Image> _activeAbilitiesIcons = new();
    private List<Image> _passiveAbilitiesIcons = new();

    private void Start()
    {
        for(int i = 0; i < GameData.InGameAttributes.ActiveAbilitySlots; i++)
        {
            _activeAbilitiesIcons.Add(Instantiate(_IconHolderPrefab, _activeAbilitiesIconsRoot));
        }
        for(int i = 0; i < GameData.InGameAttributes.PassiveAbilitySlots; i++)
        {
            _passiveAbilitiesIcons.Add(Instantiate(_IconHolderPrefab, _passiveAbilitiesIconsRoot));
        }
    }

    public void UpdateActiveAbilitiesIcons(List<BaseAbilityHandler> activeAbilitiesList)
    {
        UpdateIconsInternal(_activeAbilitiesIcons, activeAbilitiesList);
    }

    public void UpdatePassiveAbilitiesIcons(List<BaseAbilityHandler> passiveAbilitiesList)
    {
        UpdateIconsInternal(_passiveAbilitiesIcons, passiveAbilitiesList);
    }

    private void UpdateIconsInternal(List<Image> icons, List<BaseAbilityHandler> abilities)
    {
        byte abilityIndex = 0;
        foreach (Image image in icons)
        {
            if (abilityIndex >= abilities.Count)
                break;

            Sprite abilityIcon = abilities[abilityIndex].Icon;
            image.sprite = abilityIcon;

            TMP_Text abilityLevel = image.GetComponentInChildren<TMP_Text>();
            abilityLevel.text = "lv. " + abilities[abilityIndex].Level.ToString();

            image.color = Color.white;

            abilityIndex++;
        }
    }
}
