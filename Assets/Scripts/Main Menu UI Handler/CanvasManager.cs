using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI.Animate;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour,ISubject
{
    private IState currentState;
    [HideInInspector] public static CanvasManager instance;
    [HideInInspector] public CanvasAnimator canvasAnimator;
    private UIState startMenuState;
    private UIState connectMenuState;
    private string flyingTrigger;
    private string startMenuflyingStateName;
    private string fallingTrigger;
    private string startMenufallingStateName;

    private UIState mainMenuState;
    private UIState hostMenuState;
    private string slideInTrigger;
    private string mainMenuslideInStateName;
    private string slideOutTrigger;
    private string mainMenuslideOutStateName;

    private string hostMenuFlyingStateName;
    private string hostMenuFfallingStateName;

    private List<IObserver> listObserver;

    private string connectMenuSlideOutStateName;
    private string connectMenuSlideinStateName;

    [Header("Start Game Menu")]
    [SerializeField] private Animator startMenuAnimator;
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;

    [Header("Main Game Menu")]
    [SerializeField] private Animator mainMenuAnimator;
    [SerializeField] private Button offlineButton;
    [SerializeField] private Button connectButton;
    [SerializeField] private Button hostButton;
    [SerializeField] private Button mainMenuReturnButton;

    [Header("Connect Menu")]
    [SerializeField] private Animator connectMenuAnimator;
    [SerializeField] private Button connectReturnButton;
    [SerializeField] private Button connectConnectButton;
    [SerializeField] private Button connectClearButton;
    [SerializeField] private TMP_InputField connectInputField;

    [Header("Host Menu")]
    [SerializeField] private Animator hostMenuAnimator;
    [SerializeField] private Button hostMenuReturnButton;
    private void Awake()
    {
        if (instance!=null)
        {
            return;
        }
        instance= this;
        flyingTrigger = "Flying";
        fallingTrigger = "Falling";
        startMenuflyingStateName = "startMenuCanvasFlying";
        startMenufallingStateName = "startMenuCanvasFalling";
        slideInTrigger = "Slide In";
        slideOutTrigger = "Slide Out";
        mainMenuslideInStateName = "mainMenuCanvasSlideIn";
        mainMenuslideOutStateName = "mainMenuCanvasSlideOut";
        connectMenuSlideinStateName = "connectMenuCanvasSlideIn";
        connectMenuSlideOutStateName = "connectMenuCanvasSlideOut";
        hostMenuFfallingStateName = "hostMenuCanvasFalling";
        hostMenuFlyingStateName = "hostMenuCanvasFlying";
        canvasAnimator = new CanvasAnimator();
        startMenuState= new UIState(startMenuAnimator, startMenufallingStateName, fallingTrigger, startMenuflyingStateName, flyingTrigger);
        mainMenuState = new UIState(mainMenuAnimator, mainMenuslideInStateName, slideInTrigger, mainMenuslideOutStateName, slideOutTrigger);
        connectMenuState = new UIState(connectMenuAnimator, connectMenuSlideinStateName, slideInTrigger, connectMenuSlideOutStateName, slideOutTrigger);
        hostMenuState = new UIState(hostMenuAnimator, hostMenuFlyingStateName, flyingTrigger, hostMenuFfallingStateName, fallingTrigger);
        listObserver = new List<IObserver>();
        ConfigButton();
    }

    private void ConfigButton()
    {
        startButton.onClick.RemoveAllListeners();
        startButton.onClick.AddListener(() => { GoToMainMenu(); });
        quitButton.onClick.RemoveAllListeners();
        quitButton.onClick.AddListener(() => { Exit(); });
        offlineButton.onClick.RemoveAllListeners();
        offlineButton.onClick.AddListener(() => { SceneManager.LoadScene(1); });
        hostButton.onClick.RemoveAllListeners();
        hostButton.onClick.AddListener(() => { GoToHostMenu(); });
        connectButton.onClick.RemoveAllListeners();
        connectButton.onClick.AddListener(() => { GoToConnectMenu(); });
        mainMenuReturnButton.onClick.RemoveAllListeners();
        mainMenuReturnButton.onClick.AddListener(() => { GoToStartMenu(); });
        connectReturnButton.onClick.RemoveAllListeners();
        connectReturnButton.onClick.AddListener(() => { GoToMainMenu(); });
        hostMenuReturnButton.onClick.RemoveAllListeners();
        hostMenuReturnButton.onClick.AddListener(() => { HostMenuReturn(); });
        connectClearButton.onClick.RemoveAllListeners();
        connectClearButton.onClick.AddListener(() => { ClearInputField(); });
        connectConnectButton.onClick.RemoveAllListeners();
        connectConnectButton.onClick.AddListener(() => { ConnectHost(); });
    }

    public void AddObserver(IObserver observer)
    {
        listObserver.Add(observer);
    }

    public void NotifyObserver(UserAction action)
    {
        for (int i=0;i<listObserver.Count;i++)
        {
            listObserver[i].OnNotify(action);
        }
    }

    public void RemoveObserver(IObserver observer)
    {
        listObserver.Add(observer);
    }

    private IEnumerator ChangeState(IState state)
    {
        if (currentState!=null)
        {
          yield return currentState.ExitState();
        }
        currentState = state;
        yield return currentState.StartState();
    }

    void Start()
    {
        GoToStartMenu();
    }

    private void GoToMainMenu()
    {
        StartCoroutine(MainMenuRoutine());
    }

    private void GoToStartMenu()
    {
       StartCoroutine(StartMenuRoutine());
    }

    private void GoToHostMenu()
    {
        StartCoroutine(HostMenuRoutine());
    }

    private void GoToConnectMenu()
    {
        StartCoroutine(ConnectMenuRoutine());
    }

    private void HostMenuReturn()
    {
        StartCoroutine(HostMenuReturnRoutine());
    }

    private void ConnectHost()
    {
        GameManager.instance.CreateClientManager().Init(connectInputField.text, 9000);
    }

    private void ClearInputField()
    {
        connectInputField.text = null;
    }

    private void Exit()
    {
        StartCoroutine(ExitRoutine());
    }

    public IEnumerator ExitRoutine()
    {
        yield return StartCoroutine(canvasAnimator.WaitAnimationFinish(startMenuAnimator,flyingTrigger,startMenuflyingStateName));
            Application.Quit();
        
    }

    private IEnumerator HostMenuRoutine()
    {
        yield return StartCoroutine(ChangeState(hostMenuState));
        GameManager.instance.CreateServerManager().Init(9000);
        GameManager.instance.CreateClientManager().Init("127.0.0.1", 9000);
    }

    private IEnumerator ConnectMenuRoutine()
    {
        yield return StartCoroutine(ChangeState(connectMenuState));
    }

    private IEnumerator StartMenuRoutine()
    {
        yield return StartCoroutine(ChangeState(startMenuState));
    }

    private IEnumerator MainMenuRoutine()
    {
        yield return StartCoroutine(ChangeState(mainMenuState));
    }

    private IEnumerator HostMenuReturnRoutine()
    {
        Destroy(Server.instance.gameObject);
        Destroy(Client.instance.gameObject);
        yield return StartCoroutine(ChangeState(mainMenuState));
    }
}
