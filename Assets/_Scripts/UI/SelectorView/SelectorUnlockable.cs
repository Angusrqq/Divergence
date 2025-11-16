
using UnityEngine.EventSystems;

//holy shit this is insane
//TODO: Refactor
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

    public void OnUnlock()
    {
        IsUnlocked = true;
        ((SelectorManagerUnlockables)SelectorManager).UpdateDescription(Data.Description);
        SetUI();
    }
}
