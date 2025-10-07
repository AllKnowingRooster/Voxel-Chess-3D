using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

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
    private int whoTurn;
    private void Awake()
    {
        whoTurn = 0;
        col = 8;
        row = 8;
        tileLayer = LayerMask.NameToLayer("Tile");
        hoverLayer = LayerMask.NameToLayer("Hover");
        moveLayer = LayerMask.NameToLayer("Move");
        killLayer = LayerMask.NameToLayer("Kill");
        currentCamera=Camera.main;
        listTile = new GameObject[row, col];
        noTarget = Vector2Int.one * -1;
        currentPointer = noTarget;
        tileGroup = CreateObject("Tile");
        borderGroup = CreateObject("Border");
        chessPieceGroup = CreateObject("Chess Piece");
        centerBoard = Vector3.zero;
        bounds = new Vector3(row / 2, 0, col / 2)+centerBoard;
        tileSize = 1;
        cubeCenterZ = new Vector3(0,0,(float)tileSize/2);
        cubeCenterX = new Vector3((float)tileSize/2,0,0);
        chessPieceYOffset = 0.5f;
        listChessPiece = new ChessPiece[row, col];
        blackRotation = Quaternion.Euler(0,-90,0);
        whiteRotation = Quaternion.Euler(0, 90, 0);
        whiteDeads = new List<ChessPiece>();
        blackDeads = new List<ChessPiece>();
        listMove=new List<Vector2Int>();
        listKillable=new List<Vector2Int>();
        GenerateBoard();

    }

    private void Update()
    {
        RaycastHit hit;
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray cameraRay = currentCamera.ScreenPointToRay(mousePos);
        if (Physics.Raycast(cameraRay, out hit)) 
        {
            Vector2Int tileIndex = FindTile(hit.collider.gameObject);
            if (currentPointer==noTarget && tileIndex!=noTarget)
            {
                currentPointer = tileIndex;
                listTile[currentPointer.x, currentPointer.y].layer = hoverLayer;
            }
            else if (currentPointer!=noTarget && currentPointer!=tileIndex && tileIndex!=noTarget) 
            {
                listTile[currentPointer.x, currentPointer.y].layer = ChangeLayer(currentPointer.x, currentPointer.y);
                currentPointer = tileIndex;
                listTile[currentPointer.x, currentPointer.y].layer = hoverLayer;
            }
            else if(tileIndex==noTarget && currentPointer!=noTarget)
            {
                listTile[currentPointer.x, currentPointer.y].layer = ChangeLayer(currentPointer.x,currentPointer.y);
                currentPointer = noTarget;
            }

            if (selectedPiece == null && currentPointer != noTarget && listChessPiece[tileIndex.x, tileIndex.y] != null && Mouse.current.leftButton.wasPressedThisFrame)
            {
                if (whoTurn == listChessPiece[tileIndex.x,tileIndex.y].team)
                {
                    selectedPiece = listChessPiece[tileIndex.x, tileIndex.y];
                    ShowHighlight(selectedPiece);
                }
            }
        }
        else
        {
            if (currentPointer!=noTarget)
            {
                listTile[currentPointer.x, currentPointer.y].layer = tileLayer;
                currentPointer = noTarget;
            }
        }

        if (selectedPiece!=null)
        {
            Plane plane = new Plane(Vector3.up, Vector3.up);
            float distance = 0.0f;
            if (plane.Raycast(cameraRay,out distance))
            {
                selectedPiece.SetPosition(cameraRay.GetPoint(distance),false);
            }

            if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                Vector2Int prevPos = new Vector2Int(selectedPiece.XPos, selectedPiece.YPos);
                bool isValidMove = MovePiece(currentPointer.x, currentPointer.y, prevPos);
                if (!isValidMove)
                {
                    ResetPiece(prevPos);
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

    private bool MovePiece(int x,int y,Vector2Int prevPos)
    {
        if ((!listMove.Contains(new Vector2Int(x,y)) && !listKillable.Contains(new Vector2Int(x,y))) || currentPointer==noTarget)
        {
            return false;
        } 
        if (listChessPiece[x,y]!=null)
        {
            ChessPiece overlapPiece = listChessPiece[x, y];
            if (overlapPiece.team==selectedPiece.team)
            {
                return false;
            }


            if (overlapPiece.team == 0)
            {
                overlapPiece.SetPosition(calculatePiecePosition(-1 - (whiteDeads.Count / (row + 2)), 8 - (whiteDeads.Count % (row + 2)), overlapPiece.team), false);
                whiteDeads.Add(overlapPiece);
            }
            else
            {
                overlapPiece.SetPosition(calculatePiecePosition(8 + (blackDeads.Count / (row + 2)), -1 + (blackDeads.Count % (row + 2)), overlapPiece.team), false);
                blackDeads.Add(overlapPiece);
            }
        }else if (listChessPiece[x,y]==null && selectedPiece.type==ChessPieceType.Pawn && listKillable.Contains(new Vector2Int(x,y)))
        {
            int direction = selectedPiece.team == 0 ? -1 : 1;
            ChessPiece overlapPiece = listChessPiece[x+direction , y];

            if (overlapPiece.team == 0)
            {
                overlapPiece.SetPosition(calculatePiecePosition(-1 - (whiteDeads.Count / (row + 2)), 8 - (whiteDeads.Count % (row + 2)), overlapPiece.team), false);
                whiteDeads.Add(overlapPiece);
            }
            else
            {
                overlapPiece.SetPosition(calculatePiecePosition(8 + (blackDeads.Count / (row + 2)), -1 + (blackDeads.Count % (row + 2)), overlapPiece.team), false);
                blackDeads.Add(overlapPiece);
            }
        }
        selectedPiece.SetPosition(calculatePiecePosition(currentPointer.x, currentPointer.y, selectedPiece.team), false);
        selectedPiece.XPos = currentPointer.x;
        selectedPiece.YPos = currentPointer.y;
        listChessPiece[prevPos.x, prevPos.y] = null;
        listChessPiece[currentPointer.x, currentPointer.y] = selectedPiece;
        Pawn pawn=selectedPiece.GetComponent<Pawn>();
        if (pawn!=null)
        {
            pawn.totalMove++;
            pawn.prevPosition = prevPos;
        }
        selectedPiece = null;
        HideHighlight();
        whoTurn = whoTurn == 0 ? 1 : 0;
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

    private GameObject ConfigTile(int x,int y,GameObject tilePrefab)
    {
        Vector3 position = new Vector3(x, 0, y)-bounds;
        GameObject tileGameObject = Instantiate(tilePrefab, position, Quaternion.identity, tileGroup.transform);
        tileGameObject.name = string.Format("X:{0} Y:{1}", x, y);
        tileGameObject.layer = tileLayer;
        return tileGameObject;
    }
    
    private void GenerateBorder()
    {
        for (int x=-1;x<row;x++)
        {
            Vector3 positionLeft = new Vector3(-1,0,x) - bounds;
            Vector3 positionRight = new Vector3(8, 0, x+1) -bounds;
            Vector3 positionTop = new Vector3(x, 0, 8) - bounds;
            Vector3 positionBottom = new Vector3(x+1, 0, -1) - bounds;
            Instantiate(GameManager.instance.borderTilePrefab, positionLeft,Quaternion.identity,borderGroup.transform).name = GameManager.instance.borderTilePrefab.ToString();
            Instantiate(GameManager.instance.borderTilePrefab, positionRight, Quaternion.identity, borderGroup.transform).name = GameManager.instance.borderTilePrefab.ToString();
            Instantiate(GameManager.instance.borderTilePrefab, positionTop, Quaternion.identity, borderGroup.transform).name = GameManager.instance.borderTilePrefab.ToString();
            Instantiate(GameManager.instance.borderTilePrefab, positionBottom, Quaternion.identity, borderGroup.transform).name = GameManager.instance.borderTilePrefab.ToString();
        }
    }

    private Vector2Int FindTile(GameObject selectedTile)
    {
        for (int x=0;x<row;x++)
        {
            for (int y=0;y<col;y++)
            {
                if (listTile[x,y]==selectedTile)
                {
                    return new Vector2Int(x,y);
                }
            }
        }
        return noTarget;
    }

    private Vector3 calculatePiecePosition(int x,int y,int team)
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

    private void ConfigureChessPiece(ChessPieceType type,int x,int y,int team)
    {
        GameObject chessPiece = Instantiate(GameManager.instance.listChessPiecePrefab[(int)type],Vector3.zero, team ==0?whiteRotation:blackRotation, chessPieceGroup.transform);
        chessPiece.name = GameManager.instance.listChessPiecePrefab[(int)type].ToString();
        Transform chessPieceMesh = chessPiece.transform.Find("mesh");
        chessPieceMesh.GetComponent<MeshRenderer>().material = GameManager.instance.teamMaterial[team];
        listChessPiece[x,y] = chessPiece.GetComponent<ChessPiece>();
        listChessPiece[x,y].team = team;
        listChessPiece[x,y].XPos = x;
        listChessPiece[x,y].YPos = y;
        listChessPiece[x,y].SetPosition(calculatePiecePosition(x,y,team),true);
    }

    private void GenerateAllChessPiece()
    {

        ConfigureChessPiece(ChessPieceType.Rook, 0, 0, 0);
        ConfigureChessPiece(ChessPieceType.Knight, 0, 1, 0);
        ConfigureChessPiece(ChessPieceType.Bishop, 0, 2, 0);
        ConfigureChessPiece(ChessPieceType.Queen, 0, 3, 0);
        ConfigureChessPiece(ChessPieceType.King, 0, 4, 0);
        ConfigureChessPiece(ChessPieceType.Bishop, 0, 5, 0);
        ConfigureChessPiece(ChessPieceType.Knight, 0, 6, 0);
        ConfigureChessPiece(ChessPieceType.Rook, 0, 7, 0);
        for (int i=0;i<8;i++)
        {
            ConfigureChessPiece(ChessPieceType.Pawn, 1, i, 0);
        }


        ConfigureChessPiece(ChessPieceType.Rook, 7, 7, 1);
        ConfigureChessPiece(ChessPieceType.Knight, 7, 6, 1);
        ConfigureChessPiece(ChessPieceType.Bishop, 7, 5, 1);
        ConfigureChessPiece(ChessPieceType.Queen, 7, 4, 1);
        ConfigureChessPiece(ChessPieceType.King, 7, 3, 1);
        ConfigureChessPiece(ChessPieceType.Bishop, 7, 2, 1);
        ConfigureChessPiece(ChessPieceType.Knight, 7, 1, 1);
        ConfigureChessPiece(ChessPieceType.Rook, 7, 0, 1);
        for (int i = 0; i < 8; i++)
        {
            ConfigureChessPiece(ChessPieceType.Pawn, 6, i, 1);
        }

    }

    private void ShowHighlight(ChessPiece piece)
    {
        (listMove, listKillable) = piece.GetAllPossibleMoves(ref listChessPiece);
        for (int i=0;i<listMove.Count;i++)
        {
            listTile[listMove[i].x, listMove[i].y].layer = moveLayer;
        }

        for (int i = 0; i < listKillable.Count; i++)
        {
            listTile[listKillable[i].x, listKillable[i].y].layer = killLayer;
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

    private LayerMask ChangeLayer(int x,int y)
    {
        Vector2Int pos= new Vector2Int(x,y);
        if (listMove.Contains(pos))
        {
            return moveLayer;
        }
        else if(listKillable.Contains(pos))
        {
            return killLayer;
        }
        else
        {
            return tileLayer;
        }
    }

}
