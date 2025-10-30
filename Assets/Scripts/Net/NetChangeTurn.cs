using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public class NetChangeTurn : NetMessage
{
    public NetChangeTurn()
    {
        opcode = OpCode.CHANGE_TURN;
    }

    public NetChangeTurn(DataStreamReader streamReader)
    {
        opcode = OpCode.CHANGE_TURN;
        Deserialize(streamReader);
    }

    public override void Deserialize(DataStreamReader streamReader)
    {
        
    }

    public override void Serialize(ref DataStreamWriter streamWriter)
    {
        streamWriter.WriteByte((byte)opcode);
    }

    public override void ReceivedOnClient()
    {
        NetUtility.C_CHANGE_TURN?.Invoke(this);
    }

    public override void ReceivedOnServer(NetworkConnection connection)
    {
        NetUtility.S_CHANGE_TURN.Invoke(this, connection);
    }


}
