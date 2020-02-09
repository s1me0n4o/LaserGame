using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossibleMovesManager : MonoBehaviour
{
    public static PossibleMovesManager instance;

    public GameObject highliteMovePrefab;
    public GameObject highliteAttackPrefab;
    public List<GameObject> highlightedMovingObjects;
    public List<GameObject> highlightedAttackingObjects;

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
        highlightedMovingObjects = new List<GameObject>();
        highlightedAttackingObjects = new List<GameObject>();
    }

    //public bool HasEnemiesToAttack()
    //{
    //    if (highlightedAttackingObjects.Count > 0)
    //        return true;
    //    else
    //        return false;
    //}

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
                    GameObject highlitedObject = GetHighlitedObject(1);
                    highlitedObject.SetActive(true);
                    highlitedObject.transform.position = new Vector3(i + offsetPivotPoint, 0.06f, j + offsetPivotPoint);
                }
            }
        }
    }

    public void HighlightPossibleAttack(bool[,] attacks)
    {
        var rows = BoardManager.instance.GetRows();
        var cols = BoardManager.instance.GetCols();
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (attacks[i, j])
                {
                    GameObject highlitedObject = GetHighlitedObject(2);
                    highlitedObject.SetActive(true);
                    highlitedObject.transform.position = new Vector3(i + offsetPivotPoint, 0.06f, j + offsetPivotPoint);
                }
            }
        }
    }

    public void HideHighlights()
    {
        foreach (var item in highlightedMovingObjects)
        {
            item.SetActive(false);
        }
        foreach (var item in highlightedAttackingObjects)
        {
            item.SetActive(false);
        }
    }


    private GameObject GetHighlitedObject(int moveOrAttack)
    {
        GameObject highlitedObject;
        if (moveOrAttack == 1)
        {
            highlitedObject = highlightedMovingObjects.Find(g => !g.activeSelf); // will search trough the list to find the FIRST one that is not activeSelf 
            if (highlitedObject == null)
            {
                highlitedObject = Instantiate(highliteMovePrefab);
                highlightedMovingObjects.Add(highlitedObject);
            }
        }
        else
        {
            highlitedObject = highlightedAttackingObjects.Find(g => !g.activeSelf); // will search trough the list to find the FIRST one that is not activeSelf 
            if (highlitedObject == null)
            {
                highlitedObject = Instantiate(highliteAttackPrefab);
                highlightedAttackingObjects.Add(highlitedObject);
            }
        }

        return highlitedObject;
    }
}
