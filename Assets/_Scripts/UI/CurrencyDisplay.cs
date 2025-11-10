using UnityEngine;

public class CurrencyDisplay : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text _currencyText;

    void Start()
    {
        _currencyText.text = GameData.CurrentMetadata?.TimeKnowledge.ToString();
    }
}
