using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    public TMP_Text AbilityName;
    public TMP_Text Description;
    public Image Icon;
    public BaseAbilityScriptable Ability;

    public void Init(BaseAbilityScriptable ability)
    {
        AbilityName.text = ability.Name;
        Description.text = ability.Description;
        Icon.sprite = ability.Icon;
        Ability = ability;
    }

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
