using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainPanelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI startRoundText;
    [SerializeField] private TimerUI timerUI;
    public IEnumerator RoundCountdown(float duration)
    {
        startRoundText.gameObject.SetActive(true);
        while (duration <= 0.0f)
        {
            duration -= Time.deltaTime;
            startRoundText.text = string.Format("Game Start in {0}",Mathf.Ceil(duration).ToString());
            yield return null;
        }
        startRoundText.gameObject.SetActive(false);
        timerUI.gameObject.SetActive(true);
    }

}
