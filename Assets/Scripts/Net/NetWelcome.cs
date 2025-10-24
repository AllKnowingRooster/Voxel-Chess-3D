using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public class NetWelcome : NetMessage
{

    public int assignTeam;

    public NetWelcome()
    {
        opcode = OpCode.WELCOME;
    }

    public NetWelcome(DataStreamReader streamReader)
    {
        opcode= OpCode.WELCOME;
        Deserialize(streamReader);
    }

    public override void Deserialize(DataStreamReader streamReader)
    {
        assignTeam = streamReader.ReadInt();
    }

    public override void Serialize(ref DataStreamWriter streamWriter)
    {
        streamWriter.WriteByte((byte) opcode);
        Debug.Log(assignTeam);
        streamWriter.WriteInt(assignTeam);
    }

    public override void ReceivedOnClient()
    {
        NetUtility.C_WELCOME.Invoke(this);
    }

    public override void ReceivedOnServer(NetworkConnection connection)
    {
        NetUtility.S_WELCOME.Invoke(this,connection);
    }
}
