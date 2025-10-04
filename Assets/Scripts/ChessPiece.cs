using UnityEngine;


public enum ChessPieceType
{
    Pawn = 0,
    Knight = 1,
    Bishop = 2,
    Rook = 3,
    Queen = 4,
    King = 5
}
public class ChessPiece : MonoBehaviour
{
    public int team;
    private ChessPieceType type;
    public int XPos;
    public int YPos;

    public Vector3 desiredPos;

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, desiredPos, Time.deltaTime * 5);
    }

    public void SetPosition(Vector3 position,bool force)
    {
        desiredPos = position;
        if (force)
        {
            transform.position = desiredPos;
        }
    }

}
