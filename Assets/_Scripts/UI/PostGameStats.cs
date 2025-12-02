using System;
using UnityEngine;

public class PostGameStats : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text _levelText;
    [SerializeField] private TMPro.TMP_Text _timeText;

    //DONT DELETE THIS SCRIPT!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

    // private async void OnEnable()
    // {
    //     Awaitable timeAnim = StatIncrementalAnim(0f, GameData.GameTimerInstance.currentTime, _timeText, setFunction: GameTimer.FormatTimeConditionally, startText: "Time survived: ");
    //     await timeAnim;
    //     Awaitable levelAnim = StatIncrementalAnim(0f, GameData.player.Level, _levelText, startText: "Level: ", setFunction: t => ((int)t).ToString());
    //     await levelAnim;
    // }

    // private static async Awaitable StatIncrementalAnim(float from, float to, TMPro.TMP_Text text, float speed = 1f, Func<float, string> setFunction = null, string startText = null)
    // {
    //     float elapsed = 0f;
    //     while(elapsed < 1f)
    //     {
    //         if (Input.GetKeyDown(KeyCode.Escape))
    //         {
    //             return;
    //         }
    //         elapsed += Time.unscaledDeltaTime * speed;
    //         float t = Mathf.Lerp(from, to, elapsed);
    //         text.text = startText != null ? startText + (setFunction == null ? t : setFunction?.Invoke(t)) : (setFunction == null ? t.ToString() : setFunction?.Invoke(t));
    //         await Awaitable.NextFrameAsync();
    //     }
    // }
}
