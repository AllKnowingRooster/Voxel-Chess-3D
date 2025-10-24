using System.IO;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public class NetStartGame : NetMessage
{
    public NetStartGame()
    {
        opcode = OpCode.START_GAME;
    }

    public NetStartGame(DataStreamReader streamReader)
    {
        opcode= OpCode.START_GAME;
        Deserialize(streamReader);
    }

    public override void Serialize(ref DataStreamWriter streamWriter)
    {
        streamWriter.WriteByte((byte)opcode);
    }

    public override void Deserialize(DataStreamReader streamReader)
    {

    }

    public override void ReceivedOnClient()
    {
        NetUtility.C_START_GAME?.Invoke(this);
    }

    /*
    public override void ReceivedOnServer(NetworkConnection connection)
    {
         NetUtility.S_START_GAME.Invoke(this, connection);
    }
    */
}
