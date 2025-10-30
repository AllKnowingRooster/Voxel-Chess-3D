using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public class NetRematch : NetMessage
{

    public int teamId;
    public int wantRematch;
    public NetRematch() {
        opcode = OpCode.REMATCH;    
    }

    public NetRematch(DataStreamReader streamReader)
    {
        opcode = OpCode.REMATCH;
        Deserialize(streamReader);
    }

    public override void Deserialize(DataStreamReader streamReader)
    {
        teamId=streamReader.ReadInt();
        wantRematch=streamReader.ReadInt();
    }

    public override void Serialize(ref DataStreamWriter streamWriter)
    {
        streamWriter.WriteByte((byte)opcode);
        streamWriter.WriteInt(teamId);
        streamWriter.WriteInt(wantRematch);
    }

    public override void ReceivedOnClient()
    {
        NetUtility.C_REMATCH?.Invoke(this);
    }

    public override void ReceivedOnServer(NetworkConnection connection)
    {
        NetUtility.S_REMATCH?.Invoke(this,connection);
    }
}
