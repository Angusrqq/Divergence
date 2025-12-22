using UnityEngine.EventSystems;

public class SelectorUpgrade : SelectorUnlockable
{
    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);

        SelectorManagerUpgrades selectorManager = (SelectorManagerUpgrades)SelectorManager;
        selectorManager.RebuildLevelsContainer();

        if (IsUnlocked)
        {
            selectorManager.UnlockButton.GetComponentInChildren<TMPro.TMP_Text>().text = "Upgrade";
        }
        else
        {
            selectorManager.UnlockButton.GetComponentInChildren<TMPro.TMP_Text>().text = "Unlock";
        }

        UpgradeScriptable upgradeData = (UpgradeScriptable)Data;
        selectorManager.UnlockButton.interactable = upgradeData.Level < upgradeData.MaxLevel;
    }

    public void OnUpgrade()
    {
        UpgradeScriptable upgradeData = (UpgradeScriptable)Data;
        upgradeData.UpgradeLogic.OnUpgrade();

        SelectorManagerUpgrades selectorManager = (SelectorManagerUpgrades)SelectorManager;
        selectorManager.RebuildLevelsContainer();
        
        if (upgradeData.Level >= upgradeData.MaxLevel)
        {
            selectorManager.UnlockButton.interactable = false;
        }
    }
}
