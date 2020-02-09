using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public GameObject buletPrefab;


    private int _selectedX = -1;
    private int _selectedY = -1;
    private Camera _camera;
    private float hitMaxDistance = 25f;
    private BasePiece _selectedPiece;
    private BasePiece _currentSelection;
    private bool hasMoved = false;

    void Start()
    {
        _camera = Camera.main;
    }

    void Update()
    {
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
                if (!hasMoved)
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
        if (BoardManager.instance.allowedAttacks[x,y])
        {
            GameObject bulletGameObject = (GameObject)Instantiate(buletPrefab,
                                                        BoardManager.instance.GetCenterNode(_currentSelection.CurrentX, _currentSelection.CurrentY), 
                                                        Quaternion.identity);
            Bullet bullet = bulletGameObject.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.AtkTarget(BoardManager.instance.GetCenterNode(x, y));
            }

            PossibleMovesManager.instance.HideHighlights();
            hasMoved = false;
            _selectedPiece = null;
        }
        else
        {
            print("KOR");
            _selectedPiece = null;
            PossibleMovesManager.instance.HideHighlights();
            hasMoved = false;
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
        _selectedPiece = null;
    }

    private void SelectPiece(int x, int y)
    {
        if (BoardManager.instance.BasePieces[x, y] == null || 
            BoardManager.instance.BasePieces[x, y].isPlayer != BoardManager.instance.isPlayerTurn)
        {
            return;
        }

        if (!hasMoved)
        {
            BoardManager.instance.allowedMoves = BoardManager.instance.BasePieces[x, y].IsPossibleMove();
            PossibleMovesManager.instance.HighlightPossibleMoves(BoardManager.instance.allowedMoves);
        }

        _selectedPiece = BoardManager.instance.BasePieces[x, y];
        _currentSelection = _selectedPiece;
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

            Debug.Log($"Selection - x: {_selectedX}, y:{_selectedY}");
            Debug.Log($"HitPoint - x: {hit.point.x}, y:{hit.point.z}");
        }
        else
        {
            _selectedX = -1;
            _selectedY = -1;
        }
    }
}
