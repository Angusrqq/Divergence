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
        _valueText.text = GameData.CurrentMetadata.Records.GetRecord(RecordType);
    }
}