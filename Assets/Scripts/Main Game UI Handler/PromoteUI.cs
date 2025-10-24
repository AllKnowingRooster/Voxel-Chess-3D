using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class PromoteUI : MonoBehaviour
{
    [SerializeField] private List<Button> listButton;
    [SerializeField] private Button checkBoardButton;
    [SerializeField] private List<GameObject> listUI;
    private TaskCompletionSource<int> tcs;
    private Image panelBackground;

    private void Awake()
    {
        panelBackground = GetComponent<Image>();
        checkBoardButton.onClick.RemoveAllListeners();
        checkBoardButton.onClick.AddListener(() => { CheckBoard(); });
    }
    public Task<int> ShowPromoteModalDialog()
    {
        tcs = new TaskCompletionSource<int>();
        for (int i=0;i<listButton.Count;i++)
        {
            int index = i;
            listButton[index].onClick.RemoveAllListeners();
            listButton[index].onClick.AddListener(() => {HidePromoteModalDialog(index); });

        }
        gameObject.SetActive(true);
        return tcs.Task;
    }

    private void HidePromoteModalDialog(int index)
    {
        tcs.SetResult(index);
        gameObject.SetActive(false);
    }

    private void CheckBoard()
    {
        for (int i=0;i<listUI.Count;i++)
        {
            listUI[i].SetActive(!listUI[i].activeSelf);
        }
        changeAlpha(listUI[0].activeSelf);
    }

    private void changeAlpha(bool isActive)
    {
        Color panelBackgroundColor = panelBackground.color;
        panelBackgroundColor.a=isActive ? 0.5f : 0.0f;
        panelBackground.color=panelBackgroundColor;
    }
}
