using UnityEngine;

public class GameStatRepresentative : MonoBehaviour
{
    public GameStats.StatType statType;
    
    private TMPro.TMP_Text valueText;

    void Awake()
    {
        valueText = transform.Find("Value").GetComponent<TMPro.TMP_Text>();
    }

    void Start()
    {
        valueText.text = statType switch
        {
            GameStats.StatType.Time => GameTimer.FormatTimeConditionally(GameData.CurrentMetadata.gameStats.TotalTime),
            _ => GameData.CurrentMetadata.gameStats.GetStat(statType),
        };
    }
}
