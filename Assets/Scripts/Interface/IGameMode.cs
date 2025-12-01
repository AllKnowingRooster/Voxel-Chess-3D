using System.Threading.Tasks;
using UnityEngine;

public interface IGameMode
{
    public Task Move(ChessPiece selectedPiece, int x, int y, Vector2Int prevpos);
    public Task Promote(ChessPiece selectedPiece, int x, int y);
    void ChangeTurn();
    void Rematch();
    void Exit();
    void SetBoard(ChessBoard board);
}
