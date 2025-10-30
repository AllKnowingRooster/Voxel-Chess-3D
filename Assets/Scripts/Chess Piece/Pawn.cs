using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

public class Pawn : ChessPiece
{
    public int prevTurn = 0;
    public Vector2Int prevPosition;
    public override List<Vector2Int> GetAllPossibleMoves(ref ChessPiece[,] pieceOnBoard)
    {
        List<Vector2Int> listMove = new List<Vector2Int>();
        int direction = this.team == 0 ? 1 : -1;
        int border = base.team == 0 ? 7 : 0;

        if ((team == 0 && XPos + direction <= border) || (team == 1 && XPos + direction >= border))
        {
            if (pieceOnBoard[XPos + direction, YPos] == null)
            {
                listMove.Add(new Vector2Int(XPos + direction, YPos));

                if (prevTurn == 0 && pieceOnBoard[XPos + direction + direction, YPos] == null)
                {
                    listMove.Add(new Vector2Int(XPos + direction + direction, YPos));
                }
            }
        }

        return listMove;
    }


    public override List<Vector2Int> GetAllPossibleAttack(ref ChessPiece[,] pieceOnBoard)
    {
        List<Vector2Int> listAttack = new List<Vector2Int>();
        int direction = this.team == 0 ? 1 : -1;
        int border = base.team == 0 ? 7 : 0;
        if (((team == 0 && XPos + direction <= border) || (team == 1 && XPos + direction >= border)) && YPos + 1 <= 7)
        {
                if (pieceOnBoard[XPos + direction, YPos + 1] != null)
                {
                    if (pieceOnBoard[XPos + direction, YPos + 1].team != team)
                    {
                        listAttack.Add(new Vector2Int(XPos + direction, YPos + 1));
                    }
                }
            
        }

        if (((team == 0 && XPos + direction <= border) || (team == 1 && XPos + direction >= border)) && YPos - 1 >= 0)
        {
                if (pieceOnBoard[XPos + direction, YPos - 1] != null)
                {
                    if (pieceOnBoard[XPos + direction, YPos - 1].team != team)
                    {
                        listAttack.Add(new Vector2Int(XPos + direction, YPos - 1));
                    }
                }
        }

        if ((XPos + direction <= border && team == 0) || (XPos + direction >= border && team == 1))
        {
            if (YPos + 1 <= 7)
            {
                if (pieceOnBoard[XPos, YPos + 1] != null && pieceOnBoard[XPos, YPos + 1].type == ChessPieceType.Pawn && pieceOnBoard[XPos, YPos + 1].team != team)
                {
                    Pawn pawnPiece = pieceOnBoard[XPos, YPos + 1].GetComponent<Pawn>();
                    if (Math.Abs(pawnPiece.XPos - pawnPiece.prevPosition.x) == 2 && pawnPiece.prevTurn == ChessBoard.turnCount - 1)
                    {
                        listAttack.Add(new Vector2Int(pawnPiece.XPos + direction, pawnPiece.YPos));
                    }
                }
            }

            if (YPos - 1 >= 0)
            {
                if (pieceOnBoard[XPos, YPos - 1] != null && pieceOnBoard[XPos, YPos - 1].type == ChessPieceType.Pawn && pieceOnBoard[XPos, YPos - 1].team != team)
                {
                    Pawn pawnPiece = pieceOnBoard[XPos, YPos - 1].GetComponent<Pawn>();
                    if (Math.Abs(pawnPiece.XPos - pawnPiece.prevPosition.x) == 2 && pawnPiece.prevTurn == ChessBoard.turnCount - 1)
                    {
                        listAttack.Add(new Vector2Int(pawnPiece.XPos + direction, pawnPiece.YPos));
                    }
                }
            }
        }
        return listAttack;
    }

    public override List<Vector2Int> ProjectAttack(ref ChessPiece[,] pieceOnBoard, Vector2Int ignoredPosition)
    {
        List<Vector2Int> listAttack = new List<Vector2Int>();
        int direction = this.team == 0 ? 1 : -1;
        int border = base.team == 0 ? 7 : 0;
        if (((team == 0 && XPos + direction <= border) || (team == 1 && XPos + direction >= border)) && YPos + 1 <= 7)
        {
            listAttack.Add(new Vector2Int(XPos + direction, YPos + 1));
        }

        if (((team == 0 && XPos + direction <= border) || (team == 1 && XPos + direction >= border)) && YPos - 1 >= 0)
        {
           listAttack.Add(new Vector2Int(XPos + direction, YPos - 1));
        }

        return listAttack;
    }



    async public Task<ChessPieceType> Promote(int XPos,int YPos)
    {
        if((team == 0 && XPos == 7) || (team == 1 && XPos == 0))
        {
            Debug.Log("Promoting");
            int index = await MainGameUiManager.instance.promoteUI.ShowPromoteModalDialog();
            if (index == 0)
            {
                return ChessPieceType.Queen;
            }
            else if (index == 1)
            {
                return ChessPieceType.Rook;
            }
            else if (index == 2)
            {
                return ChessPieceType.Knight;
            }
            else
            {
                return ChessPieceType.Bishop;
            }
        }
        return ChessPieceType.None;
    }
}
