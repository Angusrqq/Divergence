using UnityEngine;

public class GameRecordRepresentative : MonoBehaviour
{
    public GameRecords.RecordType RecordType;
    private TMPro.TMP_Text _valueText;

    void Awake()
    {
        _valueText = transform.Find("Value").GetComponent<TMPro.TMP_Text>();
    }
    void Start()
    {
        _valueText.text = RecordType switch
        {
            GameRecords.RecordType.CritMult => GameData.CurrentMetadata.Records.MaxCritMult.ToString() + "%",
            GameRecords.RecordType.DamageMult => GameData.CurrentMetadata.Records.MaxDamageMult.ToString() + "%",
            GameRecords.RecordType.CritChance => GameData.CurrentMetadata.Records.MaxCritChance.ToString() + "%",
            _ => GameData.CurrentMetadata.Records.GetRecord(RecordType)
        };
    }
}