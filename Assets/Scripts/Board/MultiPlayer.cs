using System.Threading.Tasks;
using UnityEngine;

public class MultiPlayer : IGameMode
{

    public ChessBoard board;
    public void ChangeTurn()
    {
        Client.instance.SendToServer(new NetChangeTurn());
    }

    public void Exit()
    {
        NetRematch nr = new NetRematch();
        nr.teamId = GameManager.instance.assignedTeam;
        nr.wantRematch = 0;
        Client.instance.SendToServer(nr);
    }

    public async Task Move(ChessPiece selectedPiece, int x, int y, Vector2Int prevpos)
    {
        board.ClientMakeMove(x, y, prevpos);
        await Promote(selectedPiece, x, y);
        ChangeTurn();
    }

    public async Task Promote(ChessPiece selectedPiece, int x, int y)
    {
        await board.ClientPromote(selectedPiece, x, y);
    }

    public void Rematch()
    {
        NetRematch nr = new NetRematch();
        nr.teamId = GameManager.instance.assignedTeam;
        nr.wantRematch = 1;
        Client.instance.SendToServer(nr);
    }

    public void SetBoard(ChessBoard board)
    {
        this.board = board;
    }
}
