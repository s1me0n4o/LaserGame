﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grunt : BasePiece
{
    public GameObject healthBar;
    private HealthBar _healthbar;

    private int _health = 2;
    private int _atk = 1;

    private void Start()
    {
        _healthbar = GetComponentInChildren<HealthBar>();
        _healthbar.SetMaxHealth(_health);
        SetHealth(_health);
        SetAttack(_atk);
    }

    public override void TakeDmg(int damage)
    {
        _health -= damage;
        _healthbar.SetHealth(_health);

        if (_health <= 0)
        {
            Die();
        }
    }

    public override bool[,] IsPossibleAttack(int CurrentX, int CurrentY)
    {
        var isPossibleAttack = new bool[8, 8];
        BasePiece piecePosition;
        int i, j;

        //Top Left Diagonal ATK
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i--;
            j++;
            if (i < 0 || j >= 8)
            {
                break;
            }

            piecePosition = BoardManager.instance.BasePieces[i, j];
            if (piecePosition != null && piecePosition.isPlayer != isPlayer)
            {
                isPossibleAttack[i, j] = true;
                break;
            }
            if (piecePosition != null && piecePosition.isPlayer == isPlayer)
            {
                break;
            }
        }

        //Bottom Right Diagonal ATK
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i++;
            j--;
            if (j < 0 || i >= 8)
            {
                break;
            }

            piecePosition = BoardManager.instance.BasePieces[i, j];
            if (piecePosition != null && piecePosition.isPlayer != isPlayer)
            {
                isPossibleAttack[i, j] = true;
                break;
            }
            if (piecePosition != null && piecePosition.isPlayer == isPlayer)
            {
                break;
            }
        }

        //Top Right Diagonal ATK
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i++;
            j++;
            if (j >= 8 || i >= 8)
            {
                break;
            }

            piecePosition = BoardManager.instance.BasePieces[i, j];
            if (piecePosition != null && piecePosition.isPlayer != isPlayer)
            {
                isPossibleAttack[i, j] = true;
                break;
            }
            if (piecePosition != null && piecePosition.isPlayer == isPlayer)
            {
                break;
            }
        }

        //Bottom Left Diagonal ATK
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i--;
            j--;
            if (j < 0 || i < 0)
            {
                break;
            }

            piecePosition = BoardManager.instance.BasePieces[i, j];
            if (piecePosition != null && piecePosition.isPlayer != isPlayer)
            {
                isPossibleAttack[i, j] = true;
                break;
            }
            if (piecePosition != null && piecePosition.isPlayer == isPlayer)
            {
                break;
            }
        }
                       
        return isPossibleAttack;
    }

    public override bool[,] IsPossibleMove()
    {
        var isPossibleToMove = new bool[8, 8];

        BasePiece piecePosition;
        
        //Move Forward
        if (CurrentY != 7)
        {
            piecePosition = BoardManager.instance.BasePieces[CurrentX, CurrentY + 1];
            if (piecePosition == null)
            {
                isPossibleToMove[CurrentX, CurrentY + 1] = true;
            }
        }

        //Move Back
        if (CurrentY != 0)
        {
            piecePosition = BoardManager.instance.BasePieces[CurrentX, CurrentY - 1];
            if (piecePosition == null)
            {
                isPossibleToMove[CurrentX, CurrentY - 1] = true;
            }
        }

        //Move Left
        if (CurrentX != 0)
        {
            piecePosition = BoardManager.instance.BasePieces[CurrentX - 1, CurrentY];
            if (piecePosition == null)
            {
                isPossibleToMove[CurrentX - 1, CurrentY] = true;
            }
        }

        //Move Right
        if (CurrentX != 7)
        {
            piecePosition = BoardManager.instance.BasePieces[CurrentX + 1, CurrentY];
            if (piecePosition == null)
            {
                isPossibleToMove[CurrentX + 1, CurrentY] = true;
            }
        }

        return isPossibleToMove;
    }
}
