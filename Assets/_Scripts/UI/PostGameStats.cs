using UnityEngine;

public class PostGameStats : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text _levelText;
    [SerializeField] private TMPro.TMP_Text _timeText;

    private void OnEnable()
    {
        _timeText.text = "Time survived: " + GameTimer.FormatTime(GameData.GameTimerInstance.currentTime);
        _levelText.text = "Level: " + GameData.player.Level.ToString();
    }
}
