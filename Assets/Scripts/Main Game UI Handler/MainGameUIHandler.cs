using UnityEngine;

public class MainGameUiManager : MonoBehaviour
{
    
    public PromoteUI promoteUI;
    public MainPanelUI mainPanelUI;
    public ResultUI resultUI;
    public static MainGameUiManager instance;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
    }
}
