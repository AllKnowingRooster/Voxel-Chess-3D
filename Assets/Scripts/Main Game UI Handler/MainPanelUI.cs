using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainPanelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI startRoundText;
    [SerializeField] private TimerUI timerUI;
    [SerializeField] private GameObject resultContainer;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private Button exitButton;

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

    private string GenerateResultText(int winningTeam,WinReason reason)
    {
        if (reason==WinReason.Stalemate)
        {
            return "Stalemate";
        }else if (reason == WinReason.Checkmate)
        {
            return string.Format("{0} Win By Checkmate", winningTeam == 0 ? "White" : "Black");
        }
        else
        {
            return string.Format("{0} Win , {1} Timeout", winningTeam == 0 ? "White": "Black", winningTeam == 0 ? "Black": "White");
        }
    }
    

    public void ShowEndResult(int winningTeam,WinReason reason)
    {
        resultText.text = GenerateResultText(winningTeam,reason);
        exitButton.onClick.RemoveAllListeners();
        exitButton.onClick.AddListener(() => { Exit(); });
        gameObject.SetActive(true);
    }

    private void Exit()
    {
        gameObject.SetActive(false);
        SceneManager.sceneLoaded-=GameManager.instance.SceneLoadLogic;
        SceneManager.LoadScene(0);
    }

}
