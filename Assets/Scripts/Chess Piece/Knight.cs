using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
public class Knight : ChessPiece
{
    private int forwardSpace = 2;
    private int sideSpace = 1;
    public override List<Vector2Int> GetAllPossibleMoves(ref ChessPiece[,] pieceOnBoard)
    {
        List<Vector2Int> listMove = new List<Vector2Int>();
        if (XPos + forwardSpace <= 7)
        {
            if (YPos + sideSpace <= 7)
            {
                if (pieceOnBoard[XPos + forwardSpace, YPos + sideSpace] == null)
                {
                    listMove.Add(new Vector2Int(XPos + forwardSpace, YPos + sideSpace));
                }
            }

            if (YPos - 1 >= 0)
            {
                if (pieceOnBoard[XPos + forwardSpace, YPos - sideSpace] == null)
                {
                    listMove.Add(new Vector2Int(XPos + forwardSpace, YPos - sideSpace));
                }
            }
        }

        if (XPos - forwardSpace >= 0)
        {
            if (YPos + sideSpace <= 7)
            {
                if (pieceOnBoard[XPos - forwardSpace, YPos + sideSpace] == null)
                {
                    listMove.Add(new Vector2Int(XPos - forwardSpace, YPos + sideSpace));
                }
            }

            if (YPos - sideSpace >= 0)
            {
                if (pieceOnBoard[XPos - forwardSpace, YPos - sideSpace] == null)
                {
                    listMove.Add(new Vector2Int(XPos - forwardSpace, YPos - sideSpace));
                }
            }
        }

        if (YPos - forwardSpace >= 0)
        {
            if (XPos - sideSpace >= 0)
            {
                if (pieceOnBoard[XPos - sideSpace, YPos - forwardSpace] == null)
                {
                    listMove.Add(new Vector2Int(XPos - sideSpace, YPos - forwardSpace));
                }
            }

            if (XPos + 1 <= 7)
            {
                if (pieceOnBoard[XPos + sideSpace, YPos - forwardSpace] == null)
                {
                    listMove.Add(new Vector2Int(XPos + sideSpace, YPos - forwardSpace));
                }
            }
        }

        if (YPos + forwardSpace <= 7)
        {
            if (XPos - sideSpace >= 0)
            {
                if (pieceOnBoard[XPos - sideSpace, YPos + forwardSpace] == null)
                {
                    listMove.Add(new Vector2Int(XPos - sideSpace, YPos + forwardSpace));
                }
            }

            if (XPos + 1 <= 7)
            {
                if (pieceOnBoard[XPos + sideSpace, YPos + forwardSpace] == null)
                {
                    listMove.Add(new Vector2Int(XPos + sideSpace, YPos + forwardSpace));
                }
            }
        }

        return listMove;
    }

