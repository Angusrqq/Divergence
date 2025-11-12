using UnityEngine;

public class CurrencyDisplay : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text _currencyText;

    void Start()
    {
        _currencyText.text = GameData.CurrentMetadata?.TimeKnowledge.ToString();
    }

    public void UpdateText()
    {
        _currencyText.text = GameData.CurrentMetadata?.TimeKnowledge.ToString();
    }
}
