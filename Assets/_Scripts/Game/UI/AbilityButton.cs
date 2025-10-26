using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    public TMP_Text AbilityName;
    public TMP_Text Description;
    public Image Icon;
    public Ability Ability;

    public void Init(Ability ability)
    {
        AbilityName.text = ability.Name;
        Description.text = ability.Description;
        Icon.sprite = ability.Icon;
        Ability = ability;
    }

    public void AbilityPicked()
    {
        GameData.player.AbilityHolder.AddAbility(Ability);
        transform.parent.GetComponentInParent<GUI>().CloseLevelUp(); // TODO: Find a better way, looks bad
    }
}
