using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class ChessBoard : MonoBehaviour
{
    private Camera currentCamera;
    private LayerMask tileLayer;
    private LayerMask hoverLayer;
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

    private ChessPiece selectedPiece;

    private void Awake()
    {
        col = 8;
        row = 8;
        tileLayer = LayerMask.NameToLayer("Tile");
        hoverLayer = LayerMask.NameToLayer("Hover");
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
                listTile[currentPointer.x, currentPointer.y].layer = tileLayer;
                currentPointer = tileIndex;
                listTile[currentPointer.x, currentPointer.y].layer = hoverLayer;
            }
            else if(tileIndex==noTarget && currentPointer!=noTarget)
            {
                listTile[currentPointer.x, currentPointer.y].layer = tileLayer;
                currentPointer = noTarget;
            }

            if (selectedPiece == null && currentPointer != noTarget && listChessPiece[tileIndex.x, tileIndex.y] != null && Mouse.current.leftButton.wasPressedThisFrame)
            {
                selectedPiece = listChessPiece[tileIndex.x, tileIndex.y];
            }

            if (selectedPiece != null && currentPointer != noTarget && Mouse.current.leftButton.wasPressedThisFrame)
            {
                Vector2Int prevPosition = new Vector2Int(selectedPiece.XPos, selectedPiece.YPos);
                if (listChessPiece[tileIndex.x, tileIndex.y] != null)
                {
                    ChessPiece attackedPiece = listChessPiece[tileIndex.x, tileIndex.y];
                    if (attackedPiece.team == selectedPiece.team)
                    {
                        return;
                    }


                    if (attackedPiece.team == 0)
                    {
                        attackedPiece.SetPosition(calculatePiecePosition(-1 - (whiteDeads.Count / (row + 2)), 8 - (whiteDeads.Count % (row + 2)), attackedPiece.team), false);
                        whiteDeads.Add(attackedPiece);
                    }
                    else
                    {
                        attackedPiece.SetPosition(calculatePiecePosition(8 + (blackDeads.Count / (row + 2)), -1 + (blackDeads.Count % (row + 2)), attackedPiece.team), false);
                        blackDeads.Add(attackedPiece);
                    }
                }
                selectedPiece.SetPosition(calculatePiecePosition(tileIndex.x, tileIndex.y, selectedPiece.team), false);
                selectedPiece.XPos = tileIndex.x;
                selectedPiece.YPos = tileIndex.y;
                listChessPiece[prevPosition.x, prevPosition.y] = null;
                listChessPiece[tileIndex.x, tileIndex.y] = selectedPiece;
                selectedPiece = null;
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
                Debug.Log("I dont miss");
            }
            else
            {
                Debug.Log("I miss");
            }

            if (currentPointer == noTarget && Mouse.current.leftButton.wasPressedThisFrame)
            {
                selectedPiece.SetPosition(calculatePiecePosition(selectedPiece.XPos, selectedPiece.YPos, selectedPiece.team), false);
                selectedPiece = null;
            }
        }

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

}
