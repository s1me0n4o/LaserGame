using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossibleMovesManager : MonoBehaviour
{
    public static PossibleMovesManager instance;

    public GameObject highlitePrefab;
    public List<GameObject> highlightedObjects;

    private float offsetPivotPoint = 0.5f;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Only one PossibleMovesManager is awolled");
            return;
        }
        instance = this;
    }

    private void Start()
    {

        highlightedObjects = new List<GameObject>();
    }

    public void HighlightPossibleMoves(bool[,] moves)
    {
        var rows = BoardManager.instance.GetRows();
        var cols = BoardManager.instance.GetCols();
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (moves[i,j])
                {
                    GameObject highlitedObject = GetHighlitedObject();
                    highlitedObject.SetActive(true);
                    highlitedObject.transform.position = new Vector3(i + offsetPivotPoint, 0.06f, j + offsetPivotPoint);
                }
            }
        }
    }

    public void HideHighlights()
    {
        foreach (var item in highlightedObjects)
        {
            item.SetActive(false);
        }
    }


    private GameObject GetHighlitedObject()
    {
        GameObject highlitedObject = highlightedObjects.Find(g => !g.activeSelf); // will search trough the list to find the FIRST one that is not activeSelf 
        if (highlitedObject == null)
        {
            highlitedObject = Instantiate(highlitePrefab);
            highlightedObjects.Add(highlitedObject);
        }

        return highlitedObject;
    }


}
