using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TextMeshProUGUI rematchText;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button rematchButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        exitButton.onClick.RemoveAllListeners();
        exitButton.onClick.AddListener(() => { Exit(); });
        rematchButton.onClick.RemoveAllListeners();
        rematchButton.onClick.AddListener(() => { SendRematch(); });
    }
    private void Exit()
    {
        gameObject.SetActive(false);
        NetRematch nr = new NetRematch();
        nr.teamId = GameManager.instance.assignedTeam;
        nr.wantRematch = 0;
        Client.instance.SendToServer(nr);
        Invoke("ChangeScene", 2.0f);
    }

    private void ChangeScene()
    {
        Destroy(Client.instance.gameObject);
        if (Server.instance != null)
        {
            Destroy(Server.instance.gameObject);
        }
        SceneManager.LoadScene(0);
    }

    private void SendRematch()
    {
        NetRematch nr = new NetRematch();
        nr.teamId = GameManager.instance.assignedTeam;
        nr.wantRematch = 1;
        Client.instance.SendToServer(nr);
        rematchButton.interactable = false;
    }

    private string GenerateResultText(int winningTeam, WinReason reason)
    {
        if (reason == WinReason.Stalemate)
        {
            return "Stalemate";
        }
        else if (reason == WinReason.Checkmate)
        {
            return string.Format("{0} Win By Checkmate", winningTeam == 0 ? "White" : "Black");
        }
        else
        {
            return string.Format("{0} Win , {1} Timeout", winningTeam == 0 ? "White" : "Black", winningTeam == 0 ? "Black" : "White");
        }
    }
    public void ShowEndResult(int winningTeam, WinReason reason)
    {
        resultText.text = GenerateResultText(winningTeam, reason);
        gameObject.SetActive(true);
    }

    public void SetRematchText(string text,Color color)
    {
        rematchText.text = text;
        rematchText.color = color;
    }

}