    public override List<Vector2Int> GetAllPossibleAttack(ref ChessPiece[,] pieceOnBoard)
    {
        List<Vector2Int> listAttack = new List<Vector2Int>();
        if (XPos + forwardSpace <= 7)
        {
            if (YPos + sideSpace <= 7)
            {
                if (pieceOnBoard[XPos + forwardSpace, YPos + sideSpace] != null)
                {
                    if (pieceOnBoard[XPos + forwardSpace, YPos + sideSpace].team != team)
                    {
                        listAttack.Add(new Vector2Int(XPos + forwardSpace, YPos + sideSpace));
                    }
                }
            }

            if (YPos - sideSpace >= 0)
            {
                if (pieceOnBoard[XPos + forwardSpace, YPos - sideSpace] != null)
                {
                    if (pieceOnBoard[XPos + forwardSpace, YPos - sideSpace].team != team)
                    {
                        listAttack.Add(new Vector2Int(XPos + forwardSpace, YPos - sideSpace));
                    }
                }
            }

            if (XPos - forwardSpace >= 0)
            {
                if (YPos + sideSpace <= 7)
                {
                    if (pieceOnBoard[XPos - forwardSpace, YPos + sideSpace] != null)
                    {
                        if (pieceOnBoard[XPos - forwardSpace, YPos + sideSpace].team != team)
                        {
                            listAttack.Add(new Vector2Int(XPos - forwardSpace, YPos + sideSpace));
                        }
                    }
                }

                if (YPos - sideSpace >= 0)
                {
                    if (pieceOnBoard[XPos - forwardSpace, YPos - sideSpace] != null)
                    {
                        if (pieceOnBoard[XPos - forwardSpace, YPos - sideSpace].team != team)
                        {
                            listAttack.Add(new Vector2Int(XPos - forwardSpace, YPos - sideSpace));
                        }
                    }
                }
            }

            if (YPos - forwardSpace >= 0)
            {
                if (XPos - sideSpace >= 0)
                {
                    if (pieceOnBoard[XPos - sideSpace, YPos - forwardSpace] != null)
                    {
                        if (pieceOnBoard[XPos - sideSpace, YPos - forwardSpace].team != team)
                        {
                            listAttack.Add(new Vector2Int(XPos - sideSpace, YPos - forwardSpace));
                        }
                    }
                }

                if (XPos + sideSpace <= 7)
                {
                    if (pieceOnBoard[XPos + sideSpace, YPos - forwardSpace] != null)
                    {
                        if (pieceOnBoard[XPos + sideSpace, YPos - forwardSpace].team != team)
                        {
                            listAttack.Add(new Vector2Int(XPos + sideSpace, YPos - forwardSpace));
                        }
                    }
                }
            }

            if (YPos + forwardSpace <= 7)
            {
                if (XPos - sideSpace >= 0)
                {
                    if (pieceOnBoard[XPos - sideSpace, YPos + forwardSpace] != null)
                    {
                        if (pieceOnBoard[XPos - sideSpace, YPos + forwardSpace].team != team)
                        {
                            listAttack.Add(new Vector2Int(XPos - sideSpace, YPos + forwardSpace));
                        }
                    }
                }

                if (XPos + sideSpace <= 7)
                {
                    if (pieceOnBoard[XPos + sideSpace, YPos + forwardSpace] != null)
                    {
                        if (pieceOnBoard[XPos + sideSpace, YPos + forwardSpace].team != team)
                        {
                            listAttack.Add(new Vector2Int(XPos + sideSpace, YPos + forwardSpace));
                        }
                    }
                }
            }

        }
        return listAttack;
    }

    public override List<Vector2Int> ProjectAttack(ref ChessPiece[,] pieceOnBoard, Vector2Int ignoredPosition)
    {
        List<Vector2Int> listAttack = new List<Vector2Int>();
        if (XPos + forwardSpace <= 7)
        {
            if (YPos + sideSpace <= 7)
            {
                listAttack.Add(new Vector2Int(XPos + forwardSpace, YPos + sideSpace));
            }

            if (YPos - sideSpace >= 0)
            {
                listAttack.Add(new Vector2Int(XPos + forwardSpace, YPos - sideSpace));
            }

            if (XPos - forwardSpace >= 0)
            {
                if (YPos + sideSpace <= 7)
                {
                   listAttack.Add(new Vector2Int(XPos - forwardSpace, YPos + sideSpace));
                }

                if (YPos - sideSpace >= 0)
                {
                   listAttack.Add(new Vector2Int(XPos - forwardSpace, YPos - sideSpace));
                }
            }

            if (YPos - forwardSpace >= 0)
            {
                if (XPos - sideSpace >= 0)
                {
                    listAttack.Add(new Vector2Int(XPos - sideSpace, YPos - forwardSpace));
                }

                if (XPos + sideSpace <= 7)
                {
                    listAttack.Add(new Vector2Int(XPos + sideSpace, YPos - forwardSpace));
                }
            }

            if (YPos + forwardSpace <= 7)
            {
                if (XPos - sideSpace >= 0)
                {
                    listAttack.Add(new Vector2Int(XPos - sideSpace, YPos + forwardSpace));
                }

                if (XPos + sideSpace <= 7)
                {

                    listAttack.Add(new Vector2Int(XPos + sideSpace, YPos + forwardSpace));
                }
            }

        }
        return listAttack;
    }
}

