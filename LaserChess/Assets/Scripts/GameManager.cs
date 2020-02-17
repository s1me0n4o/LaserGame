using Assets.Scripts.External;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public delegate void TurnChanged(bool isPlayerTurn);
    public static event TurnChanged OnTurnChange;

    public bool isPlayerTurn;
    public bool isGameOver;

    private int _rows;
    private int _cols;
    private string _loadGameOverScene = "GameOver";
    private string _loadWinScene = "Win";

//TODO separate the loading once you have time
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
        isGameOver = false;
        _rows = BoardManager.instance.GetRows();
        _cols = BoardManager.instance.GetCols();
    }

    private void Update()
    {
        if (!BoardManager.instance.isBuildFinishied && !BoardManager.instance.isGameStarted)
        {
            return;
        }

        var allPlayerUnitsAlive = GetAllPlayerAliveUnits();
        var allCU = GetAllAliveAICommandUnits();

        if (allPlayerUnitsAlive.Count <= 0)
        {
            EndGame();
            return;
        }

        if (allCU.Count <= 0)
        {
            PlayerWin();
        }
    }

    public void PlayerWin()
    {
        isGameOver = true;
        SceneManager.LoadScene(_loadWinScene);

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

    public void EndGame()
    {
        isGameOver = true;
        SceneManager.LoadScene(_loadGameOverScene);
    }

    public KDTree<BasePiece> GetAllPlayerAliveUnits()
    {
        KDTree<BasePiece> allPlayerPieces = new KDTree<BasePiece>();
        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _cols; j++)
            {
                if (BoardManager.instance.BasePieces[i, j] != null && BoardManager.instance.BasePieces[i, j].isPlayer)
                {
                    allPlayerPieces.Add(BoardManager.instance.BasePieces[i, j]);
                }
            }
        }
        return allPlayerPieces;

    }
    private List<BasePiece> GetAllAliveAICommandUnits()
    {
        List<BasePiece> allCU = new List<BasePiece>();
        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _cols; j++)
            {
                if (BoardManager.instance.BasePieces[i, j] != null && !BoardManager.instance.BasePieces[i, j].isPlayer)
                {

                    if (BoardManager.instance.BasePieces[i, j] as ComandUnit)
                    {
                        allCU.Add(BoardManager.instance.BasePieces[i, j]);
                    }
                }
            }
        }
        return allCU;
    }

}
