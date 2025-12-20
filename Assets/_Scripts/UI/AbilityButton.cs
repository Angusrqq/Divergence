using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using static Utilities;

public class AbilityButton : MonoBehaviour
{
    public TMP_Text AbilityName;
    public TMP_Text Description;
    public Image Icon;
    public BaseAbilityScriptable Ability;
    public Image TierBorder;
    public TMP_Text NewAbilityText;

    public void Init(BaseAbilityScriptable ability)
    {
        AbilityName.text = ability.Name;
        Icon.sprite = ability.Icon;
        Ability = ability;

        BaseAbilityHandler handler = GameData.player.AbilityHolder.GetHandlerForAbility(ability);
        if (handler != null)
        {
            List<string> statStrings = new() { $"Level: <color=white>{handler.Level}</color> -> <color=yellow>{handler.Level + 1}</color>" };
            
            if (ability.Stats == null) return;

            var AllStats = ability.Stats;
            foreach (var stat in AllStats)
            {
                string name = string.IsNullOrEmpty(stat.Name) ? stat.type.ToString() : stat.Name;
                string currentValue = FormatAbilityValue(stat.Scaling.Evaluate(handler.Level), stat.type, stat.Name);
                string nextValue = FormatAbilityValue(stat.Scaling.Evaluate(handler.Level + 1), stat.type, stat.Name);

                statStrings.Add($"{name}: <color=white>{currentValue}</color> -> <color=yellow>{nextValue}</color>");
            }

            Description.text = string.Join("\n", statStrings);
        }
        else
        {
            Description.text = ability.Description;
            NewAbilityText.gameObject.SetActive(true);
        }
    }

    private void Start() => TierBorder.color = GetTierColor(Ability.Tier);

    public void AbilityPicked()
    {
        if (Ability.GetType() == typeof(PassiveAbility))
        {
            GameData.player.AbilityHolder.AddPassive((PassiveAbility)Ability);
        }
        else if (Ability.GetType() == typeof(InstantiatedAbilityScriptable))
        {
            GameData.player.AbilityHolder.AddAbility((InstantiatedAbilityScriptable)Ability);
        }
        
        transform.parent.GetComponentInParent<GUI>().CloseLevelUp(); // TODO: Find a better way, looks bad
    }
}
