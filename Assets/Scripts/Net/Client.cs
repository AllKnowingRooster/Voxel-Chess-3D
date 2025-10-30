using System;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Client : MonoBehaviour
{

    public static Client instance { get; private set; }
    private NetworkDriver driver;
    private NetworkConnection connection;
    private bool isActive = false;
    private Action connectionDropped;


    private void Awake()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Init(string ip,ushort port)
    {
        driver = NetworkDriver.Create(new NetworkSettings());
        NetworkEndpoint endPoint=NetworkEndpoint.Parse(ip, port);
        endPoint.Port = port;
        connection=driver.Connect(endPoint);
        isActive= true;
        RegisterToEvent();
    }

    public void Shutdown()
    {
        if (isActive)
        {
            UnregisterToEvent();
            driver.Dispose();
            connection=default(NetworkConnection);
            isActive = false;
        }
    }

    private void OnDestroy()
    {
        Shutdown();
    }

    void Update()
    {
        if (!isActive)
        {
            return;
        }
        driver.ScheduleUpdate().Complete();
        CheckAlive();
        UpdateMessagePump();
    }

    void CheckAlive()
    {
        if (!connection.IsCreated && isActive)
        {
            Debug.Log("Something Bad Happened");
            connectionDropped?.Invoke();
            Shutdown();
        }
    }

    void UpdateMessagePump()
    {
        DataStreamReader streamReader;
            NetworkEvent.Type cmd;
            while ((cmd = driver.PopEventForConnection(connection, out streamReader)) != NetworkEvent.Type.Empty)
            {
                if (cmd == NetworkEvent.Type.Data)
                {
                    NetUtility.OnData(streamReader, connection);
                }
                else if (cmd == NetworkEvent.Type.Disconnect)
                {
                    Debug.Log("Client Disconnected from Server");
                    connection = default(NetworkConnection);
                    connectionDropped?.Invoke();
                    Shutdown();
                }
                else if(cmd==NetworkEvent.Type.Connect)
                {
                    Debug.Log("Connected");
                    SendToServer(new NetWelcome());
                }
            }
    }

    public void SendToServer(NetMessage msg)
    {
        DataStreamWriter streamWriter;
        driver.BeginSend(connection,out streamWriter);
        msg.Serialize(ref streamWriter);
        driver.EndSend(streamWriter);
    }

    private void RegisterToEvent()
    {
        NetUtility.C_KEEP_ALIVE += OnKeepAlive;
        NetUtility.C_WELCOME += OnWelcomeClient;
        NetUtility.C_START_GAME += OnStartGame;
    }

    private void UnregisterToEvent()
    {
        NetUtility.C_KEEP_ALIVE -= OnKeepAlive;
        NetUtility.C_WELCOME -= OnWelcomeClient;
        NetUtility.C_START_GAME -= OnStartGame; 
    }

    void OnKeepAlive(NetMessage msg)
    {
        SendToServer(msg);
    }

    void OnWelcomeClient(NetMessage msg)
    {
        NetWelcome welcome = msg as NetWelcome;
        GameManager.instance.assignedTeam = welcome.assignTeam;
        Debug.Log(string.Format("Client Assign To Team {0}", welcome.assignTeam));
    }


    void OnStartGame(NetMessage msg)
    {
        SceneManager.LoadScene(1);
    }



}
