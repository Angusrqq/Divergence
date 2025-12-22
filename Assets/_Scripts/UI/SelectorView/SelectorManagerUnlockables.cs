using UnityEngine.UI;

public class SelectorManagerUnlockables : SelectorManager
{
    public CurrencyDisplay CurrencyDisplay;
    public Button UnlockButton;

    public virtual void UpdateCost(int cost)
    {
        CurrencyDisplay.UpdateText();
        CurrencyDisplay.GetComponentInChildren<TMPro.TMP_Text>().text += $"-{cost}?";
    }

    public virtual void Unlock(string type)
    {
        if (GameData.CurrentMetadata.TimeKnowledge < ((BaseScriptableObjectUnlockable)CurrentSelectedData).Cost)
        {
            // Some kind of feedback for not enough currency
            return;
        }

        GameData.CurrentMetadata.TimeKnowledge -= ((BaseScriptableObjectUnlockable)CurrentSelectedData).Cost;
        ((BaseScriptableObjectUnlockable)CurrentSelectedData).IsUnlocked = true;

        switch (type)
        {
            case "ability":
                GameData.unlockedAbilities.Add((BaseAbilityScriptable)CurrentSelectedData);
                break;
            case "upgrade":
                GameData.unlockedUpgrades.Add((UpgradeScriptable)CurrentSelectedData);
                break;
            case "character":
                GameData.unlockedCharacters.Add((Character)CurrentSelectedData);
                break;
            case "map":
                GameData.unlockedMaps.Add((BetterMapData)CurrentSelectedData);
                break;
        }

        ((SelectorUnlockable)CurrentSelectedItem).OnUnlock();
        CurrencyDisplay.UpdateText();
        //something else unlocking related?
    }
}
