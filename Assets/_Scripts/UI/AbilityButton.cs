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
        BaseAbilityHandler handler = ability.Type == HandlerType.Passive ? GameData.player.AbilityHolder.GetPassiveByName(ability.Name) : GameData.player.AbilityHolder.GetAbilityByName(ability.Name);
        if (handler != null)
        {
            List<string> statStrings = new()
            {
                $"Level: <color=white>{handler.Level}</color> -> <color=yellow>{handler.Level + 1}</color>"
            };
            if(ability.Stats == null) return;
            var AllStats = ability.Stats;
            foreach (var s in AllStats)
            {
                string name = string.IsNullOrEmpty(s.Name) ? s.type.ToString() : s.Name;
                string currentValue = FormatAbilityValue(s.Scaling.Evaluate(handler.Level), s.type, s.Name);
                string nextValue = FormatAbilityValue(s.Scaling.Evaluate(handler.Level + 1), s.type, s.Name);
                statStrings.Add($"{name}: <color=white>{currentValue}</color> -> <color=yellow>{nextValue}</color>");
            }
            Description.text = string.Join("\n", statStrings);
        }
        else 
        {
            Description.text = ability.Description;
            NewAbilityText.gameObject.SetActive(true);
        }
        Icon.sprite = ability.Icon;
        Ability = ability;
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
