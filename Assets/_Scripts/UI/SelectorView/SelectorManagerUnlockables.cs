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
        if (GameData.CurrentMetadata.TimeKnowledge < CurrentSelectedUnlockable.Cost)
        {
            // Some kind of feedback for not enough currency
            return;
        }

        GameData.CurrentMetadata.TimeKnowledge -= CurrentSelectedUnlockable.Cost;
        CurrentSelectedUnlockable.IsUnlocked = true;

        switch (type)
        {
            case "ability":
                GameData.unlockedAbilities.Add(CurrentSelectedAbility);
                break;
            case "upgrade":
                GameData.unlockedUpgrades.Add(CurrentSelectedUpgrade);
                break;
            case "character":
                GameData.unlockedCharacters.Add(CurrentSelectedCharacter);
                break;
            case "map":
                GameData.unlockedMaps.Add(CurrentSelectedMap);
                break;
        }

        ((SelectorUnlockable)CurrentSelectedItem).OnUnlock();
        CurrencyDisplay.UpdateText();
        // Something else unlocking related?
    }
}
