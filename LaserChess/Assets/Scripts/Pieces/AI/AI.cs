using Assets.Scripts.External;
using Assets.Scripts.Pieces.Enums;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AI : MonoBehaviour
{
    public GameObject buletPrefab;
    private State _state;

    public delegate void StateFinished(State state);
    public static event StateFinished OnStateFinished;

    private float _timeToWait = 1f;
    private int _moveCounterDrones;
    private int _moveCounterDreadnought;
    private int _rows;
    private int _cols;
    private bool _allDronesHaveBeenMoved;
    private bool _allDreadnoughtsHaveBeenMoved;

    private const int Move_straight_cost = 10;
    private const int Move_diagonal_cost = 14;

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

        _moveCounterDrones = 0;
        _moveCounterDreadnought = 0;
    }
    private void OnEnable()
    {
        GameManager.OnTurnChange += InitState;
        OnStateFinished += ChangeState;
    }

    private void OnDisable()
    {
        GameManager.OnTurnChange -= InitState;
    }

    private void InitState(bool isPlayerTurn)
    {
        if (GameManager.instance.isPlayerTurn)
        {
            return;
        }

        if (!GameManager.instance.isPlayerTurn)
        {
            State = State.Drones;
        }
    }
   
    private void ChangeState(State state)
    {
        ExitState();
        State = state;
    }

    private void EnterState(State stateEntered)
    {
        while (true)
        {
            switch (stateEntered)
            {
                case State.Drones:

                    var allDrones = GetAllAliveAIDrones();
                    StartCoroutine(DroneBehaviour(allDrones));

                    break;
                case State.DN:
                    var allDreadnought = GetAllAliveAIDreadnought();
                    StartCoroutine(DreadnoughtBehaviour(allDreadnought));

                    break;
                case State.CU:
                    var allCommandUnits = GetAllAliveAICommandUnits();
                    StartCoroutine(CommandUnitsBehaviour(allCommandUnits));
                    break;
                case State.EndTurn:
                    GameManager.instance.EndTurn();
                    break;
            }
            break;
        }
    }
    private void ExitState()
    {
        StopAllCoroutines();
    }

    //TODO fix the states and check if the below works
    private IEnumerator CommandUnitsBehaviour(List<BasePiece> allCommandUnits)
    {
        foreach (var cu in allCommandUnits)
        {
            //overriding tempy
            var cuTempYTop = cu.CurrentY + 1;
            var cuTempYBottom = cu.CurrentY - 1;

            BoardManager.instance.allowedMoves = BoardManager.instance.BasePieces[cu.CurrentX, cu.CurrentY].IsPossibleMove();
            for (int ii = 0; ii < _rows; ii++)
            {
                for (int jj = 0; jj < _cols; jj++)
                {
                    //if there is available move
                    if (BoardManager.instance.allowedMoves[ii, jj])
                    {
                        var allPlayerPieces = GetAllPlayerAliveUnits();
                        
                        //TODO: finish this
                        foreach (var pPiece in allPlayerPieces)
                        {
                            //check all possible moves of Player
                            var possibleMoves = pPiece.IsPossibleMove();
                            for (int i = 0; i < _rows; i++)
                            {
                                for (int j = 0; j < _cols; j++)
                                {
                                    if (possibleMoves[i,j])
                                    {
                                        //TODO maybe add props futureMoveX, futureMoveZ
                                        pPiece.TempX = i;
                                        pPiece.TempY = j;
                                        var possibleAttacks = BoardManager.instance.BasePieces[pPiece.TempX, pPiece.TempY].IsPossibleAttack();

                                        for (int x = 0; x < _rows; x++)
                                        {
                                            for (int y = 0; y < _cols; y++)
                                            {
                                        //if there are potential attacks for the current pos
                                                if (possibleAttacks[x,y])
                                                {
                                                    //TODO check if the coordinates match cu[x,y]
                                                    if (possibleAttacks[x,y] == BoardManager.instance.BasePieces[cu.CurrentX, cu.CurrentY])
                                                    {
                                                        if (possibleAttacks[x, y] == BoardManager.instance.BasePieces[cu.CurrentX, cuTempYTop])
                                                        {
                                                            //go bot (y-1)
                                                            MovePiece(cu, cu.CurrentX, cuTempYBottom);
                                                            break;
                                                        }
                                                        else if (possibleAttacks[x, y] == BoardManager.instance.BasePieces[cu.CurrentX, cuTempYBottom])
                                                        {
                                                            //go top (y+1)
                                                            //moving
                                                            MovePiece(cu, cu.CurrentX, cuTempYTop);
                                                            break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //dont move
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }
                }
            }
            yield return new WaitForSeconds(_timeToWait);
        }
        OnStateFinished(State.EndTurn);
    }

    private IEnumerator DreadnoughtBehaviour(List<BasePiece> dreadnoughts)
    {
        foreach (var dreadnought in dreadnoughts)
        {
            if (!dreadnought.hasBeenMoved)
            {
                BoardManager.instance.allowedMoves = BoardManager.instance.BasePieces[dreadnought.CurrentX, dreadnought.CurrentY].IsPossibleMove();

                var allPlayerUnits = GetAllPlayerAliveUnits();
                allPlayerUnits.UpdatePositions();
                var nearestEnenmy = allPlayerUnits.FindClosest(dreadnought.transform.position);

                Debug.DrawLine(new Vector3(dreadnought.transform.position.x, 0.5f, dreadnought.transform.position.z), new Vector3(nearestEnenmy.transform.position.x, 0.5f, nearestEnenmy.transform.position.z), Color.red, 5);

                for (int ii = 0; ii < _rows; ii++)
                {
                    for (int jj = 0; jj < _cols; jj++)
                    {
                        //if there is available move
                        if (BoardManager.instance.allowedMoves[ii, jj])
                        {
                            int tempX, tempY;
                            CalculateOptimalPath(dreadnought, nearestEnenmy, out tempX, out tempY);
                            //moving
                            if (tempX == ii && tempY == jj && MovePiece(dreadnought, tempX, tempY))
                            {
                                _moveCounterDreadnought++;
                            }

                            //attacking
                            if (dreadnought.hasBeenMoved && !dreadnought.hasAttacked)
                            {
                                BoardManager.instance.allowedAttacks = BoardManager.instance.BasePieces[dreadnought.CurrentX, dreadnought.CurrentY].IsPossibleAttack();

                                //attack all surroundings
                                for (int i = 0; i < _rows; i++)
                                {
                                    for (int j = 0; j < _cols; j++)
                                    {
                                        if ((BoardManager.instance.allowedAttacks[i, j]))
                                        {
                                            var enemyPiece = BoardManager.instance.BasePieces[i, j];
                                            if (enemyPiece != null)
                                            {
                                                AttackPiece(dreadnought, i, j);
                                            }
                                        }
                                    }
                                }
                                dreadnought.hasAttacked = true;
                            }
                        }
                    }
                }
            }
            yield return new WaitForSeconds(_timeToWait);
        }
        if (_allDreadnoughtsHaveBeenMoved)
        {
            OnStateFinished(State.CU);
        }
        else
        {
            if (_moveCounterDreadnought >= dreadnoughts.Count)
            {
                _allDreadnoughtsHaveBeenMoved = true;
                OnStateFinished(State.CU);
            }
            else
            {
                OnStateFinished(State.EndTurn);
            }
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
                            if (MovePiece(piece, ii, jj))
                            {
                                _moveCounterDrones++;
                            }

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
        if (_allDronesHaveBeenMoved)
        {
            OnStateFinished(State.DN);
        }
        else
        {
            if (_moveCounterDrones >= allDrones.Count)
            {
                _allDronesHaveBeenMoved = true;
                OnStateFinished(State.DN);
            }
            else
            {
                OnStateFinished(State.EndTurn);
            }
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

    private bool MovePiece(BasePiece piece, int x, int y)
    {
        if (!piece.hasBeenMoved)
        {
            BoardManager.instance.BasePieces[piece.CurrentX, piece.CurrentY] = null;
            piece.transform.position = BoardManager.instance.GetCenterNode(x, y);
            piece.SetPosition(x, y);
            BoardManager.instance.BasePieces[x, y] = piece;
            piece.hasBeenMoved = true;
            return true;
        }
        return false;
    }

    private List<BasePiece> GetAllAliveAIDrones()
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
                }
            }
        }
        return allDrones;
    }

    private List<BasePiece> GetAllAliveAIDreadnought()
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
                }
            }
        }
        return allDreadnought;
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
   
    private KDTree<BasePiece> GetAllPlayerAliveUnits()
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

    private void CalculateOptimalPath(BasePiece dreadnought, BasePiece nearestEnenmy, out int tempX, out int tempY)
    {
        //getting values between -1 and 1 based on enemy position
        var dirX = Mathf.Max(Mathf.Min(nearestEnenmy.CurrentX - dreadnought.CurrentX, 1), -1);
        var dirY = Mathf.Max(Mathf.Min(nearestEnenmy.CurrentY - dreadnought.CurrentY, 1), -1);

        //calculating the best future point
        tempX = dreadnought.CurrentX + dirX;
        tempY = dreadnought.CurrentY + dirY;
    }
}
