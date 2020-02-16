using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComandUnit : BasePiece
{
    public GameObject healthBar;
    private HealthBar _healthbar;
    private int _health = 5;
    private int _atk = 0;

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

        return isPossibleToMove;
    }

}
