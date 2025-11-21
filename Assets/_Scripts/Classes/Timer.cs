using System.Timers;
using UnityEngine;

[RequireComponent(typeof(TMPro.TMP_Text))]
public class GameTimer : MonoBehaviour
{
    void Awake()
    {
        GameData.UpdateTimerRef(this);
        timerText = GetComponent<TMPro.TMP_Text>();
    }
    public float currentTime = 0f;
    public TMPro.TMP_Text timerText;
    private void Update()
    {
        currentTime += Time.deltaTime;
        timerText.text = FormatTime(currentTime);
    }

    private static string FormatTime(float time, bool showMilliseconds = false, bool showHours = false)
    {
        string res = "";
        int hours = (int)(time / 3600);
        int minutes = (int)(time % 3600 / 60);
        int seconds = (int)(time % 60);
        int milliseconds = (int)(time * 1000 % 1000);

        res += showHours ? hours.ToString("00") + ":" : "";
        res += minutes.ToString("00") + ":";
        res += seconds.ToString("00");
        if (showMilliseconds) res += "." + milliseconds.ToString("000");
        return res;
    }
}
