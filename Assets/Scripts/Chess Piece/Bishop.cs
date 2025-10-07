using System.Collections.Generic;
using UnityEngine;

public class Bishop : ChessPiece
{
    public override (List<Vector2Int>, List<Vector2Int>) GetAllPossibleMoves(ref ChessPiece[,] pieceOnBoard)
    {
        List<Vector2Int> listMove= new List<Vector2Int>();
        List<Vector2Int> listKillable= new List<Vector2Int>();

        for (int i=1;i<8;i++)
        {
            int newXPos = XPos - i;
            int newYPos = YPos + i;
            if (newXPos<0 || newYPos >7)
            {
                break;
            }else if (pieceOnBoard[newXPos,newYPos]!=null)
            {
                if (pieceOnBoard[newXPos,newYPos].team!=team)
                {
                    listKillable.Add(new Vector2Int(newXPos,newYPos));
                }
                break;
            }
            listMove.Add(new Vector2Int(newXPos, newYPos));
        }

        for (int i=1;i<8;i++)
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
            if (newXPos > 7 || newYPos<0)
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
