using Assets.Scripts.Pieces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePiece : MonoBehaviour, IBasePiece
{
    public bool isPlayer;

    public int CurrentX { get; set; }
    public int CurrentY { get; set; }

    public void SetPosition(int x, int y)
    {
        CurrentX = x;
        CurrentY = y;
    }

    public virtual bool IsPossibleMove(int x, int y)
    {
        return true;
    }
}
