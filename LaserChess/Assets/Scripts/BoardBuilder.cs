using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardBuilder : MonoBehaviour
{
    [Header("Board")]
    public int rows = 8;
    public int cols = 8;
    public GameObject nodePrefab;
    public float nodePefabSize = 1.1f;
    
    private float _waitTime = .03f;

    [Header("Pieces Prefabs")]
    public List<GameObject> piecesPrefabs;
    public List<GameObject> piecesAlive;
    public static BasePiece[,] BasePieces { get; set; }
    
    [Header("Nodes")]
    private List<float[]> _nodesPosition = new List<float[]>();
    
    public static bool isBuildFinishied = false;
    public static bool isPlayerTurn = true;

    private void Start()
    {
        StartCoroutine(CreateBoard());
    }

    private IEnumerator CreateBoard()
    {
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                var position = new Vector3(r * nodePefabSize, 0, c * nodePefabSize);
                var node = Instantiate(nodePrefab, position, Quaternion.identity) as GameObject;
                node.transform.SetParent(transform);
                _nodesPosition.Add(new float[] { node.transform.position.x, node.transform.position.z });

                print($"x: {node.transform.position.x} ; z: {node.transform.position.z}");
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
        //Tank
        SpownSinglePiece(4, 0, 0);
        SpownSinglePiece(3, 0, 0);
        //JumpShip
        SpownSinglePiece(1, 0, 1);
        SpownSinglePiece(5, 0, 1);
        SpownSinglePiece(2, 0, 1);
        SpownSinglePiece(6, 0, 1);
        //Grunt
        for (int i = 0; i < rows; i++)
        {
            if (i == 0 || i == 7)
            {
                SpownSinglePiece(i, 0, 2);
            }
            SpownSinglePiece(i, 1, 2);
        }
    }

    public Vector3 GetNodeCenter(int x, int z)
    {
        var origin = Vector3.zero;
        origin.x = (nodePefabSize * x);
        origin.z = (nodePefabSize * z);
        return origin;
    }

    private void SpownSinglePiece(int x, int y, int unityIndex)
    {
        var gameObj = Instantiate(piecesPrefabs[unityIndex], GetNodeCenter(x, y), Quaternion.identity) as GameObject;
        BasePieces [x, y] = gameObj.GetComponent<BasePiece>();
        BasePieces[x, y].SetPosition(x, y);
        piecesAlive.Add(gameObj);
    }
}
