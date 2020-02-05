using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public BoardBuilder boardManager;

    private int _selectedX = -1;
    private int _selectedY = -1;
    private Camera _camera;
    private float hitMaxDistance = 1000f;
    private BasePiece _selectedPiece;
    private bool isMoved = false;

    void Start()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (!BoardBuilder.isBuildFinishied)
        {
            return;
        }

        if (_camera == null)
        {
            return;
        }

        //if we hit a node
        if (Input.GetMouseButtonDown(0))
        {
            SelectXandY();
            if (_selectedX >= 0 && _selectedY >= 0)
            {
                if (_selectedPiece == null)
                {
                    //select piece
                    SelectPiece(_selectedX, _selectedY);
                }
                else if (isMoved == false)
                {
                    //move
                    MovePiece(_selectedX, _selectedY);
                }
                else if (_selectedPiece != null && isMoved)
                {
                    //attack
                }
            }
        }
    }

    //TODO check some bug with moving to certen nodes => probably the calc of the node center
    private void MovePiece(int x, int y)
    {
        if (_selectedPiece.IsPossibleMove(x,y))
        {
            BoardBuilder.BasePieces[_selectedPiece.CurrentX, _selectedPiece.CurrentY] = null;
            _selectedPiece.transform.position = boardManager.GetNodeCenter(x, y);
            BoardBuilder.BasePieces[x, y] = _selectedPiece;
        }

        //deselecting the piece
        _selectedPiece = null;
        isMoved = true;
    }

    private void SelectPiece(int x, int y)
    {
        if (BoardBuilder.BasePieces[x, y] == null || BoardBuilder.BasePieces[x,y].isPlayer != BoardBuilder.isPlayerTurn)
        {
            return;
        }

        _selectedPiece = BoardBuilder.BasePieces[x, y];
        
    }

    private void SelectXandY()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        var isHit = Physics.Raycast(ray, out RaycastHit hit, hitMaxDistance, LayerMask.GetMask("Node"));

        if (isHit)
        {
            _selectedX = (int)hit.point.x;
            _selectedY = (int)hit.point.z;
        }
        else
        {
            _selectedX = -1;
            _selectedY = -1;
        }
    }
}
