using System;
using Unity.Collections;
using Unity.Networking.Transport;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Server : MonoBehaviour
{
    public static Server instance { get; private set; }
    private NetworkDriver driver;

    private NativeList<NetworkConnection> connections;

    private bool isActive = false;
    private float keepAliveTickRate = 20.0f;
    private float lastKeptAlive=0.0f;

    public Action connectionDropped;

    private void Awake()
    {
        if (instance!=null)
        {
            return;
        }
        instance = this;
        DontDestroyOnLoad(instance);
    }

    public void Init(ushort port)
    {
        driver = NetworkDriver.Create(new NetworkSettings());
        NetworkEndpoint endPoint = NetworkEndpoint.AnyIpv4;
        endPoint.Port = port;

        if (driver.Bind(endPoint) != 0)
        {
            Debug.Log("Cannot Bind Endpoint on Port" + endPoint.Port);
            return;
        }
        else
        {
            driver.Listen();
            Debug.Log("Currently Listening On Port " + endPoint.Port);
        }

        connections = new NativeList<NetworkConnection>(2,Allocator.Persistent);
        isActive= true;
        RegisterEvent();
    }

    private void KeepAlive()
    {
        if (Time.time - keepAliveTickRate>lastKeptAlive)
        {
            lastKeptAlive = Time.time;
            Broadcast(new NetKeepAlive());
        }
    }

    private void OnApplicationQuit()
    {
        Shutdown();
    }

    public void Shutdown()
    {
        if (isActive)
        {
            UnregisterEvent();
            driver.Dispose();
            connections.Dispose();
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
        KeepAlive();
        CleanUpConnections();
        AcceptNewConnections();
        UpdateMessagePump();
    }

    void CleanUpConnections()
    {
        for (int i=0;i<connections.Length;i++)
        {
            if (!connections[i].IsCreated)
            {
                connections.RemoveAtSwapBack(i);
                i--;
            }
        }
    }

    void AcceptNewConnections()
    {
        NetworkConnection connection;
        while ((connection=driver.Accept())!=default(NetworkConnection))
        {
            connections.Add(connection);
            Debug.Log(connections.Length);
        }
    }
    void UpdateMessagePump()
    {
        DataStreamReader streamReader;
        for (int i=0;i<connections.Length;i++)
        {
            NetworkEvent.Type cmd;
            while ((cmd = driver.PopEventForConnection(connections[i],out streamReader))!=NetworkEvent.Type.Empty)
            {
                if (cmd==NetworkEvent.Type.Data)
                {
                    NetUtility.OnData(streamReader, connections[i],this); 
                }else if ( cmd==NetworkEvent.Type.Disconnect)
                {
                    Debug.Log("Client Disconnected from Server");
                    connections[i]=default(NetworkConnection);
                    connectionDropped?.Invoke();
                    Shutdown();
                }
            }
        }
    }
    
    void SendToClient(NetworkConnection connection,NetMessage msg)
    {
        DataStreamWriter streamWriter;
        driver.BeginSend(connection, out streamWriter);
        msg.Serialize(ref streamWriter);
        driver.EndSend(streamWriter);

    }

    void Broadcast(NetMessage msg)
    {
        for (int i = 0; i < connections.Length;i++)
        {
            if (connections[i].IsCreated)
            {
                SendToClient(connections[i], msg);
            }
        }
    }

    void RegisterEvent()
    {
        NetUtility.S_WELCOME += OnWelcomeServer;
        NetUtility.S_MAKE_MOVE += OnMakeMoveServer;
        NetUtility.S_PROMOTE += OnPromoteServer;
        NetUtility.S_CHANGE_TURN += OnChangeTurnServer;
        NetUtility.S_REMATCH += OnRematchServer;
    }

    void UnregisterEvent()
    {
        NetUtility.S_WELCOME -= OnWelcomeServer;
        NetUtility.S_MAKE_MOVE -= OnMakeMoveServer;
        NetUtility.S_PROMOTE -= OnPromoteServer;
        NetUtility.S_CHANGE_TURN -= OnChangeTurnServer;
        NetUtility.S_REMATCH-= OnRematchServer;
    }

    void OnChangeTurnServer(NetMessage msg, NetworkConnection connection)
    {
        Broadcast(msg);
    }
    void OnMakeMoveServer(NetMessage msg,NetworkConnection connection)
    {
        Broadcast(msg);
    }

    void OnPromoteServer(NetMessage msg, NetworkConnection connection)
    {
        Broadcast(msg);
    }

    void OnRematchServer(NetMessage msg, NetworkConnection connection)
    {
        Broadcast(msg);
    }


    void OnWelcomeServer(NetMessage msg, NetworkConnection connection)
    {
        NetWelcome welcome = msg as NetWelcome;

        welcome.assignTeam = connections.Length % 2;

        SendToClient(connection, welcome);

          if (connections.Length==2)
          {
             Broadcast(new NetStartGame());
          }
    }

}
