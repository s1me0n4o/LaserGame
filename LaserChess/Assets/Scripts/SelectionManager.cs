using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public GameObject buletPrefab;
    public bool hasMoved = false;

    private int _selectedX = -1;
    private int _selectedY = -1;
    private Camera _camera;
    private float hitMaxDistance = 25f;
    private BasePiece _selectedPiece;
    private BasePiece _currentSelection;
    private bool _hasAttacked = false;

    void Start()
    {
        hasMoved = false;
        _camera = Camera.main;
    }

    private void ChangeHasMoved(bool isPlayerTurn)
    {
        hasMoved = false;
        PossibleMovesManager.instance.HideHighlights();

        foreach (var item in BoardManager.instance.BasePieces)
        {
            if (item != null)
            {
                item.hasBeenMoved = false;
                item.hasAttacked = false;
            }
        }
    }

    void Update()
    {
        GameManager.OnTurnChange += ChangeHasMoved;

        if (!BoardManager.instance.isBuildFinishied)
        {
            return;
        }

        if (_camera == null)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            SelectXandY();
            if (_selectedX >= 0 && _selectedY >= 0)
            {
                if (!hasMoved) //after AI this is always true
                {
                    if (_selectedPiece == null)
                    {
                        SelectPiece(_selectedX, _selectedY);
                    }
                    else
                    {
                        MovePiece(_selectedX, _selectedY);
                    }
                }
                else
                {
                    AttackEnemy(_selectedX, _selectedY);
                }
            }
        }
    }

    private void AttackEnemy(int x, int y)
    {
        //jumpship
        //Not cool, TODO think for better approach if you have time
        if (_currentSelection as JumpShip)
        {
            if (!_currentSelection.hasAttacked)
            {
                var rows = BoardManager.instance.GetRows();
                var cols = BoardManager.instance.GetCols();
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        if ((BoardManager.instance.allowedAttacks[i, j]))
                        {
                            var enemyPiece = BoardManager.instance.BasePieces[i, j];
                            if (enemyPiece != null)
                            {
                                InitBullet(i, j);
                            }
                        }
                    }
                }
                _currentSelection.hasAttacked = true;
                PossibleMovesManager.instance.HideHighlights();
                _selectedPiece = null;
                hasMoved = false;
            }
        }
        else
        {
            if (BoardManager.instance.allowedAttacks[x, y])
            {
                InitBullet(x, y);
                PossibleMovesManager.instance.HideHighlights();

                _selectedPiece = null;
                hasMoved = false;
            }
            else
            {
                print("Attack the suggested zones. If there are no such zones, no more available moves for this turn! Suggested end of turn.");
            }
        }
    }

    private void InitBullet(int i, int j)
    {
        GameObject bulletGameObject = (GameObject)Instantiate(buletPrefab,
                                        BoardManager.instance.GetCenterNode(_currentSelection.CurrentX, _currentSelection.CurrentY),
                                        Quaternion.identity);
        Bullet bullet = bulletGameObject.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.AtkTarget(BoardManager.instance.BasePieces[i, j], BoardManager.instance.BasePieces[_currentSelection.CurrentX, _currentSelection.CurrentY].Attack);
        }
    }

    private void MovePiece(int x, int y)
    {
        if (BoardManager.instance.allowedMoves[x,y] && 
            BoardManager.instance.BasePieces[_selectedPiece.CurrentX, _selectedPiece.CurrentY] != BoardManager.instance.BasePieces[x, y])
        {
            BoardManager.instance.BasePieces[_selectedPiece.CurrentX, _selectedPiece.CurrentY] = null;
            _selectedPiece.transform.position = BoardManager.instance.GetCenterNode(x, y);
            _selectedPiece.SetPosition(x, y);
            BoardManager.instance.BasePieces[x, y] = _selectedPiece;

            PossibleMovesManager.instance.HideHighlights();
            _selectedPiece.hasBeenMoved = true;
            hasMoved = true;

        }
        else
        {
            _selectedPiece = null;
            PossibleMovesManager.instance.HideHighlights();
        }

        if (hasMoved)
        {
            BoardManager.instance.allowedAttacks = BoardManager.instance.BasePieces[x, y].IsPossibleAttack();
            PossibleMovesManager.instance.HighlightPossibleAttack(BoardManager.instance.allowedAttacks);
        }
        else
        {
            Debug.Log("No possible moves");
        }
 
        
        _selectedPiece = null;
    }

    private void SelectPiece(int x, int y)
    {
        if (BoardManager.instance.BasePieces[x, y] == null || 
            BoardManager.instance.BasePieces[x, y].isPlayer != GameManager.instance.isPlayerTurn)
        {
            return;
        }

        if (!hasMoved && !BoardManager.instance.BasePieces[x, y].hasBeenMoved)
        {
            BoardManager.instance.allowedMoves = BoardManager.instance.BasePieces[x, y].IsPossibleMove();
            PossibleMovesManager.instance.HighlightPossibleMoves(BoardManager.instance.allowedMoves);
            _selectedPiece = BoardManager.instance.BasePieces[x, y];
            _currentSelection = _selectedPiece;

        }
        else
        {
            Debug.Log("Already moved this piece");

            _selectedPiece = null;
        }

    }

    private void SelectXandY()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        var isHit = Physics.Raycast(ray, out RaycastHit hit, hitMaxDistance, LayerMask.GetMask("Node"));

        if (isHit)
        {
            var selectionX = Math.Floor(hit.point.x);
            _selectedX = (int)selectionX;
            var selectionY = Math.Floor(hit.point.z);
            _selectedY = (int)selectionY;
        }
        else
        {
            _selectedX = -1;
            _selectedY = -1;
        }
    }
}
