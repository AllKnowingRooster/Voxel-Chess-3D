using System.Collections.Generic;
using UnityEngine;

public class Queen : ChessPiece
{
    public override (List<Vector2Int>, List<Vector2Int>) GetAllPossibleMoves(ref ChessPiece[,] pieceOnBoard)
    {
        List<Vector2Int> listMove = new List<Vector2Int>();
        List<Vector2Int> listKillable = new List<Vector2Int>();


        //left
        for (int i = 1; i < 8; i++)
        {
            if (XPos - i < 0)
            {
                break;
            }
            else if (pieceOnBoard[XPos - i, YPos] != null)
            {
                if (pieceOnBoard[XPos - i, YPos].team != team)
                {
                    listKillable.Add(new Vector2Int(XPos - i, YPos));
                }
                break;
            }
            listMove.Add(new Vector2Int(XPos - i, YPos));
        }

        //top
        for (int i = 1; i < 8; i++)
        {
            if (YPos + i > 7)
            {
                break;
            }
            else if (pieceOnBoard[XPos, YPos + i] != null)
            {
                if (pieceOnBoard[XPos, YPos + i].team != team)
                {
                    listKillable.Add(new Vector2Int(XPos, YPos + i));
                }
                break;
            }
            listMove.Add(new Vector2Int(XPos, YPos + i));
        }

        //right
        for (int i = 1; i < 8; i++)
        {
            if (XPos + i > 7)
            {
                break;
            }
            else if (pieceOnBoard[XPos + i, YPos] != null)
            {
                if (pieceOnBoard[XPos + i, YPos].team != team)
                {
                    listKillable.Add(new Vector2Int(XPos + i, YPos));
                }
                break;
            }
            listMove.Add(new Vector2Int(XPos + i, YPos));
        }

        //right
        for (int i = 1; i < 8; i++)
        {
            if (YPos - i < 0)
            {
                break;
            }
            else if (pieceOnBoard[XPos, YPos - i] != null)
            {
                if (pieceOnBoard[XPos, YPos - i].team != team)
                {
                    listKillable.Add(new Vector2Int(XPos, YPos - i));
                }
                break;
            }
            listMove.Add(new Vector2Int(XPos, YPos - i));
        }


        for (int i = 1; i < 8; i++)
        {
            int newXPos = XPos - i;
            int newYPos = YPos + i;
            if (newXPos < 0 || newYPos > 7)
            {
                break;
            }
            else if (pieceOnBoard[newXPos, newYPos] != null)
            {
                if (pieceOnBoard[newXPos, newYPos].team != team)
                {
                    listKillable.Add(new Vector2Int(newXPos, newYPos));
                }
                break;
            }
            listMove.Add(new Vector2Int(newXPos, newYPos));
        }

        for (int i = 1; i < 8; i++)
        {
            int newXPos = XPos + i;
            int newYPos = YPos + i;
            if (newXPos > 7 || newYPos > 7)
            {
                break;
            }
            else if (pieceOnBoard[newXPos, newYPos] != null)
            {
                if (pieceOnBoard[newXPos, newYPos].team != team)
                {
                    listKillable.Add(new Vector2Int(newXPos, newYPos));
                }
                break;
            }
            listMove.Add(new Vector2Int(newXPos, newYPos));
        }


        //left down diagonal;
        for (int i = 1; i < 8; i++)
        {
            int newXPos = XPos - i;
            int newYPos = YPos - i;
            if (newXPos < 0 || newYPos < 0)
            {
                break;
            }
            else if (pieceOnBoard[newXPos, newYPos] != null)
            {
                if (pieceOnBoard[newXPos, newYPos].team != team)
                {
                    listKillable.Add(new Vector2Int(newXPos, newYPos));
                }
                break;
            }
            listMove.Add(new Vector2Int(newXPos, newYPos));
        }


        //right down diagonal;
        for (int i = 1; i < 8; i++)
        {
            int newXPos = XPos + i;
            int newYPos = YPos - i;
            if (newXPos > 7 || newYPos < 0)
            {
                break;
            }
            else if (pieceOnBoard[newXPos, newYPos] != null)
            {
                if (pieceOnBoard[newXPos, newYPos].team != team)
                {
                    listKillable.Add(new Vector2Int(newXPos, newYPos));
                }
                break;
            }
            listMove.Add(new Vector2Int(newXPos, newYPos));
        }

        return (listMove, listKillable);

    }
}
