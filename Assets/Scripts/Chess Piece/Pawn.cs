using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessPiece
{
    public bool isFirstMove = true;
    public override (List<Vector2Int>, List<Vector2Int>) GetAllPossibleMoves(ref ChessPiece[,] pieceOnBoard)
    {
        List<Vector2Int> listMove = new List<Vector2Int>();
        List<Vector2Int> listKillable=new List<Vector2Int>();
        int direction = team == 0 ? 1 : -1;

        if (pieceOnBoard[XPos+direction,YPos]==null)
        {
            listMove.Add(new Vector2Int(XPos+direction,YPos));
            if (isFirstMove && (pieceOnBoard[XPos + direction + direction, YPos] == null))
            {
                listMove.Add(new Vector2Int(XPos + direction + direction, YPos));
            }
        }

        if (YPos != 7 &&pieceOnBoard[XPos+direction,YPos+1]!=null)
        {
            listKillable.Add(new Vector2Int(XPos+direction,YPos+1));
        }

        if (YPos!=0 && pieceOnBoard[XPos+direction,YPos-1]!=null)
        {
            listKillable.Add(new Vector2Int(XPos+direction,YPos-1));
        }

        return (listMove,listKillable);
    }
}
