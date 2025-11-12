using System.Collections.Generic;
using UnityEngine.UI;

public class SelectorManagerUnlockables : SelectorManager
{
    public CurrencyDisplay CurrencyDisplay;
    public Button UnlockButton;

    public void UpdateCost(int cost)
    {
        CurrencyDisplay.UpdateText();
        CurrencyDisplay.GetComponentInChildren<TMPro.TMP_Text>().text += $"-{cost}?";
    }

    public void Unlock()
    {
        
        if (GameData.CurrentMetadata.TimeKnowledge < ((BaseScriptableObjectUnlockable)CurrentSelectedData).Cost)
        {
            //some kind of feedback for not enough currency
            return;
        }
        GameData.CurrentMetadata.TimeKnowledge -= ((BaseScriptableObjectUnlockable)CurrentSelectedData).Cost;
        ((BaseScriptableObjectUnlockable)CurrentSelectedData).IsUnlocked = true;
        GameData.unlockedAbilities.Add((BaseAbilityScriptable)CurrentSelectedData);
        ((SelectorUnlockable)CurrentSelectedItem).OnUnlock();
        CurrencyDisplay.UpdateText();
        //something else unlocking related?
    }
}