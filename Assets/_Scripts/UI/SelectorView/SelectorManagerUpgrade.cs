using UnityEngine;
using UnityEngine.UI;

public class SelectorManagerUpgrades : SelectorManagerUnlockables
{
    public RectTransform LevelsContainer;
    public Toggle LevelTogglePrefab;

    public void RebuildLevelsContainer()
    {
        foreach (Transform child in LevelsContainer)
        {
            Destroy(child.gameObject);
        }

        UpgradeScriptable upgradeData = (UpgradeScriptable)CurrentSelectedData;
        for (int i = 1; i <= upgradeData.MaxLevel; i++)
        {
            Toggle levelToggle = Instantiate(LevelTogglePrefab, LevelsContainer);
            levelToggle.isOn = i <= upgradeData.Level;
        }
    }

    public override void Unlock(string type)
    {
        if (((UpgradeScriptable)CurrentSelectedData).IsUnlocked)
        {
            SelectorUpgrade selectorUpgrade = (SelectorUpgrade)CurrentSelectedItem;
            selectorUpgrade.OnUpgrade();
        }
        else
        {
            base.Unlock(type);
            UnlockButton.GetComponentInChildren<TMPro.TMP_Text>().text = "Upgrade";
        }
    }
}
