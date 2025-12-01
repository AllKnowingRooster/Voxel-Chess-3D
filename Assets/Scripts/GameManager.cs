using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum UserAction
{
    Hover,
    Click,
    Exit
}
public enum WinReason
{
    Stalemate,
    Checkmate,
    Timeout
}

public class GameManager : MonoBehaviour, ISubject
{
    public List<IObserver> listObserver;
    public static GameManager instance;

    [HideInInspector] public int assignedTeam;
    [Header("Chess Board Config")]
    private GameObject chessBoardGameObject;
    [SerializeField] public GameObject blueTilePrefab;
    [SerializeField] public GameObject blackTilePrefab;
    [SerializeField] public GameObject borderTilePrefab;
    private float startRoundTimer;


    [Header("Chess Piece Config")]
    public Material[] teamMaterial;
    public List<GameObject> listChessPiecePrefab;

    [HideInInspector] public bool isGameover;
    [HideInInspector] public float startingTime;
    [HideInInspector] public int winner;
    [HideInInspector] public WinReason winReason;
    public IGameMode gameMode;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(instance);
        isGameover = false;
        startRoundTimer = 3.0f;
        startingTime = 900.0f;
        listObserver = new List<IObserver>();
        SceneManager.sceneLoaded += SceneLoadLogic;
    }

    public void AddObserver(IObserver observer)
    {
        listObserver.Add(observer);
    }

    public void NotifyObserver(UserAction action)
    {
        Debug.Log(GetInstanceID());
        for (int i = 0; i < listObserver.Count; i++)
        {
            listObserver[i].OnNotify(action);
        }
    }

    public void RemoveObserver(IObserver observer)
    {
        listObserver.Remove(observer);
    }

    public void SceneLoadLogic(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1)
        {
            isGameover = false;
            startingTime = 900.0f;
            StartCoroutine(GameLoop());
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= SceneLoadLogic;
    }

    public void ResetVariable()
    {
        isGameover = false;
        startRoundTimer = 3.0f;
        startingTime = 900.0f;
    }

    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(StartGame());
        yield return StartCoroutine(PlayingGame());
        yield return StartCoroutine(EndGame());
    }


    private IEnumerator StartGame()
    {
        chessBoardGameObject = new GameObject("Chess Board");
        chessBoardGameObject.AddComponent<ChessBoard>();
        yield return StartCoroutine(MainGameUiManager.instance.mainPanelUI.RoundCountdown(startRoundTimer));
    }

    private IEnumerator PlayingGame()
    {
        while (!isGameover)
        {
            yield return null;
        }
    }

    private IEnumerator EndGame()
    {
        MainGameUiManager.instance.resultUI.ShowEndResult(winner, winReason);
        yield return null;
    }

    public void SetWinner(int team, WinReason reason)
    {
        winner = team;
        winReason = reason;
        isGameover = true;
    }


    public Client CreateClientManager()
    {
        GameObject clientManager = new GameObject();
        clientManager.name = "Client Manager";
        clientManager.AddComponent<Client>();
        return clientManager.GetComponent<Client>();
    }

    public Server CreateServerManager()
    {
        GameObject serverManager = new GameObject();
        serverManager.name = "Server Manager";
        serverManager.AddComponent<Server>();
        return serverManager.GetComponent<Server>();
    }

}
