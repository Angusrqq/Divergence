using UnityEngine;

public class CurrencyDisplay : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text _currencyText;

    void Start()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        _currencyText.text = GameData.CurrentMetadata?.TimeKnowledge.ToString();
    }
}
