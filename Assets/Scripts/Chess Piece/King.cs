using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class King : ChessPiece
{
    public bool isChecked = false;
    public bool haveMoved= false;
    public Rook kingSideRook;
    public Rook queenSideRook;

    public override List<Vector2Int> GetAllPossibleMoves(ref ChessPiece[,] pieceOnBoard)
    {
        List<Vector2Int> listMove = new List<Vector2Int>();

        if (XPos+1<=7)
        {
            if (pieceOnBoard[XPos + 1, YPos] == null)
            {
                listMove.Add(new Vector2Int(XPos + 1, YPos));
            }
        }

        if (XPos -1 >=0)
        {
            if (pieceOnBoard[XPos - 1, YPos] == null)
            {
                    listMove.Add(new Vector2Int(XPos - 1, YPos));
            }
        }

        if (YPos + 1 <= 7)
        {
            if (pieceOnBoard[XPos, YPos + 1] == null)
            {
                listMove.Add(new Vector2Int(XPos, YPos + 1));
                if (!isChecked && !haveMoved && !queenSideRook.haveMoved && pieceOnBoard[XPos, YPos + 2] == null && pieceOnBoard[XPos, YPos + 3] == null && pieceOnBoard[XPos, YPos + 4] ==queenSideRook)
                {
                    listMove.Add(new Vector2Int(XPos,YPos+2));
                }
            }
        }

        if (YPos - 1>=0)
        {
            if (pieceOnBoard[XPos, YPos - 1] == null)
            {
                listMove.Add(new Vector2Int(XPos, YPos - 1));

                if ( !isChecked && !haveMoved && !kingSideRook.haveMoved && pieceOnBoard[XPos, YPos - 2] == null && pieceOnBoard[XPos, YPos - 3] == kingSideRook)
                {
                    listMove.Add(new Vector2Int(XPos, YPos - 2));
                }
            }
        }

        if(XPos-1>=0 && YPos + 1 <= 7)
        {
            if (pieceOnBoard[XPos - 1, YPos + 1] == null)
            {
                listMove.Add(new Vector2Int(XPos - 1, YPos + 1));
            }
        }

        if (XPos - 1 >= 0 && YPos - 1 >= 0)
        {
            if (pieceOnBoard[XPos - 1, YPos - 1] == null)
            {
                listMove.Add(new Vector2Int(XPos - 1, YPos - 1));
            }
        }

        if (XPos + 1 <= 7 && YPos - 1 >= 0)
        {
            if (pieceOnBoard[XPos + 1, YPos - 1] == null)
            {
                listMove.Add(new Vector2Int(XPos + 1, YPos - 1));
            }
        }

        if (XPos + 1 <=7 && YPos + 1 <= 7)
        {
            if (pieceOnBoard[XPos + 1, YPos + 1] == null)
            {
                listMove.Add(new Vector2Int(XPos + 1, YPos + 1));
            }
        }

        return listMove;
    }

    public override List<Vector2Int> GetAllPossibleAttack(ref ChessPiece[,] pieceOnBoard)
    {
        List<Vector2Int> listAttack = new List<Vector2Int>();

        if (XPos + 1 <= 7)
        {
            if (pieceOnBoard[XPos + 1, YPos] != null)
            {
                if (pieceOnBoard[XPos + 1, YPos].team != team)
                {
                    listAttack.Add(new Vector2Int(XPos + 1, YPos));
                            
                }
            }
        }

        if (XPos - 1 >= 0)
        {
            if (pieceOnBoard[XPos - 1, YPos] != null)
            {
                if (pieceOnBoard[XPos - 1, YPos].team != team)
                {
                    listAttack.Add(new Vector2Int(XPos - 1, YPos));
                }
            }
        }

        if (YPos + 1 <= 7)
        {
            if (pieceOnBoard[XPos, YPos + 1] != null)
            {
                if (pieceOnBoard[XPos, YPos + 1].team != team)
                {
                    listAttack.Add(new Vector2Int(XPos, YPos + 1));
                }
            }
        }

        if (YPos - 1 >= 0)
        {
            if (pieceOnBoard[XPos, YPos - 1] != null)
            {
                if (pieceOnBoard[XPos, YPos - 1].team != team)
                {
                    listAttack.Add(new Vector2Int(XPos, YPos - 1));
                }
            }
        }

        if (XPos - 1 >= 0 && YPos + 1 <= 7)
        {
            if (pieceOnBoard[XPos - 1, YPos + 1] != null)
            {
                if (pieceOnBoard[XPos - 1, YPos + 1].team != team)
                {
                    listAttack.Add(new Vector2Int(XPos - 1, YPos + 1));
                }
            }
        }

        if (XPos - 1 >= 0 && YPos - 1 >= 0)
        {
            if (pieceOnBoard[XPos - 1, YPos - 1] != null)
            {
                if (pieceOnBoard[XPos - 1, YPos - 1].team != team)
                {
                    listAttack.Add(new Vector2Int(XPos - 1, YPos - 1));
                }
            }
        }

        if (XPos + 1 <= 7 && YPos - 1 >= 0)
        {
            if (pieceOnBoard[XPos + 1, YPos - 1] != null)
            {
                if (pieceOnBoard[XPos + 1, YPos - 1].team != team)
                {
                    listAttack.Add(new Vector2Int(XPos + 1, YPos - 1));
                }
            }
        }

        if (XPos + 1 <= 7 && YPos + 1 <= 7)
        {
            if (pieceOnBoard[XPos + 1, YPos + 1] != null)
            {
                if (pieceOnBoard[XPos + 1, YPos + 1].team != team)
                {
                    listAttack.Add(new Vector2Int(XPos + 1, YPos + 1));
                }
            }
        }

        return listAttack;
    }

    public override List<Vector2Int> ProjectAttack(ref ChessPiece[,] pieceOnBoard, Vector2Int ignoredPosition)
    {

        List<Vector2Int> listAttack = new List<Vector2Int>();

        if (XPos + 1 <= 7)
        {
            listAttack.Add(new Vector2Int(XPos + 1, YPos));
        }

        if (XPos - 1 >= 0)
        {
            listAttack.Add(new Vector2Int(XPos - 1, YPos));
        }

        if (YPos + 1 <= 7)
        {
            listAttack.Add(new Vector2Int(XPos, YPos + 1));
        }

        if (YPos - 1 >= 0)
        {
            listAttack.Add(new Vector2Int(XPos, YPos - 1));
        }

        if (XPos - 1 >= 0 && YPos + 1 <= 7)
        {
            listAttack.Add(new Vector2Int(XPos - 1, YPos + 1));
        }

        if (XPos - 1 >= 0 && YPos - 1 >= 0)
        {
            listAttack.Add(new Vector2Int(XPos - 1, YPos - 1));
        }

        if (XPos + 1 <= 7 && YPos - 1 >= 0)
        {
            listAttack.Add(new Vector2Int(XPos + 1, YPos - 1));
        }

        if (XPos + 1 <= 7 && YPos + 1 <= 7)
        {
            listAttack.Add(new Vector2Int(XPos + 1, YPos + 1));
        }

        return listAttack;
    }

}
