using Assets.Scripts.Pieces.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public GameObject buletPrefab;
    private List<BasePiece> _movedDronesCount;
    private State _state;

    private List<BasePiece> allDrones;
    private float _timeToWait = 1f;
    private int _moveCounter;
    private int _rows;
    private int _cols;

    public State State
    {
        get
        {
            return _state;
        }
        set
        {
            //ExitState();
            _state = value;
            EnterState(_state);
        }
    }
    private void Start()
    {
        _rows = BoardManager.instance.GetRows();
        _cols = BoardManager.instance.GetCols();

        _movedDronesCount = new List<BasePiece>();
        _moveCounter = 0;
    }

    private void Update()
    {
        if (!BoardManager.instance.isBuildFinishied)
        {
            return;
        }

        if (!GameManager.instance.isPlayerTurn)
        {
            State = State.Drones;



            //EnterState(_state, 8, 8);
        }
}

    //private void Update()
    //{
    //    if (!GameManager.instance.isPlayerTurn)
    //    {

    //        //get all alive AI pieces
    //        GetAllAliveAIPieces(allAIPieces, allDrones, allDreadnought, allCU, rows, cols);

    //        //Drones
    //        //var randomNumb = UnityEngine.Random.Range(0, allDrones.Count);

    //        //var piece = allDrones[randomNumb];

    //        //TODO this could be level difficulty based
    //        foreach (var piece in allDrones)
    //        {
    //            DroneBehaviour(rows, cols, piece);
    //        }

    //        //if all drones are moved
    //        foreach (var drone in allDrones)
    //        {
    //            if (drone.hasBeenMoved)
    //            {
    //                _movedDronesCount.Add(drone);
    //            }
    //        }

    //        //move Dreadnoughts
    //        if (_movedDronesCount.Count == allDrones.Count)
    //        {
    //            foreach (var dreadnought in allDreadnought)
    //            {
    //                DreadnoughtBehaviour(rows, cols, dreadnought);
    //            }
    //        }






    //        //drones moves first -> move and atk if possible - if not end of turn

    //        //when all drones are moved move dreadnoughts
    //        //move 1 space to the nearest target and atk if possible - if not end of turn

    //        //CU moves when all DN have moved
    //    }

    //}



    private void EnterState(State stateEntered)
    {
        var allAIPieces = new List<BasePiece>();
        //var allDrones = new List<BasePiece>();
        //var allDreadnought = new List<BasePiece>();
        //var allCU = new List<BasePiece>();

        switch (stateEntered)
        {
            case State.Drones:

                var allDrones = GetAllAliveAIDrones(allAIPieces);
                StartCoroutine(DroneBehaviour(allDrones));

                if (_moveCounter == allDrones.Count)
                {
                    ExitState();
                    State = State.DN;
                }

                break;
            case State.DN:
                var allDreadnought = GetAllAliveAIDreadnought(allAIPieces);
                StartCoroutine(DreadnoughtBehaviour(allDreadnought));

                break;
            case State.CU:
                break;
            default:
                GameManager.instance.EndTurn();

                break;
        }
    }

    private void ExitState()
    {
        StopAllCoroutines();
    }

    private List<BasePiece> GetAllAliveAIDrones(List<BasePiece> allAIPieces)
    {
        List<BasePiece> allDrones = new List<BasePiece>();
        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _cols; j++)
            {
                if (BoardManager.instance.BasePieces[i, j] != null && !BoardManager.instance.BasePieces[i, j].isPlayer)
                {
                    if (BoardManager.instance.BasePieces[i, j] as Drone)
                    {
                        allDrones.Add(BoardManager.instance.BasePieces[i, j]);
                    }

                    allAIPieces.Add(BoardManager.instance.BasePieces[i, j]);
                }
            }
        }
        return allDrones;
    }

    private List<BasePiece> GetAllAliveAIDreadnought(List<BasePiece> allAIPieces)
    {
        List<BasePiece> allDreadnought = new List<BasePiece>();
        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _cols; j++)
            {
                if (BoardManager.instance.BasePieces[i, j] != null && !BoardManager.instance.BasePieces[i, j].isPlayer)
                {
                    if (BoardManager.instance.BasePieces[i, j] as Dreadnought)
                    {
                        allDreadnought.Add(BoardManager.instance.BasePieces[i, j]);
                    }

                    allAIPieces.Add(BoardManager.instance.BasePieces[i, j]);
                }
            }
        }
        return allDreadnought;
    }

    private List<BasePiece> GetAllAliveAIComandUnits(List<BasePiece> allAIPieces)
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
                    allAIPieces.Add(BoardManager.instance.BasePieces[i, j]);
                }
            }
        }
        return allCU;
    }


    private IEnumerator DreadnoughtBehaviour(List<BasePiece> dreadnoughts)
    {
        _moveCounter = 0;
        foreach (var dreadnought in dreadnoughts)
        {
            if (!dreadnought.hasBeenMoved)
            {
                BoardManager.instance.allowedMoves = BoardManager.instance.BasePieces[dreadnought.CurrentX, dreadnought.CurrentY].IsPossibleMove();
                for (int ii = 0; ii < _rows; ii++)
                {
                    for (int jj = 0; jj < _cols; jj++)
                    {
                        //if there is available move
                        if (BoardManager.instance.allowedMoves[ii, jj])
                        {
                            //moving
                            MovePiece(dreadnought, ii, jj);
                            _moveCounter++;

                            //attacking
                            if (dreadnought.hasBeenMoved)
                            {
                                BoardManager.instance.allowedAttacks = BoardManager.instance.BasePieces[dreadnought.CurrentX, dreadnought.CurrentY].IsPossibleAttack();

                                //TODO change that if have time.
                                int x, y;
                                PickPossibleAttack(out x, out y);
                              
                                if (x != -1 && y != -1)
                                {
                                    AttackPiece(dreadnought, x, y);
                                    Debug.Log("Attacked");
                                }
                            }
                        }
                    }
                }
            }
            yield return new WaitForSeconds(_timeToWait);
        }
    }

    private IEnumerator DroneBehaviour(List<BasePiece> allDrones)
    {
        foreach (var piece in allDrones)
        {
            if (!piece.hasBeenMoved)
            {
                BoardManager.instance.allowedMoves = BoardManager.instance.BasePieces[piece.CurrentX, piece.CurrentY].IsPossibleMove();
                for (int ii = 0; ii < _rows; ii++)
                {
                    for (int jj = 0; jj < _cols; jj++)
                    {
                        //if there is available move
                        if (BoardManager.instance.allowedMoves[ii, jj])
                        {
                            //moving
                            MovePiece(piece, ii, jj);
                            _moveCounter++;

                            //attacking
                            if (piece.hasBeenMoved)
                            {
                                BoardManager.instance.allowedAttacks = BoardManager.instance.BasePieces[piece.CurrentX, piece.CurrentY].IsPossibleAttack();
                                
                                //TODO change that if have time.
                                int x, y;
                                PickPossibleAttack(out x, out y);

                                if (x != -1 && y != -1)
                                {
                                    AttackPiece(piece, x, y);
                                }
                            }
                        }
                    }
                }
            }
            yield return new WaitForSeconds(_timeToWait);
        }
    }

    private void PickPossibleAttack(out int x, out int y)
    {
        x = -1;
        y = -1;
        for (int k = 0; k < _rows; k++)
        {
            for (int t = 0; t < _cols; t++)
            {
                if (BoardManager.instance.allowedAttacks[k, t])
                {
                    x = k;
                    y = t;
                    break;
                }
            }
        }
    }

    private void AttackPiece(BasePiece piece, int x, int y)
    {
        var bulletX = BoardManager.instance.GetCenterNode(piece.CurrentX, piece.CurrentY).x;
        var bulletZ = BoardManager.instance.GetCenterNode(piece.CurrentX, piece.CurrentY).z;

        GameObject bulletGameObject = (GameObject)Instantiate(buletPrefab, 
                                                             new Vector3(bulletX, 0.5f ,bulletZ), 
                                                             Quaternion.identity);
        var bulletRenderer = bulletGameObject.GetComponent<Renderer>();
        bulletRenderer.material.color = Color.red;
        Bullet bullet = bulletGameObject.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.AtkTarget(BoardManager.instance.BasePieces[x, y],
                             BoardManager.instance.BasePieces[piece.CurrentX, piece.CurrentY].Attack);
        }
    }

    private void MovePiece(BasePiece piece, int x, int y)
    {
        BoardManager.instance.BasePieces[piece.CurrentX, piece.CurrentY] = null;
        piece.transform.position = BoardManager.instance.GetCenterNode(x, y);
        piece.SetPosition(x, y);
        BoardManager.instance.BasePieces[x, y] = piece;
        piece.hasBeenMoved = true;
    }
}
