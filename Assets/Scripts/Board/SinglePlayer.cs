using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SinglePlayer : IGameMode
{

    public ChessBoard board;
    public void ChangeTurn()
    {
        board.ChangeTurn();
    }

    public void Exit()
    {
        SceneManager.LoadScene(0);
    }

    public async Task Move(ChessPiece selectedPiece, int x, int y, Vector2Int prevpos)
    {
        board.MovePiece(x, y, prevpos);
        await Promote(selectedPiece, x, y);
        ChangeTurn();
    }

    public async Task Promote(ChessPiece selectedPiece, int x, int y)
    {
        if (selectedPiece.type == ChessPieceType.Pawn)
        {
            Pawn pawn = selectedPiece.GetComponent<Pawn>();
            ChessPieceType pieceType = await pawn.Promote(x, y);
        }
    }

    public void Rematch()
    {
        SceneManager.LoadScene(1);
    }

    public void SetBoard(ChessBoard board)
    {
        this.board = board;
    }
}
