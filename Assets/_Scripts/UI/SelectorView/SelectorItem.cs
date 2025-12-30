using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Represents an item in a selection menu (Scroll View), such as character or map selection.
/// Handles selection state updates and visual feedback through a highlighted image.
/// </summary>
public class SelectorItem : MonoBehaviour, ISelectHandler
{
    [SerializeField] private ScriptableObject _itemData;
    [SerializeField] private SelectorManager _selectorManager;

    public Image SelectedImage;

    public ScriptableObject ItemData => _itemData;
    public SelectorManager SelectorManager
    {
        get => _selectorManager;
        set => _selectorManager = value;
    }
    protected SelectorManagerUnlockables SelectorUnlockablesManager => (SelectorManagerUnlockables)SelectorManager;
    protected SelectorManagerUpgrades SelectorUpgradesManager => (SelectorManagerUpgrades)SelectorManager;

    public virtual void Init(ScriptableObject data, SelectorManager manager)
    {
        _itemData = data;
        _selectorManager = manager;
    }

    public virtual void OnSelect(BaseEventData eventData)
    {
        _selectorManager.CurrentSelectedItem = this;

        SelectedImage.enabled = true;
        try
        {
            SelectedImage.material.SetColor("_Color", Utilities.GetTierColor(((BaseAbilityScriptable)_itemData).Tier) * 4f);
        }
        catch
        {
            SelectedImage.material.SetColor("_Color", new Color(3.56486797f, 1.9095813f, 0, 2) * 1f);
        }
    }

    public virtual void OnDeselect()
    {
        SelectedImage.enabled = false;
    }
}
