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

        var selectorManager = SelectorUnlockablesManager;
        selectorManager.UnlockButton.interactable = !IsUnlocked;
        selectorManager.CurrencyDisplay.UpdateText();

        if (IsUnlocked) return;

        selectorManager.SetDescription("???");
        selectorManager.UpdateCost(UnlockableData.Cost);
    }

    public void OnUnlock()
    {
        IsUnlocked = true;
        SelectorUnlockablesManager.SetDescription(Data.Description);

        SetUI();
    }
}
