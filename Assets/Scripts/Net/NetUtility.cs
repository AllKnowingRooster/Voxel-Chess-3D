using System;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public enum OpCode
{
    KEEP_ALIVE = 1,
    WELCOME = 2,
    START_GAME = 3,
    MAKE_MOVE = 4,
    REMATCH = 5,
}
public static class NetUtility 
{
    public static void OnData(DataStreamReader streamReader,NetworkConnection connection,Server server = null)
    {
        NetMessage msg = null;
        OpCode opcode = (OpCode)streamReader.ReadByte();
        Debug.Log(opcode);
        if (opcode == OpCode.KEEP_ALIVE)
        {
            msg = new NetKeepAlive(streamReader);
        }
        else if (opcode == OpCode.WELCOME)
        {
            msg = new NetWelcome(streamReader);
        }
        else if (opcode == OpCode.START_GAME)
        {
            msg= new NetStartGame(streamReader);
        }
        else if (opcode == OpCode.MAKE_MOVE)
        {
            msg=new NetMakeMove(streamReader);
        }
        else if (opcode == OpCode.REMATCH)
        {

        }

        if (server != null) {
            msg.ReceivedOnServer(connection);
        }
        else
        {
            msg.ReceivedOnClient();
        }

    }

    public static Action<NetMessage> C_KEEP_ALIVE;
    public static Action<NetMessage> C_WELCOME;
    public static Action<NetMessage> C_START_GAME;
    public static Action<NetMessage> C_MAKE_MOVE;
    public static Action<NetMessage> C_REMATCH;
    public static Action<NetMessage, NetworkConnection> S_WELCOME;
    public static Action<NetMessage, NetworkConnection> S_START_GAME;
    public static Action<NetMessage, NetworkConnection> S_MAKE_MOVE;
    public static Action<NetMessage, NetworkConnection> S_REMATCH;
}
