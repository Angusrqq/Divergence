
using UnityEngine.EventSystems;

public class SelectorUnlockable : SelectorItemWithInfo
{
    public bool IsUnlocked;
    public override void Init(BaseScriptableObjectInfo data, SelectorManager manager)
    {
        data = (BaseScriptableObjectUnlockable)data;
        manager = (SelectorManagerUnlockables)manager;
        base.Init(data, manager);
    }
    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        SelectorManagerUnlockables selectorManager = (SelectorManagerUnlockables)SelectorManager;
        selectorManager.UnlockButton.interactable = !IsUnlocked;
        selectorManager.CurrencyDisplay.UpdateText();
        if (IsUnlocked) return;
        selectorManager.UpdateDescription("???");
        selectorManager.UpdateCost(((BaseScriptableObjectUnlockable)Data).Cost);
    }

    /// <summary>
    /// <para>
    /// <c>OnDeselect</c> method hides the border image when this button is deselected.
    /// </para>
    /// </summary>
    public override void OnDeselect()
    {
        base.OnDeselect();
    }

    public void OnUnlock()
    {
        IsUnlocked = true;
        ((SelectorManagerUnlockables)SelectorManager).UpdateDescription(Data.Description);
        SetUI();
    }

    /// <summary>
    /// <para>
    /// <c>SetUI</c> method sets the button image and name text based on whether the character is unlocked or not.
    /// </para>
    /// </summary>
    public override void SetUI()
    {
        //BaseScriptableObjectUnlockable tempdata = (BaseScriptableObjectUnlockable)Data;
        ButtonImage.sprite = Data.Icon;
        nameText.text = Data.Name;
    }
}
