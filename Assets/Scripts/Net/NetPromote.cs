using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public class NetPromote : NetMessage
{
    public ChessPieceType pieceType;
    public int x;
    public int y;


    public NetPromote()
    {
        opcode = OpCode.PROMOTE;
    }

    public NetPromote(DataStreamReader streamReader)
    {
        opcode=OpCode.PROMOTE;
        Deserialize(streamReader);
    }

    public override void Deserialize(DataStreamReader streamReader)
    {
        pieceType=(ChessPieceType)streamReader.ReadByte();
        x = streamReader.ReadInt();
        y = streamReader.ReadInt();
    }

    public override void Serialize(ref DataStreamWriter streamWriter)
    {
        streamWriter.WriteByte((byte)opcode);
        streamWriter.WriteByte((byte)pieceType);
        streamWriter.WriteInt(x);
        streamWriter.WriteInt(y);
    }

    public override void ReceivedOnClient()
    {
        NetUtility.C_PROMOTE?.Invoke(this);
    }

    public override void ReceivedOnServer(NetworkConnection connection)
    {
        NetUtility.S_PROMOTE?.Invoke(this, connection);
    }
}
