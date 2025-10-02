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
    private Vector3 cubeCenter;
    private float chessPieceYOffset;
    private ChessPiece[,] listChessPiece;

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
        cubeCenter = new Vector3((float)tileSize / 2, 0, (float)tileSize / 2);
        chessPieceYOffset = 0.5f;
        listChessPiece = new ChessPiece[row, col];
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
            if (currentPointer==noTarget)
            {
                currentPointer = tileIndex;
                listTile[currentPointer.x, currentPointer.y].layer = hoverLayer;
            }
            else if (currentPointer!=noTarget && currentPointer!=tileIndex) 
            {
                listTile[currentPointer.x, currentPointer.y].layer = tileLayer;
                currentPointer = tileIndex;
                listTile[currentPointer.x, currentPointer.y].layer = hoverLayer;
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

    private void ConfigureChessPiece(ChessPieceType type,int x,int y,int team)
    {
        Vector3 piecePos = new Vector3(x, chessPieceYOffset, y) - bounds - cubeCenter;
        GameObject chessPiece = Instantiate(GameManager.instance.listChessPiecePrefab[(int)type], piecePos, Quaternion.identity, chessPieceGroup.transform);
        chessPiece.name = GameManager.instance.listChessPiecePrefab[(int)type].ToString();
        Transform chessPieceMesh = chessPiece.transform.Find("mesh");
        chessPieceMesh.GetComponent<MeshRenderer>().material = GameManager.instance.teamMaterial[team];
        listChessPiece[x, y] = chessPiece.GetComponent<ChessPiece>();
    }

    private void GenerateAllChessPiece()
    {
        //white
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

        //black
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
