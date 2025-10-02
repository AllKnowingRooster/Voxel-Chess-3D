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
    private int team;
    private ChessPieceType type;
    private int XPos;
    private int YPos;
}
