using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public class NetMakeMove : NetMessage
{
    public int xPos;
    public int yPos;
    public int originalXPos;
    public int originalYPos;

    public NetMakeMove()
    {
        opcode = OpCode.MAKE_MOVE;
    }

    public NetMakeMove(DataStreamReader streamReader)
    {
        opcode= OpCode.MAKE_MOVE;
        Deserialize(streamReader);
    }

    public override void Deserialize(DataStreamReader streamReader)
    {
        xPos=streamReader.ReadInt();
        yPos=streamReader.ReadInt();
        originalXPos=streamReader.ReadInt();
        originalYPos=streamReader.ReadInt();
    }

    public override void Serialize(ref DataStreamWriter streamWriter)
    {
        streamWriter.WriteByte((byte)opcode);
        streamWriter.WriteInt(xPos);
        streamWriter.WriteInt(yPos);
        streamWriter.WriteInt(originalXPos);
        streamWriter.WriteInt(originalYPos);
    }

    public override void ReceivedOnClient()
    {
        NetUtility.C_MAKE_MOVE?.Invoke(this);
    }

    public override void ReceivedOnServer(NetworkConnection connection)
    {
        NetUtility.S_MAKE_MOVE?.Invoke(this,connection);
    }
}
