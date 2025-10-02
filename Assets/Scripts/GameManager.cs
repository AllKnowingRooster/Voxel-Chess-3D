using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    [Header("Chess Board Config")]
    private GameObject chessBoardGameObject;
    [SerializeField] public GameObject blueTilePrefab;
    [SerializeField] public GameObject blackTilePrefab;
    [SerializeField] public GameObject borderTilePrefab;

    [Header("Chess Piece Config")]
    public Material[] teamMaterial;
    public List<GameObject> listChessPiecePrefab;

    private void Awake()
    {
        if (instance!=null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        chessBoardGameObject = new GameObject("Chess Board");
        chessBoardGameObject.AddComponent<ChessBoard>();
    }


}
