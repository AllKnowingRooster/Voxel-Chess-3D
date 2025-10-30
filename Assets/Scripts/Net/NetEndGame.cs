using Unity.Collections;
using UnityEngine;

public class NetEndGame : NetMessage
{
    public NetEndGame() {
        opcode = OpCode.END_GAME;
    }

    public NetEndGame(DataStreamReader streamReader)
    {
        opcode = OpCode.END_GAME;
        Deserialize(streamReader);
    }

    public override void Serialize(ref DataStreamWriter streamWriter)
    {
        base.Serialize(ref streamWriter);
    }

    public override void Deserialize(DataStreamReader streamReader)
    {
        base.Deserialize(streamReader);
    }

    public override void ReceivedOnClient()
    {
        base.ReceivedOnClient();
    }

}
