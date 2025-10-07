using System.Collections.Generic;
using UnityEngine;

public class King : ChessPiece
{
    public override (List<Vector2Int>, List<Vector2Int>) GetAllPossibleMoves(ref ChessPiece[,] pieceOnBoard)
    {
        List<Vector2Int> listMove = new List<Vector2Int>();
        List<Vector2Int> listKillable = new List<Vector2Int>();

        if (XPos+1<=7)
        {
            if (pieceOnBoard[XPos+1,YPos]!=null)
            {
                if (pieceOnBoard[XPos+1,YPos].team!=team)
                {
                    listKillable.Add(new Vector2Int(XPos+1,YPos));
                }

            }
            else
            {
                listMove.Add(new Vector2Int(XPos+1,YPos));
            }




        }

        if (XPos -1 >=0)
        {
            if (pieceOnBoard[XPos - 1, YPos] != null)
            {
                if (pieceOnBoard[XPos - 1, YPos].team != team)
                {
                    listKillable.Add(new Vector2Int(XPos - 1, YPos));
                }

            }
            else
            {
                listMove.Add(new Vector2Int(XPos - 1, YPos));
            }
        }

        if (YPos + 1 <= 7)
        {
            if (pieceOnBoard[XPos, YPos+1] != null)
            {
                if (pieceOnBoard[XPos, YPos+1].team != team)
                {
                    listKillable.Add(new Vector2Int(XPos, YPos+1));
                }

            }
            else
            {
                listMove.Add(new Vector2Int(XPos, YPos+1));
            }
        }

        if (YPos - 1>=0)
        {
            if (pieceOnBoard[XPos, YPos - 1] != null)
            {
                if (pieceOnBoard[XPos, YPos - 1].team != team)
                {
                    listKillable.Add(new Vector2Int(XPos, YPos - 1));
                }

            }
            else
            {
                listMove.Add(new Vector2Int(XPos, YPos - 1));
            }
        }

        if(XPos-1>=0 && YPos + 1 <= 7)
        {
            if (pieceOnBoard[XPos-1, YPos + 1] != null)
            {
                if (pieceOnBoard[XPos-1, YPos + 1].team != team)
                {
                    listKillable.Add(new Vector2Int(XPos-1, YPos + 1));
                }

            }
            else
            {
                listMove.Add(new Vector2Int(XPos-1, YPos + 1));
            }
        }

        if (XPos - 1 >= 0 && YPos - 1 >= 0)
        {
            if (pieceOnBoard[XPos - 1, YPos - 1] != null)
            {
                if (pieceOnBoard[XPos - 1, YPos - 1].team != team)
                {
                    listKillable.Add(new Vector2Int(XPos - 1, YPos - 1));
                }

            }
            else
            {
                listMove.Add(new Vector2Int(XPos - 1, YPos - 1));
            }
        }

        if (XPos + 1 <= 0 && YPos - 1 >= 0)
        {
            if (pieceOnBoard[XPos + 1, YPos - 1] != null)
            {
                if (pieceOnBoard[XPos + 1, YPos - 1].team != team)
                {
                    listKillable.Add(new Vector2Int(XPos + 1, YPos - 1));
                }

            }
            else
            {
                listMove.Add(new Vector2Int(XPos + 1, YPos - 1));
            }
        }

        if (XPos + 1 <=7 && YPos + 1 <= 7)
        {
            if (pieceOnBoard[XPos + 1, YPos + 1] != null)
            {
                if (pieceOnBoard[XPos + 1, YPos + 1].team != team)
                {
                    listKillable.Add(new Vector2Int(XPos + 1, YPos + 1));
                }

            }
            else
            {
                listMove.Add(new Vector2Int(XPos + 1, YPos + 1));
            }
        }

        return (listMove, listKillable);
    }
}
