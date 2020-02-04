using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardBuilder : MonoBehaviour
{
    [Header("Board")]
    public int rows = 8;
    public int cols = 8;
    public GameObject boardPrefab;
    public float nodePefabSize = 1.1f;
    
    private float waitTime = .03f;
    private float nodeOffset = .5f;

    [Header("Pieces Prefabs")]
    public List<GameObject> piecesPrefabs;

    private List<float[]> nodesPosition = new List<float[]>();

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
                var node = Instantiate(boardPrefab, position, Quaternion.identity) as GameObject;
                node.transform.SetParent(transform);
                nodesPosition.Add(new float[] { node.transform.position.x, node.transform.position.z });

                print($"x: {node.transform.position.x} ; z: {node.transform.position.z}");
                yield return new WaitForSeconds(waitTime);
            }
        }
        SpownAllPieces();
    }

    private void SpownAllPieces()
    {
    //Spown Human Pieces
        //Tank
        SpownSinglePiece(GetNodeCenter(4, 0), 0);
        //JumpShip
        SpownSinglePiece(GetNodeCenter(1, 0), 1);
        SpownSinglePiece(GetNodeCenter(6, 0), 1);
        //Grunt
        for (int i = 0; i < rows; i++)
        {
            SpownSinglePiece(GetNodeCenter(i, 1), 2);
        }
    }

    private Vector3 GetNodeCenter(int x, int z)
    {
        var origin = Vector3.zero;
        origin.x = (nodePefabSize * x);
        origin.z = (nodePefabSize * z);
        return origin;
    }

    private void SpownSinglePiece(Vector3 position, int unityIndex)
    {
        var gameObj = Instantiate(piecesPrefabs[unityIndex], position, Quaternion.identity) as GameObject;
    }
}
