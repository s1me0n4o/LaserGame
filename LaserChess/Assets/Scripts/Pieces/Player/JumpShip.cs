using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpShip : BasePiece
{
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
        if (x >= 0 && x < 8 && y >= 0 && y < 0)
        {
            piecePosition = BoardManager.instance.BasePieces[x, y];
            if (piecePosition == null)
            {
                isPossibleToMove[x, y] = true;
            }
        }
    }
}
