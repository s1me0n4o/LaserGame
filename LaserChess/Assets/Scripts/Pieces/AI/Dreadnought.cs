using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO Should attack everybody around him
public class Dreadnought : BasePiece
{
    public GameObject healthBar;
    private HealthBar _healthbar;

    private int _health = 5;
    private int _atk = 2;

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

    public override bool[,] IsPossibleMove()
    {
        var isPossibleToMove = new bool[8, 8];

        BasePiece piecePosition;

        //Move Forward
        if (CurrentY != 0)
        {
            piecePosition = BoardManager.instance.BasePieces[CurrentX, CurrentY - 1];
            if (piecePosition == null)
            {
                isPossibleToMove[CurrentX, CurrentY - 1] = true;
            }
        }

        //Move Back
        if (CurrentY != 7)
        {
            piecePosition = BoardManager.instance.BasePieces[CurrentX, CurrentY + 1];
            if (piecePosition == null)
            {
                isPossibleToMove[CurrentX, CurrentY + 1] = true;
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

        //Move Top Left Diagonal
        if (CurrentX != 0 && CurrentY != 7)
        {
            piecePosition = BoardManager.instance.BasePieces[CurrentX - 1, CurrentY + 1];
            if (piecePosition == null)
            {
                isPossibleToMove[CurrentX - 1, CurrentY + 1] = true;
            }
        }

        //Move Top Right Diagonal
        if (CurrentX != 7 && CurrentY != 7)
        {
            piecePosition = BoardManager.instance.BasePieces[CurrentX + 1, CurrentY + 1];
            if (piecePosition == null)
            {
                isPossibleToMove[CurrentX + 1, CurrentY + 1] = true;
            }
        }

        //Move Bottom Right Diagonal
        if (CurrentX != 7 && CurrentY != 0)
        {
            piecePosition = BoardManager.instance.BasePieces[CurrentX + 1, CurrentY - 1];
            if (piecePosition == null)
            {
                isPossibleToMove[CurrentX + 1, CurrentY - 1] = true;
            }
        }

        //Move Bottom Left Diagonal
        if (CurrentX != 0 && CurrentY != 0)
        {
            piecePosition = BoardManager.instance.BasePieces[CurrentX - 1, CurrentY - 1];
            if (piecePosition == null)
            {
                isPossibleToMove[CurrentX - 1, CurrentY - 1] = true;
            }
        }
        return isPossibleToMove;
    }


    public override bool[,] IsPossibleAttack(int f, int g)
    {
        var isPossibleAttack = new bool[8, 8];
        BasePiece piecePosition;

        //Attack Forward
        if (CurrentY != 0)
        {
            piecePosition = BoardManager.instance.BasePieces[CurrentX, CurrentY - 1];
            if (piecePosition != null && piecePosition.isPlayer != isPlayer)
            {
                isPossibleAttack[CurrentX, CurrentY - 1] = true;
            }
        }

        //Attack Back
        if (CurrentY != 7)
        {
            piecePosition = BoardManager.instance.BasePieces[CurrentX, CurrentY + 1];
            if (piecePosition != null && piecePosition.isPlayer != isPlayer)
            {
                isPossibleAttack[CurrentX, CurrentY + 1] = true;
            }
        }

        //Attack Left
        if (CurrentX != 0)
        {
            piecePosition = BoardManager.instance.BasePieces[CurrentX - 1, CurrentY];
            if (piecePosition != null && piecePosition.isPlayer != isPlayer)
            {
                isPossibleAttack[CurrentX - 1, CurrentY] = true;
            }
        }

        //Attack Right
        if (CurrentX != 7)
        {
            piecePosition = BoardManager.instance.BasePieces[CurrentX + 1, CurrentY];
            if (piecePosition != null && piecePosition.isPlayer != isPlayer)
            {
                isPossibleAttack[CurrentX + 1, CurrentY] = true;
            }
        }

        //Attack Top Left Diagonal
        if (CurrentX != 0 && CurrentY != 7)
        {
            piecePosition = BoardManager.instance.BasePieces[CurrentX - 1, CurrentY + 1];
            if (piecePosition != null && piecePosition.isPlayer != isPlayer)
            {
                isPossibleAttack[CurrentX - 1, CurrentY + 1] = true;
            }
        }

        //Attack Top Right Diagonal
        if (CurrentX != 7 && CurrentY != 7)
        {
            piecePosition = BoardManager.instance.BasePieces[CurrentX + 1, CurrentY + 1];
            if (piecePosition != null && piecePosition.isPlayer != isPlayer)
            {
                isPossibleAttack[CurrentX + 1, CurrentY + 1] = true;
            }
        }

        //Attack Bottom Right Diagonal
        if (CurrentX != 7 && CurrentY != 0)
        {
            piecePosition = BoardManager.instance.BasePieces[CurrentX + 1, CurrentY - 1];
            if (piecePosition != null && piecePosition.isPlayer != isPlayer)
            {
                isPossibleAttack[CurrentX + 1, CurrentY - 1] = true;
            }
        }

        //Attack Bottom Left Diagonal
        if (CurrentX != 0 && CurrentY != 0)
        {
            piecePosition = BoardManager.instance.BasePieces[CurrentX - 1, CurrentY - 1];
            if (piecePosition != null && piecePosition.isPlayer != isPlayer)
            {
                isPossibleAttack[CurrentX - 1, CurrentY - 1] = true;
            }
        }

        return isPossibleAttack;
    }
}
