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
    PROMOTE=5,
    CHANGE_TURN=6,
    END_GAME=7,
    REMATCH = 8,
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
        else if (opcode==OpCode.PROMOTE)
        {
            msg = new NetPromote(streamReader);
        }
        else if (opcode==OpCode.CHANGE_TURN)
        {
            msg=new NetChangeTurn(streamReader);
        }
        else if (opcode == OpCode.END_GAME)
        {
            msg = new NetEndGame(streamReader);
        }
        else if (opcode == OpCode.REMATCH)
        {
            msg = new NetRematch(streamReader);
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
    public static Action<NetMessage> C_PROMOTE;
    public static Action<NetMessage> C_END_GAME;
    public static Action<NetMessage> C_CHANGE_TURN;
    public static Action<NetMessage> C_REMATCH;
    public static Action<NetMessage, NetworkConnection> S_WELCOME;
    public static Action<NetMessage, NetworkConnection> S_START_GAME;
    public static Action<NetMessage, NetworkConnection> S_MAKE_MOVE;
    public static Action<NetMessage, NetworkConnection> S_PROMOTE;
    public static Action<NetMessage, NetworkConnection> S_CHANGE_TURN;
    public static Action<NetMessage, NetworkConnection> S_END_GAME;
    public static Action<NetMessage, NetworkConnection> S_REMATCH;
}
