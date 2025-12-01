using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ChessBoard : MonoBehaviour
{
    private Camera currentCamera;
    private LayerMask tileLayer;
    private LayerMask hoverLayer;
    private LayerMask moveLayer;
    private LayerMask killLayer;
    private Vector2Int currentPointer;
    private Vector2Int noTarget;
    private GameObject[,] listTile;
    private int col;
    private int row;
    private GameObject tileGroup;
    private GameObject borderGroup;
    private GameObject chessPieceGroup;
    private Vector3 centerBoard;
    private Vector3 bounds;
    private int tileSize;
    private Vector3 cubeCenterX;
    private Vector3 cubeCenterZ;
    private float chessPieceYOffset;
    private ChessPiece[,] listChessPiece;
    private Quaternion whiteRotation;
    private Quaternion blackRotation;
    private List<ChessPiece> whiteDeads;
    private List<ChessPiece> blackDeads;
    private List<Vector2Int> listMove;
    private List<Vector2Int> listKillable;
    private ChessPiece selectedPiece;
    public static int whoTurn;
    private King blackKing;
    private King whiteKing;
    private bool hasPlayerMove;
    public static int turnCount;
    private bool[] playerWantRematch;
    private void Awake()
    {
        whoTurn = 0;
        col = 8;
        row = 8;
        tileLayer = LayerMask.NameToLayer("Tile");
        hoverLayer = LayerMask.NameToLayer("Hover");
        moveLayer = LayerMask.NameToLayer("Move");
        killLayer = LayerMask.NameToLayer("Kill");
        currentCamera = Camera.main;
        listTile = new GameObject[row, col];
        noTarget = Vector2Int.one * -1;
        currentPointer = noTarget;
        tileGroup = CreateObject("Tile");
        borderGroup = CreateObject("Border");
        chessPieceGroup = CreateObject("Chess Piece");
        centerBoard = Vector3.zero;
        bounds = new Vector3(row / 2, 0, col / 2) + centerBoard;
        tileSize = 1;
        cubeCenterZ = new Vector3(0, 0, (float)tileSize / 2);
        cubeCenterX = new Vector3((float)tileSize / 2, 0, 0);
        chessPieceYOffset = 0.5f;
        listChessPiece = new ChessPiece[row, col];
        blackRotation = Quaternion.Euler(0, -90, 0);
        whiteRotation = Quaternion.Euler(0, 90, 0);
        whiteDeads = new List<ChessPiece>();
        blackDeads = new List<ChessPiece>();
        listMove = new List<Vector2Int>();
        listKillable = new List<Vector2Int>();
        turnCount = 1;
        hasPlayerMove = false;
        playerWantRematch = new bool[2];
        GameManager.instance.gameMode.SetBoard(this);
        Debug.Log(GameManager.instance.gameMode);
        GenerateBoard();
        RegisterEvent();
    }


    private async void Update()
    {
        if (!hasPlayerMove)
        {
            RaycastHit hit;
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Ray cameraRay = currentCamera.ScreenPointToRay(mousePos);
            if (Physics.Raycast(cameraRay, out hit))
            {
                Vector2Int tileIndex = FindTile(hit.collider.gameObject);
                if (currentPointer == noTarget && tileIndex != noTarget)
                {
                    currentPointer = tileIndex;
                    listTile[currentPointer.x, currentPointer.y].layer = hoverLayer;
                }
                else if (currentPointer != noTarget && currentPointer != tileIndex && tileIndex != noTarget)
                {
                    listTile[currentPointer.x, currentPointer.y].layer = ChangeLayer(currentPointer.x, currentPointer.y);
                    currentPointer = tileIndex;
                    listTile[currentPointer.x, currentPointer.y].layer = hoverLayer;
                }
                else if (tileIndex == noTarget && currentPointer != noTarget)
                {
                    listTile[currentPointer.x, currentPointer.y].layer = ChangeLayer(currentPointer.x, currentPointer.y);
                    currentPointer = noTarget;
                }

                if (selectedPiece == null && currentPointer != noTarget && listChessPiece[tileIndex.x, tileIndex.y] != null && Mouse.current.leftButton.wasPressedThisFrame)
                {
                    if (whoTurn == listChessPiece[tileIndex.x, tileIndex.y].team && (GameManager.instance.gameMode is SinglePlayer || (GameManager.instance.gameMode is MultiPlayer && GameManager.instance.assignedTeam == whoTurn)))
                    {
                        selectedPiece = listChessPiece[tileIndex.x, tileIndex.y];
                        listKillable = selectedPiece.GetAllPossibleAttack(ref listChessPiece);
                        listMove = selectedPiece.GetAllPossibleMoves(ref listChessPiece);
                        preMoveSimulation(selectedPiece.team == 0 ? whiteKing : blackKing, selectedPiece, ref listMove);
                        preMoveSimulation(selectedPiece.team == 0 ? whiteKing : blackKing, selectedPiece, ref listKillable);
                        ShowHighlight();

                    }
                }
            }
            else
            {
                if (currentPointer != noTarget)
                {
                    listTile[currentPointer.x, currentPointer.y].layer = tileLayer;
                    currentPointer = noTarget;
                }
            }

            if (selectedPiece != null)
            {
                Plane plane = new Plane(Vector3.up, Vector3.up);
                float distance = 0.0f;
                if (plane.Raycast(cameraRay, out distance))
                {
                    selectedPiece.SetPosition(cameraRay.GetPoint(distance), false);
                }

                if (Mouse.current.leftButton.wasReleasedThisFrame)
                {
                    Vector2Int prevPos = new Vector2Int(selectedPiece.XPos, selectedPiece.YPos);
                    bool isValidMove = IsValidMove(currentPointer.x, currentPointer.y);
                    if (!isValidMove)
                    {
                        ResetPiece(prevPos);
                    }
                    else
                    {
                        await GameManager.instance.gameMode.Move(selectedPiece, currentPointer.x, currentPointer.y, prevPos);
                    }
                }
            }
        }
    }

    private void ResetPiece(Vector2Int prevPos)
    {
        selectedPiece.SetPosition(calculatePiecePosition(prevPos.x, prevPos.y, selectedPiece.team), false);
        selectedPiece = null;
        HideHighlight();
    }

    async public Task ClientPromote(ChessPiece selectedPiece, int x, int y)
    {
        if (selectedPiece.type == ChessPieceType.Pawn)
        {
            Pawn pawn = selectedPiece.GetComponent<Pawn>();
            ChessPieceType pieceType = await pawn.Promote(x, y);

            if (pieceType != ChessPieceType.None)
            {
                NetPromote np = new NetPromote();
                np.x = pawn.XPos;
                np.y = pawn.YPos;
                np.pieceType = pieceType;
                Client.instance.SendToServer(np);
            }

        }
    }

    public void ChangeTurn()
    {
        if (CheckCheckmate(selectedPiece.team == 0 ? blackKing : whiteKing, selectedPiece, ref listChessPiece))
        {
            return;
        }
        selectedPiece = null;
        turnCount++;
        whoTurn = whoTurn == 0 ? 1 : 0;
        hasPlayerMove = false;
    }

    private bool IsValidMove(int x, int y)
    {
        if ((!listMove.Contains(new Vector2Int(x, y)) && !listKillable.Contains(new Vector2Int(x, y))) || currentPointer == noTarget || (listChessPiece[x, y] != null && listChessPiece[x, y].team == selectedPiece.team))
        {
            return false;
        }


        return true;
    }

    private void RemovePiece(ChessPiece piece)
    {
        if (piece.team == 0)
        {
            piece.SetPosition(calculatePiecePosition(-1 - (whiteDeads.Count / (row + 2)), 8 - (whiteDeads.Count % (row + 2)), piece.team), false);
            whiteDeads.Add(piece);
        }
        else
        {
            piece.SetPosition(calculatePiecePosition(8 + (blackDeads.Count / (row + 2)), -1 + (blackDeads.Count % (row + 2)), piece.team), false);
            blackDeads.Add(piece);
        }
    }

    public bool MovePiece(int x, int y, Vector2Int prevPos)
    {

        ChessPiece overlapPiece = listChessPiece[x, y];
        if (overlapPiece != null)
        {
            if (overlapPiece.team == selectedPiece.team)
            {
                return false;
            }
            RemovePiece(overlapPiece);
        }
        else if (overlapPiece == null && selectedPiece.type == ChessPieceType.Pawn && listKillable.Contains(new Vector2Int(x, y)))
        {
            int direction = selectedPiece.team == 0 ? -1 : 1;
            overlapPiece = listChessPiece[x + direction, y];
            RemovePiece(overlapPiece);
        }


        hasPlayerMove = true;
        if (selectedPiece.type == ChessPieceType.King)
        {
            King king = selectedPiece.GetComponent<King>();
            int castlePos = y - prevPos.y;
            Rook goingToMoveRook = null;
            if (castlePos == 2)
            {
                goingToMoveRook = king.queenSideRook;
            }
            else if (castlePos == -2)
            {
                goingToMoveRook = king.kingSideRook;
            }

            if (goingToMoveRook != null)
            {
                goingToMoveRook.haveMoved = true;
                listChessPiece[goingToMoveRook.XPos, goingToMoveRook.YPos] = null;
                if (castlePos == 2)
                {
                    goingToMoveRook.YPos = y - 1;
                }
                else
                {
                    goingToMoveRook.YPos = y + 1;
                }
                goingToMoveRook.SetPosition(calculatePiecePosition(goingToMoveRook.XPos, goingToMoveRook.YPos, goingToMoveRook.team), false);
                listChessPiece[goingToMoveRook.XPos, goingToMoveRook.YPos] = goingToMoveRook;

            }

            if (!king.haveMoved)
            {
                king.haveMoved = true;
            }
        }
        else if (selectedPiece.type == ChessPieceType.Pawn)
        {
            Pawn pawn = selectedPiece.GetComponent<Pawn>();
            pawn.prevTurn = turnCount;
            pawn.prevPosition = prevPos;
        }
        else if (selectedPiece.type == ChessPieceType.Rook)
        {
            Rook rook = selectedPiece.GetComponent<Rook>();
            if (!rook.haveMoved)
            {
                rook.haveMoved = true;
            }
        }

        selectedPiece.SetPosition(calculatePiecePosition(x, y, selectedPiece.team), false);
        selectedPiece.XPos = x;
        selectedPiece.YPos = y;

        listChessPiece[prevPos.x, prevPos.y] = null;
        listChessPiece[x, y] = selectedPiece;
        HideHighlight();
        return true;
    }

    private GameObject CreateObject(string name)
    {
        GameObject obj = new GameObject(name);
        obj.transform.parent = transform;
        return obj;
    }

    private void GenerateBoard()
    {
        GenerateBorder();
        GenerateTile();
        GenerateAllChessPiece();
        TurnOnPlayerCamera();
    }

    private void GenerateTile()
    {
        for (int x = 0; x < row; x++)
        {
            for (int y = 0; y < col; y++)
            {
                if (x % 2 == y % 2)
                {
                    listTile[x, y] = ConfigTile(x, y, GameManager.instance.blackTilePrefab);
                }
                else
                {
                    listTile[x, y] = ConfigTile(x, y, GameManager.instance.blueTilePrefab);
                }
            }
        }
    }

    private GameObject ConfigTile(int x, int y, GameObject tilePrefab)
    {
        Vector3 position = new Vector3(x, 0, y) - bounds;
        GameObject tileGameObject = Instantiate(tilePrefab, position, Quaternion.identity, tileGroup.transform);
        tileGameObject.name = string.Format("X:{0} Y:{1}", x, y);
        tileGameObject.layer = tileLayer;
        return tileGameObject;
    }

    private void GenerateBorder()
    {
        for (int x = -1; x < row; x++)
        {
            Vector3 positionLeft = new Vector3(-1, 0, x) - bounds;
            Vector3 positionRight = new Vector3(8, 0, x + 1) - bounds;
            Vector3 positionTop = new Vector3(x, 0, 8) - bounds;
            Vector3 positionBottom = new Vector3(x + 1, 0, -1) - bounds;
            Instantiate(GameManager.instance.borderTilePrefab, positionLeft, Quaternion.identity, borderGroup.transform).name = GameManager.instance.borderTilePrefab.ToString();
            Instantiate(GameManager.instance.borderTilePrefab, positionRight, Quaternion.identity, borderGroup.transform).name = GameManager.instance.borderTilePrefab.ToString();
            Instantiate(GameManager.instance.borderTilePrefab, positionTop, Quaternion.identity, borderGroup.transform).name = GameManager.instance.borderTilePrefab.ToString();
            Instantiate(GameManager.instance.borderTilePrefab, positionBottom, Quaternion.identity, borderGroup.transform).name = GameManager.instance.borderTilePrefab.ToString();
        }
    }

    private Vector2Int FindTile(GameObject selectedTile)
    {
        for (int x = 0; x < row; x++)
        {
            for (int y = 0; y < col; y++)
            {
                if (listTile[x, y] == selectedTile)
                {
                    return new Vector2Int(x, y);
                }
            }
        }
        return noTarget;
    }

    private Vector3 calculatePiecePosition(int x, int y, int team)
    {
        Vector3 piecePos = new Vector3(x, chessPieceYOffset, y) - bounds;
        if (team == 0)
        {
            piecePos -= cubeCenterX;
            piecePos += cubeCenterZ;
        }
        else
        {
            piecePos += cubeCenterX;
            piecePos -= cubeCenterZ;
        }
        return piecePos;
    }

    private ChessPiece ConfigureChessPiece(ChessPieceType type, int x, int y, int team)
    {
        GameObject chessPiece = Instantiate(GameManager.instance.listChessPiecePrefab[(int)type], Vector3.zero, team == 0 ? whiteRotation : blackRotation, chessPieceGroup.transform);
        chessPiece.name = GameManager.instance.listChessPiecePrefab[(int)type].ToString();
        Transform chessPieceMesh = chessPiece.transform.Find("mesh");
        chessPieceMesh.GetComponent<MeshRenderer>().material = GameManager.instance.teamMaterial[team];
        listChessPiece[x, y] = chessPiece.GetComponent<ChessPiece>();
        listChessPiece[x, y].team = team;
        listChessPiece[x, y].XPos = x;
        listChessPiece[x, y].YPos = y;
        listChessPiece[x, y].type = type;
        listChessPiece[x, y].SetPosition(calculatePiecePosition(x, y, team), true);
        return listChessPiece[x, y];
    }

    private void GenerateAllChessPiece()
    {
        whiteKing = ConfigureChessPiece(ChessPieceType.King, 0, 3, 0).GetComponent<King>();

        whiteKing.kingSideRook = ConfigureChessPiece(ChessPieceType.Rook, 0, 0, 0).GetComponent<Rook>();
        ConfigureChessPiece(ChessPieceType.Knight, 0, 1, 0);
        ConfigureChessPiece(ChessPieceType.Bishop, 0, 2, 0);
        ConfigureChessPiece(ChessPieceType.Queen, 0, 4, 0);
        ConfigureChessPiece(ChessPieceType.Bishop, 0, 5, 0);
        ConfigureChessPiece(ChessPieceType.Knight, 0, 6, 0);
        whiteKing.queenSideRook = ConfigureChessPiece(ChessPieceType.Rook, 0, 7, 0).GetComponent<Rook>();
        for (int i = 0; i < 8; i++)
        {
            ConfigureChessPiece(ChessPieceType.Pawn, 1, i, 0);
        }

        blackKing = ConfigureChessPiece(ChessPieceType.King, 7, 3, 1).GetComponent<King>();

        blackKing.queenSideRook = ConfigureChessPiece(ChessPieceType.Rook, 7, 7, 1).GetComponent<Rook>();
        ConfigureChessPiece(ChessPieceType.Knight, 7, 6, 1);
        ConfigureChessPiece(ChessPieceType.Bishop, 7, 5, 1);
        ConfigureChessPiece(ChessPieceType.Queen, 7, 4, 1);
        ConfigureChessPiece(ChessPieceType.Bishop, 7, 2, 1);
        ConfigureChessPiece(ChessPieceType.Knight, 7, 1, 1);
        blackKing.kingSideRook = ConfigureChessPiece(ChessPieceType.Rook, 7, 0, 1).GetComponent<Rook>();
        for (int i = 0; i < 8; i++)
        {
            ConfigureChessPiece(ChessPieceType.Pawn, 6, i, 1);
        }

    }

    private void ShowHighlight()
    {
        for (int i = 0; i < listMove.Count; i++)
        {
            listTile[listMove[i].x, listMove[i].y].layer = moveLayer;
        }

        for (int i = 0; i < listKillable.Count; i++)
        {
            if (!listMove.Contains(listKillable[i]))
            {
                listTile[listKillable[i].x, listKillable[i].y].layer = killLayer;
            }
        }


    }

    private void HideHighlight()
    {
        for (int i = 0; i < listMove.Count; i++)
        {
            listTile[listMove[i].x, listMove[i].y].layer = tileLayer;
        }

        for (int i = 0; i < listKillable.Count; i++)
        {
            listTile[listKillable[i].x, listKillable[i].y].layer = tileLayer;
        }
        listMove.Clear();
        listKillable.Clear();
    }

    private LayerMask ChangeLayer(int x, int y)
    {
        Vector2Int pos = new Vector2Int(x, y);
        if (listMove.Contains(pos))
        {
            return moveLayer;
        }
        else if (listKillable.Contains(pos))
        {
            return killLayer;
        }
        else
        {
            return tileLayer;
        }
    }

    private void preMoveSimulation(King king, ChessPiece selectedPiece, ref List<Vector2Int> listMove)
    {
        int originalX = selectedPiece.XPos;
        int originalY = selectedPiece.YPos;
        List<Vector2Int> removedMoveList = new List<Vector2Int>();
        for (int i = 0; i < listMove.Count; i++)
        {
            ChessPiece[,] chessPieceSimulation = new ChessPiece[row, col];
            List<ChessPiece> enemyPiece = new List<ChessPiece>();
            for (int j = 0; j < row; j++)
            {
                for (int l = 0; l < col; l++)
                {
                    if (listChessPiece[j, l] != null)
                    {
                        chessPieceSimulation[j, l] = listChessPiece[j, l];
                        if (listChessPiece[j, l].team != selectedPiece.team)
                        {
                            enemyPiece.Add(listChessPiece[j, l]);
                        }
                    }
                }
            }
            int simX = listMove[i].x;
            int simY = listMove[i].y;
            if (chessPieceSimulation[simX, simY] != null)
            {
                if (chessPieceSimulation[simX, simY].team != selectedPiece.team)
                {
                    enemyPiece.Remove(chessPieceSimulation[simX, simY]);
                }
                else
                {
                    return;
                }
            }
            chessPieceSimulation[originalX, originalY] = null;
            chessPieceSimulation[simX, simY] = selectedPiece;
            selectedPiece.XPos = simX;
            selectedPiece.YPos = simY;

            List<Vector2Int> enemyListMove = new List<Vector2Int>();

            for (int p = 0; p < enemyPiece.Count; p++)
            {
                List<Vector2Int> enemyMove = enemyPiece[p].GetAllPossibleAttack(ref chessPieceSimulation);
                for (int o = 0; o < enemyMove.Count; o++)
                {
                    enemyListMove.Add(enemyMove[o]);
                }
            }

            if (selectedPiece.type == ChessPieceType.King && !king.haveMoved && !king.isChecked)
            {
                if (selectedPiece.team == 0)
                {
                    if (listMove.Contains(new Vector2Int(0, 1)) && enemyListMove.Contains(new Vector2Int(0, 2)))
                    {
                        listMove.Remove(new Vector2Int(0, 1));
                    }

                    if (listMove.Contains(new Vector2Int(0, 5)) && enemyListMove.Contains(new Vector2Int(0, 4)))
                    {
                        listMove.Remove(new Vector2Int(0, 5));
                    }
                }
                else
                {
                    if (listMove.Contains(new Vector2Int(7, 1)) && enemyListMove.Contains(new Vector2Int(7, 2)))
                    {
                        listMove.Remove(new Vector2Int(7, 1));
                    }

                    if (listMove.Contains(new Vector2Int(7, 5)) && enemyListMove.Contains(new Vector2Int(7, 4)))
                    {
                        listMove.Remove(new Vector2Int(7, 5));
                    }
                }
            }

            for (int u = 0; u < enemyListMove.Count; u++)
            {
                if (enemyListMove[u].x == king.XPos && enemyListMove[u].y == king.YPos)
                {
                    removedMoveList.Add(listMove[i]);
                    break;
                }
            }
            selectedPiece.XPos = originalX;
            selectedPiece.YPos = originalY;
        }

        for (int j = 0; j < removedMoveList.Count; j++)
        {
            listMove.Remove(removedMoveList[j]);
        }
    }

    public bool CheckCheckmate(King king, ChessPiece selectedPiece, ref ChessPiece[,] listChessPiece)
    {
        List<ChessPiece> listEnemy = new List<ChessPiece>();
        List<ChessPiece> listAlly = new List<ChessPiece>();
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (listChessPiece[i, j] != null)
                {
                    if (listChessPiece[i, j].team != selectedPiece.team)
                    {
                        listAlly.Add(listChessPiece[i, j]);
                    }
                    else
                    {
                        listEnemy.Add(listChessPiece[i, j]);
                    }
                }
            }
        }
        bool isChecked = false;
        List<Vector2Int> listEnemyAttack = new List<Vector2Int>();
        for (int i = 0; i < listEnemy.Count; i++)
        {
            List<Vector2Int> listAttack2 = listEnemy[i].GetAllPossibleAttack(ref listChessPiece);
            for (int j = 0; j < listAttack2.Count; j++)
            {
                listEnemyAttack.Add(listAttack2[j]);
                if (listAttack2[j].x == king.XPos && listAttack2[j].y == king.YPos && !isChecked)
                {
                    isChecked = true;
                    break;
                }
            }
        }
        king.isChecked = isChecked;
        if (!king.isChecked)
        {
            for (int i = 0; i < listAlly.Count; i++)
            {
                List<Vector2Int> listAllyMove = listAlly[i].GetAllPossibleMoves(ref listChessPiece);
                List<Vector2Int> listAllyAttack = listAlly[i].GetAllPossibleAttack(ref listChessPiece);
                preMoveSimulation(king, listAlly[i], ref listAllyMove);
                preMoveSimulation(king, listAlly[i], ref listAllyAttack);
                if (listAllyMove.Count != 0 || listAllyAttack.Count != 0)
                {
                    return false;
                }
            }
            GameManager.instance.SetWinner(2, WinReason.Stalemate);
        }
        else if (king.isChecked)
        {
            for (int i = 0; i < listAlly.Count; i++)
            {
                List<Vector2Int> listAllyMove = listAlly[i].GetAllPossibleMoves(ref listChessPiece);
                List<Vector2Int> listAllyAttack = listAlly[i].GetAllPossibleAttack(ref listChessPiece);
                preMoveSimulation(king, listAlly[i], ref listAllyMove);
                preMoveSimulation(king, listAlly[i], ref listAllyAttack);
                if (listAllyMove.Count != 0 || listAllyAttack.Count != 0)
                {
                    return false;
                }
            }
            GameManager.instance.SetWinner(selectedPiece.team, WinReason.Checkmate);
        }
        return true;
    }

    private void OnDestroy()
    {
        UnregisterEvent();
    }
    private void RegisterEvent()
    {
        NetUtility.C_MAKE_MOVE += OnMakeMoveClient;
        NetUtility.C_PROMOTE += OnPromoteClient;
        NetUtility.C_CHANGE_TURN += OnChangeTurnClient;
        NetUtility.C_REMATCH += OnRematchClient;
    }

    private void UnregisterEvent()
    {
        NetUtility.C_MAKE_MOVE -= OnMakeMoveClient;
        NetUtility.C_PROMOTE -= OnPromoteClient;
        NetUtility.C_CHANGE_TURN -= OnChangeTurnClient;
        NetUtility.C_REMATCH -= OnRematchClient;
    }

    private void OnChangeTurnClient(NetMessage msg)
    {
        ChangeTurn();
    }

    private void OnMakeMoveClient(NetMessage msg)
    {
        NetMakeMove nmm = msg as NetMakeMove;
        selectedPiece = listChessPiece[nmm.originalXPos, nmm.originalYPos];
        MovePiece(nmm.xPos, nmm.yPos, new Vector2Int(nmm.originalXPos, nmm.originalYPos));
    }

    public void TurnOnPlayerCamera()
    {
        Transform cameraParent = GameObject.Find("Cameras").transform;
        if (GameManager.instance.assignedTeam == 1)
        {
            cameraParent.Find("BlackCamera")?.gameObject.SetActive(true);
        }
        else
        {
            cameraParent.Find("WhiteCamera")?.gameObject.SetActive(true);
        }
    }

    public void ClientMakeMove(int x, int y, Vector2Int prevpos)
    {
        NetMakeMove nmm = new NetMakeMove();
        nmm.originalYPos = prevpos.y;
        nmm.originalXPos = prevpos.x;
        nmm.xPos = x;
        nmm.yPos = y;
        Client.instance.SendToServer(nmm);
    }


    private void OnPromoteClient(NetMessage msg)
    {
        NetPromote np = msg as NetPromote;
        int team = listChessPiece[np.x, np.y].team;
        Destroy(listChessPiece[np.x, np.y].gameObject);
        listChessPiece[np.x, np.y] = ConfigureChessPiece(np.pieceType, np.x, np.y, team);
    }

    private void OnRematchClient(NetMessage msg)
    {
        NetRematch np = msg as NetRematch;
        playerWantRematch[np.teamId] = np.wantRematch == 1;
        Debug.Log(np.wantRematch);
        if (np.teamId != GameManager.instance.assignedTeam)
        {
            if (np.wantRematch == 1)
            {
                MainGameUiManager.instance.resultUI.SetRematchText("Opponent Want A Rematch", Color.green);
            }
            else
            {
                MainGameUiManager.instance.resultUI.SetRematchText("Opponent Has Left", Color.red);
                MainGameUiManager.instance.resultUI.rematchButton.interactable = false;
            }
        }

        if (playerWantRematch[0] && playerWantRematch[1])
        {
            GameManager.instance.ResetVariable();
            SceneManager.LoadScene(1);
        }
    }

}
