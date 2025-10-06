using System.Collections.Generic;
using UnityEngine;

public class Knight : ChessPiece
{
    public override (List<Vector2Int>, List<Vector2Int>) GetAllPossibleMoves(ref ChessPiece[,] pieceOnBoard)
    {
        List<Vector2Int> listMove = new List<Vector2Int>();
        List<Vector2Int> listKillable = new List<Vector2Int>();
        int forwardSpace = 2;
        int sideSpace = 1;
        if (XPos+forwardSpace<=7)
        {
            if (YPos+sideSpace<=7)
            {
                if (pieceOnBoard[XPos + forwardSpace, YPos + sideSpace] != null)
                {
                    listKillable.Add(new Vector2Int(XPos + forwardSpace, YPos + sideSpace));
                }
                else
                {
                    listMove.Add(new Vector2Int(XPos + forwardSpace, YPos + sideSpace));
                }
            }

            if (YPos-1>=0)
            {
                if (pieceOnBoard[XPos+forwardSpace,YPos-sideSpace]!=null)
                {
                    listKillable.Add(new Vector2Int(XPos+forwardSpace, YPos-sideSpace));   
                }
                else
                {
                    listMove.Add(new Vector2Int(XPos + forwardSpace, YPos - sideSpace));
                }
            }
        }

        if (XPos -forwardSpace >= 0)
        {
            if (YPos + sideSpace <= 7)
            {
                if (pieceOnBoard[XPos - forwardSpace, YPos + sideSpace] != null)
                {
                    listKillable.Add(new Vector2Int(XPos - forwardSpace, YPos + sideSpace));
                }
                else
                {
                    listMove.Add(new Vector2Int(XPos - forwardSpace, YPos + sideSpace));
                }
            }

            if (YPos - sideSpace >= 0)
            {
                if (pieceOnBoard[XPos - forwardSpace, YPos - sideSpace] != null)
                {
                    listKillable.Add(new Vector2Int(XPos - forwardSpace, YPos - sideSpace));
                }
                else
                {
                    listMove.Add(new Vector2Int(XPos - forwardSpace, YPos - sideSpace));
                }
            }
        }

        if (YPos - forwardSpace >= 0)
        {
            if (XPos - sideSpace >= 0)
            {
                if (pieceOnBoard[XPos - sideSpace, YPos - forwardSpace] != null)
                {
                    listKillable.Add(new Vector2Int(XPos-sideSpace, YPos -forwardSpace));
                }
                else
                {
                    listMove.Add(new Vector2Int(XPos -sideSpace , YPos -forwardSpace ));
                }
            }

            if (XPos + 1 <= 7)
            {
                if (pieceOnBoard[XPos + sideSpace, YPos - forwardSpace] != null)
                {
                    listKillable.Add(new Vector2Int(XPos + sideSpace, YPos - forwardSpace));
                }
                else
                {
                    listMove.Add(new Vector2Int(XPos + sideSpace, YPos - forwardSpace));
                }
            }
        }

        if (YPos + 3 <= 7)
        {
            if (XPos - 1 >= 0)
            {
                if (pieceOnBoard[XPos - sideSpace, YPos + forwardSpace] != null)
                {
                    listKillable.Add(new Vector2Int(XPos - 1, YPos + forwardSpace));
                }
                else
                {
                    listMove.Add(new Vector2Int(XPos - 1, YPos + forwardSpace));
                }
            }

            if (XPos + 1 <= 7)
            {
                if (pieceOnBoard[XPos + 1, YPos + 3] != null)
                {
                    listKillable.Add(new Vector2Int(XPos + 1, YPos + 3));
                }
                else
                {
                    listMove.Add(new Vector2Int(XPos + 1, YPos + 3));
                }
            }
        }


        return (listMove, listKillable);

    }
}
