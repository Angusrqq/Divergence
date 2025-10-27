using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    public TMP_Text AbilityName;
    public TMP_Text Description;
    public Image Icon;
    public BaseAbility Ability;

    public void Init(BaseAbility ability)
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
            GameData.player.AbilityHolder.AddPassive(Ability);
        }
        else
        {
            GameData.player.AbilityHolder.AddAbility(Ability);
        }
            transform.parent.GetComponentInParent<GUI>().CloseLevelUp(); // TODO: Find a better way, looks bad
    }
}
