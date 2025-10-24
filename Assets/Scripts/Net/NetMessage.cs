using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public class NetMessage 
{

    public OpCode opcode;

    public virtual void Serialize(ref DataStreamWriter streamWriter)
    {
        streamWriter.WriteByte((byte)opcode);
    }
    
    public virtual void Deserialize(DataStreamReader streamReader)
    {

    }
    
    public virtual void ReceivedOnClient()
    {

    }

    public virtual void ReceivedOnServer(NetworkConnection connection)
    {

    }
}
