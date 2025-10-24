using System.Collections.Generic;
using UnityEngine;

public class Bishop : ChessPiece
{
    public override List<Vector2Int> GetAllPossibleMoves(ref ChessPiece[,] pieceOnBoard)
    {
        List<Vector2Int> listMove= new List<Vector2Int>();

        for (int i=1;i<8;i++)
        {
            int newXPos = XPos - i;
            int newYPos = YPos + i;
            if ((newXPos<0 || newYPos >7) || pieceOnBoard[newXPos, newYPos] != null)
            {
                break;
            }
            listMove.Add(new Vector2Int(newXPos, newYPos));
        }

        for (int i=1;i<8;i++)
        {
            int newXPos = XPos + i;
            int newYPos = YPos + i;
            if ((newXPos > 7 || newYPos > 7) || pieceOnBoard[newXPos, newYPos] != null)
            {
                break;
            }
            listMove.Add(new Vector2Int(newXPos, newYPos));
        }


        for (int i = 1; i < 8; i++)
        {
            int newXPos = XPos - i;
            int newYPos = YPos - i;
            if ((newXPos < 0 || newYPos < 0)||pieceOnBoard[newXPos, newYPos] != null)
            {
                break;
            }
           
            listMove.Add(new Vector2Int(newXPos, newYPos));
        }

        for (int i = 1; i < 8; i++)
        {
            int newXPos = XPos + i;
            int newYPos = YPos - i;
            if (newXPos > 7 || newYPos<0)
            {
                break;
            }
            
            if (pieceOnBoard[newXPos, newYPos] != null)
            {
                break;
            }
            listMove.Add(new Vector2Int(newXPos, newYPos));
        }


        return listMove;
    }

    public override List<Vector2Int> GetAllPossibleAttack(ref ChessPiece[,] pieceOnBoard)
    {
        List<Vector2Int> listAttack = new List<Vector2Int>();

        for (int i = 1; i < 8; i++)
        {
            int newXPos = XPos - i;
            int newYPos = YPos + i;
            if (newXPos < 0 || newYPos > 7)
            {
                break;
            }

            if (pieceOnBoard[newXPos, newYPos] != null)
            {
                if (pieceOnBoard[newXPos, newYPos].team != team)
                {
                    listAttack.Add(new Vector2Int(newXPos, newYPos));
                }
                break;
            }
            listAttack.Add(new Vector2Int(newXPos, newYPos));
        }

        for (int i = 1; i < 8; i++)
        {
            int newXPos = XPos + i;
            int newYPos = YPos + i;
            if (newXPos > 7 || newYPos > 7)
            {
                break;
            }

            if (pieceOnBoard[newXPos, newYPos] != null)
            {
                if (pieceOnBoard[newXPos, newYPos].team != team)
                {
                    listAttack.Add(new Vector2Int(newXPos, newYPos));
                }
                break;
            }
            listAttack.Add(new Vector2Int(newXPos, newYPos));
        }


        for (int i = 1; i < 8; i++)
        {
            int newXPos = XPos - i;
            int newYPos = YPos - i;
            if (newXPos < 0 || newYPos < 0)
            {
                break;
            }

            if (pieceOnBoard[newXPos, newYPos] != null)
            {
                if (pieceOnBoard[newXPos, newYPos].team != team)
                {
                    listAttack.Add(new Vector2Int(newXPos, newYPos));
                }
                break;
            }
            listAttack.Add(new Vector2Int(newXPos, newYPos));
        }

        for (int i = 1; i < 8; i++)
        {
            int newXPos = XPos + i;
            int newYPos = YPos - i;
            if (newXPos > 7 || newYPos < 0)
            {
                break;
            }

            if (pieceOnBoard[newXPos, newYPos] != null)
            {
                if (pieceOnBoard[newXPos,newYPos].team!=team)
                {
                    listAttack.Add(new Vector2Int(newXPos, newYPos));
                }
                break;
            }
            listAttack.Add(new Vector2Int(newXPos, newYPos));
        }


        return listAttack;
    }

    public override List<Vector2Int> ProjectAttack(ref ChessPiece[,] pieceOnBoard, Vector2Int ignoredPosition)
    {
        List<Vector2Int> listAttack = new List<Vector2Int>();

        for (int i = 1; i < 8; i++)
        {
            int newXPos = XPos - i;
            int newYPos = YPos + i;
            if (newXPos < 0 || newYPos > 7)
            {
                break;
            }

            listAttack.Add(new Vector2Int(newXPos, newYPos));
            if (ignoredPosition.x == newXPos && ignoredPosition.y == newYPos)
            {
                continue;
            }
            else if (pieceOnBoard[newXPos, newYPos] != null)
            {
                break;
            }
        }

        for (int i = 1; i < 8; i++)
        {
            int newXPos = XPos + i;
            int newYPos = YPos + i;
            if (newXPos > 7 || newYPos > 7)
            {
                break;
            }

            listAttack.Add(new Vector2Int(newXPos, newYPos));
            if (ignoredPosition.x == newXPos && ignoredPosition.y == newYPos)
            {
                continue;
            }
            else if (pieceOnBoard[newXPos, newYPos] != null)
            {
                break;
            }
        }


        for (int i = 1; i < 8; i++)
        {
            int newXPos = XPos - i;
            int newYPos = YPos - i;
            if (newXPos < 0 || newYPos < 0)
            {
                break;
            }

            listAttack.Add(new Vector2Int(newXPos, newYPos));
            if (ignoredPosition.x == newXPos && ignoredPosition.y == newYPos)
            {
                continue;
            }
            else if (pieceOnBoard[newXPos, newYPos] != null)
            {
                break;
            }
        }

        for (int i = 1; i < 8; i++)
        {
            int newXPos = XPos + i;
            int newYPos = YPos - i;
            if (newXPos > 7 || newYPos < 0)
            {
                break;
            }

            listAttack.Add(new Vector2Int(newXPos, newYPos));
            if (ignoredPosition.x == newXPos && ignoredPosition.y == newYPos)
            {
                continue;
            }
            else if (pieceOnBoard[newXPos, newYPos] != null)
            {
                break;
            }
        }


        return listAttack;
    }
}
