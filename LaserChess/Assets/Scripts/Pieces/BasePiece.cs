using Assets.Scripts.Pieces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePiece : MonoBehaviour, IBasePiece
{
    public bool isPlayer;
    public bool hasBeenMoved;

    public int CurrentX { get; set; }
    public int CurrentY { get; set; }

    public int Health { get; set; }
    public int Attack { get; set; }


    public void SetPosition(int x, int y)
    {
        CurrentX = x;
        CurrentY = y;
    }

    public void SetHealth(int health)
    {
        Health = health;
    }

    public void SetAttack(int attack)
    {
        Attack = attack;
    }

    public virtual void TakeDmg(int damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public virtual bool[,] IsPossibleAttack()
    {
        return new bool[8, 8];
    }

    public virtual bool[,] IsPossibleMove()
    {
        return new bool[8,8];
    }

 

    private void Die(GameObject go)
    {
        Destroy(go);
    }
}
