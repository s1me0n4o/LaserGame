using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpShip : BasePiece
{
    public GameObject healthBar;
    private HealthBar _healthbar;

    private int _health = 2;
    private int _atk = 2;

    private int _atkRange = 2;
    private void Start()
    {
        _healthbar = GetComponentInChildren<HealthBar>();
        _healthbar.SetMaxHealth(_health);

        SetHealth(_health);
        SetAttack(_atk);
    }

    public override bool[,] IsPossibleAttack(int CurrentX, int CurrentY)
    {
        var isPossibleAttack = new bool[8, 8];
        BasePiece piecePosition;

        //Attack Forward
        for (int l = 1; l <= _atkRange; l++)
        {
            if (CurrentY != 7 && CurrentY + l <= 7)
            {
                piecePosition = BoardManager.instance.BasePieces[CurrentX, CurrentY + l];
                if (piecePosition != null && piecePosition.isPlayer != isPlayer)
                {
                    isPossibleAttack[CurrentX, CurrentY + l] = true;
                }
                if (piecePosition != null && piecePosition.isPlayer == isPlayer)
                {
                    break;
                }
            }
        }

        //Attack Back
        for (int l = 1; l <= _atkRange; l++)
        {
            if (CurrentY != 0 && CurrentY - l >= 0)
            {

                piecePosition = BoardManager.instance.BasePieces[CurrentX, CurrentY - l];
                if (piecePosition != null && piecePosition.isPlayer != isPlayer)
                {
                    isPossibleAttack[CurrentX, CurrentY - l] = true;
                }
                if (piecePosition != null && piecePosition.isPlayer == isPlayer)
                {
                    break;
                }
            }
        }

        //Attack Left
        for (int l = 1; l <= _atkRange; l++)
        {
            if (CurrentX != 0 && CurrentX - l >= 0)
            {
                piecePosition = BoardManager.instance.BasePieces[CurrentX - l, CurrentY];
                if (piecePosition != null && piecePosition.isPlayer != isPlayer)
                {
                    isPossibleAttack[CurrentX - l, CurrentY] = true;
                }
                if (piecePosition != null && piecePosition.isPlayer == isPlayer)
                {
                    break;
                }
            }
        }

        //Attack Right
        for (int l = 1; l <= _atkRange; l++)
        {
            if (CurrentX != 7 && l + CurrentX <= 7)
            {
                piecePosition = BoardManager.instance.BasePieces[CurrentX + l, CurrentY];
                if (piecePosition != null && piecePosition.isPlayer != isPlayer)
                {
                    isPossibleAttack[CurrentX + l, CurrentY] = true;
                }
                if (piecePosition != null && piecePosition.isPlayer == isPlayer)
                {
                    break;
                }
            }
       }

        return isPossibleAttack;
    }

    public override bool[,] IsPossibleMove()
    {
        var isPossibleToMove = new bool[8, 8];
        //Forward left
        JumpShipMove(CurrentX - 1, CurrentY + 2, ref isPossibleToMove);
        
        //Forward right
        JumpShipMove(CurrentX + 1, CurrentY + 2, ref isPossibleToMove);

        //Right forward
        JumpShipMove(CurrentX + 2, CurrentY + 1, ref isPossibleToMove);

        //Right back
        JumpShipMove(CurrentX + 2, CurrentY -1, ref isPossibleToMove);

        //Back left
        JumpShipMove(CurrentX - 1, CurrentY - 2, ref isPossibleToMove);

        //Back right
        JumpShipMove(CurrentX + 1, CurrentY - 2, ref isPossibleToMove);

        //Left forward
        JumpShipMove(CurrentX - 2, CurrentY + 1, ref isPossibleToMove);

        //Left back
        JumpShipMove(CurrentX - 2, CurrentY - 1, ref isPossibleToMove);
        return isPossibleToMove;
    }

    private void JumpShipMove(int x, int y, ref bool[,] isPossibleToMove)
    {
        BasePiece piecePosition;
        if (x >= 0 && x < 8 && y >= 0 && y < 8)
        {
            piecePosition = BoardManager.instance.BasePieces[x, y];
            if (piecePosition == null)
            {
                isPossibleToMove[x, y] = true;
            }
        }
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
}
