using System.Collections;
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    private float whiteTime;
    private float blackTime;
    [SerializeField] private TextMeshProUGUI blackTimeText;
    [SerializeField] private TextMeshProUGUI whiteTimeText;
    private float secondPerMinute;

    void Update()
    {
        if (!GameManager.instance.isGameover)
        {
            ReduceTime(ref (ChessBoard.whoTurn == 0 ? ref whiteTime : ref blackTime), secondPerMinute, ref (ChessBoard.whoTurn == 0 ? ref whiteTimeText : ref blackTimeText));
            isTimeOut(ChessBoard.whoTurn, ref (ChessBoard.whoTurn == 0 ? ref whiteTime : ref blackTime));
        }
    }

    void ReduceTime(ref float time, float secondPerMinute, ref TextMeshProUGUI timeText)
    {
        time -= Time.deltaTime;
        SetText(ref time, secondPerMinute, ref timeText);
    }

    void SetText(ref float time, float secondPerMinute, ref TextMeshProUGUI timeText)
    {
        string beforeSeparator = Mathf.Floor(time / secondPerMinute).ToString();
        string afterSeparator = Mathf.Floor(time % secondPerMinute).ToString();
        timeText.text = string.Format("{0}:{1}", AppendZero(beforeSeparator), AppendZero(afterSeparator));
    }

    string AppendZero(string text)
    {
        if (text.Length > 1)
        {
            return text;
        }

        return "0" + text;
    }

    void isTimeOut(int whoTurn, ref float time)
    {
        if (time <= 0.0f)
        {
            GameManager.instance.SetWinner(ChessBoard.whoTurn==0?1:0, WinReason.Timeout);
        }
    }

    void OnEnable()
    {
        whiteTime = GameManager.instance.startingTime;
        blackTime = GameManager.instance.startingTime;
        secondPerMinute = 60.0f;
        SetText(ref whiteTime, secondPerMinute, ref whiteTimeText);
        SetText(ref blackTime, secondPerMinute, ref blackTimeText);
    }



}
