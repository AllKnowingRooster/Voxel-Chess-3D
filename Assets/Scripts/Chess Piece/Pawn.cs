using System;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessPiece
{
    public int totalMove = 0;
    public Vector2Int prevPosition;

    public override (List<Vector2Int>, List<Vector2Int>) GetAllPossibleMoves(ref ChessPiece[,] pieceOnBoard)
    {
        List<Vector2Int> listMove = new List<Vector2Int>();
        List<Vector2Int> listKillable=new List<Vector2Int>();
        int direction = team == 0 ? 1 : -1;
        int border = team == 0 ? 7 : 0;


            if ( ( team==0 && XPos+direction<=border) || (team==1 && XPos+direction>=border) )
            {
                if (pieceOnBoard[XPos+direction,YPos]==null)
                {
                    listMove.Add(new Vector2Int(XPos+direction,YPos));
                    if (totalMove == 0 && pieceOnBoard[XPos + direction + direction, YPos] == null)
                    {
                        listMove.Add(new Vector2Int(XPos+direction+direction,YPos));
                    }
                }
            }

        if (((team == 0 && XPos + direction <= border) || (team == 1 && XPos + direction >= border)) && YPos+1<=7)
        {
            if (pieceOnBoard[XPos+direction,YPos+1]!=null)
            {
                if (pieceOnBoard[XPos+direction,YPos+1].team!=team)
                {
                    listKillable.Add(new Vector2Int(XPos+direction,YPos+1));
                }
            }
        }

        if (((team == 0 && XPos + direction <= border) || (team == 1 && XPos + direction >= border)) && YPos - 1 >= 0)
        {
            if (pieceOnBoard[XPos + direction, YPos - 1] != null)
            {
                if (pieceOnBoard[XPos + direction, YPos - 1].team != team)
                {
                    listKillable.Add(new Vector2Int(XPos + direction, YPos - 1));
                }
            }
        }

        if ((XPos+direction<=border && team==0) || (XPos+direction>=border && team==1) )
        {
            if (YPos+1<=7)
            {
                if (pieceOnBoard[XPos, YPos + 1] != null && pieceOnBoard[XPos, YPos+1].type == ChessPieceType.Pawn && pieceOnBoard[XPos,YPos+1].team!=team)
                {
                    Pawn pawnPiece = pieceOnBoard[XPos, YPos + 1].GetComponent<Pawn>();
                    if (Math.Abs(pawnPiece.XPos-pawnPiece.prevPosition.x)==2 && pawnPiece.totalMove==1 )
                    {
                        listKillable.Add(new Vector2Int(pawnPiece.XPos+direction,pawnPiece.YPos));
                    }
                }
            }

            if (YPos - 1 >= 0)
            {
                if (pieceOnBoard[XPos, YPos - 1] != null && pieceOnBoard[XPos, YPos-1].type == ChessPieceType.Pawn && pieceOnBoard[XPos,YPos-1].team!=team)
                {
                    Pawn pawnPiece = pieceOnBoard[XPos, YPos - 1].GetComponent<Pawn>();
                    if (Math.Abs(pawnPiece.XPos - pawnPiece.prevPosition.x) == 2 && pawnPiece.totalMove == 1)
                    {
                        listKillable.Add(new Vector2Int(pawnPiece.XPos + direction, pawnPiece.YPos));
                    }
                }
            }
        }

        return (listMove,listKillable);
    }
}
