using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grunt : BasePiece
{
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
