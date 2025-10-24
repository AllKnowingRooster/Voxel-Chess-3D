using Unity.Collections;
using Unity.Networking.Transport;
public class NetKeepAlive : NetMessage
{

    public NetKeepAlive() {
        opcode = OpCode.KEEP_ALIVE;
    }

    public NetKeepAlive(DataStreamReader streamReader)
    {
        opcode = OpCode.KEEP_ALIVE;
        Deserialize(streamReader);
    }

    public override void ReceivedOnClient()
    {
        NetUtility.C_KEEP_ALIVE?.Invoke(this);
    }
}
