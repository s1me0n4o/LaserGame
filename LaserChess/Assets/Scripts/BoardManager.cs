using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager instance;

    [Header("Board")]
    public GameObject nodePrefab;

    private int _rows = 8;
    private int _cols = 8;
    private const float _nodePefabSize = 1f;
    private const float _nodeOffset = 0.5f;
    private float _waitTime = .03f;

    [Header("Pieces Prefabs")]
    public List<GameObject> piecesPrefabs;

    private List<GameObject> piecesAlive;
    
    [Header("Nodes")]
    public List<float[]> _nodesPosition = new List<float[]>();

      
    public bool isBuildFinishied = false;
    public bool isPlayerTurn = true;

    public BasePiece[,] BasePieces { get; set; }
    public bool[,] allowedMoves { get; set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Only one BoardManager is awolled");
            return;
        }
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(CreateBoard());
    }

    public int GetRows()
    {
        return _rows;
    }
    public int GetCols()
    {
        return _cols;
    }

    public Vector3 GetCenterNode(int x, int y)
    {
        var origin = Vector3.zero;

        origin.x = (_nodePefabSize * x) + _nodeOffset;
        origin.z = (_nodePefabSize * y) + _nodeOffset;
        Debug.Log($"Ceneter Node: {origin}");
        return origin;
    }

    private IEnumerator CreateBoard()
    {
        for (int r = 0; r < _rows; r++)
        {
            for (int c = 0; c < _cols; c++)
            {
                var position = new Vector3(r * _nodePefabSize, 0, c * _nodePefabSize);
                var node = Instantiate(nodePrefab, position, Quaternion.identity) as GameObject;
                node.transform.SetParent(transform);
                _nodesPosition.Add(new float[] { node.transform.position.x, node.transform.position.z });

                yield return new WaitForSeconds(_waitTime);
            }
        }
        SpownAllPieces();
        isBuildFinishied = true;
    }


    private void SpownAllPieces()
    {
        piecesAlive = new List<GameObject>();
        BasePieces = new BasePiece[8, 8];
        //Spown Human Pieces
        for (int i = 0; i < _rows; i++)
        {
            //Tank
            if (i == 4 || i == 3)
            {
                    
                SpownSinglePiece(i, 0, 0);
            }
            //JumpShip
            if (i == 1 || i == 5 || i == 2 || i == 6)
            {
                SpownSinglePiece(i, 0, 0);
            }

            if (i == 1 || i == 5 || i == 2 || i == 6)
            {
                SpownSinglePiece(i, 0, 0);
            }
            //Grunt
            if (i == 0 || i == 7)
            {
                SpownSinglePiece(i, 0, 2);
            }
            SpownSinglePiece(i, 1, 2);
        }
        
    }

    private void SpownSinglePiece(int x, int y, int unityIndex)
    {
        var gameObj = Instantiate(piecesPrefabs[unityIndex], GetCenterNode(x, y), Quaternion.identity) as GameObject;
        gameObj.transform.SetParent(transform);
        BasePieces [x, y] = gameObj.GetComponent<BasePiece>();
        BasePieces[x, y].SetPosition(x, y);
        piecesAlive.Add(gameObj);
    }
}
