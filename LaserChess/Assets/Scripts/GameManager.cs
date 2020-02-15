using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public delegate void TurnChanged(bool isPlayerTurn);
    public static event TurnChanged OnTurnChange;

    public bool isPlayerTurn;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Only one GameManager is awolled");
            return;
        }
        instance = this;
    }

    private void Start()
    {
        isPlayerTurn = true;
    }

    public void EndTurn()
    {
        if (!BoardManager.instance.isBuildFinishied)
        {
            return;
        }

        isPlayerTurn = !isPlayerTurn;

        if (OnTurnChange != null)
        {
            OnTurnChange(isPlayerTurn);
        }
    }
}
