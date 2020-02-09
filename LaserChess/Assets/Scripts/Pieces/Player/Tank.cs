using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : BasePiece
{

    //TODO Fixed range up to 3 spaces
    public override bool[,] IsPossibleMove()
    {
        var isPossibleToMove = new bool[8, 8];

        BasePiece piecePosition;
        int i, j;

        //Move Forward
        i = CurrentY;
        while (true)
        {
            i++;
            if (i >= 8)
            {
                break;
            }

            piecePosition = BoardManager.instance.BasePieces[CurrentX, i];
            if (piecePosition == null)
            {
                isPossibleToMove[CurrentX, i] = true;
            }
            else
            {
                isPossibleToMove[CurrentX, i - 1] = true;
                break;
            }
        }

        //Move Back
        i = CurrentY;
        while (true)
        {
            i--;
            if (i < 0)
            {
                break;
            }

            piecePosition = BoardManager.instance.BasePieces[CurrentX, i];
            if (piecePosition == null)
            {
                isPossibleToMove[CurrentX, i] = true;
            }
            else
            {
                isPossibleToMove[CurrentX, i + 1] = true;
                break;
            }
        }

        //Move Left
        i = CurrentX;
        while (true)
        {
            i--;
            if (i < 0)
            {
                break;
            }

            piecePosition = BoardManager.instance.BasePieces[i, CurrentY];
            if (piecePosition == null)
            {
                isPossibleToMove[i, CurrentY] = true;
            }
            else
            {
                isPossibleToMove[i + 1, CurrentY] = true;
                break;
            }
        }

        //Move Right
        i = CurrentX;
        while (true)
        {
            i++;
            if (i >= 8)
            {
                break;
            }

            piecePosition = BoardManager.instance.BasePieces[i, CurrentY];
            if (piecePosition == null)
            {
                isPossibleToMove[i, CurrentY] = true;
            }
            else
            {
                isPossibleToMove[i - 1, CurrentY] = true;
                break;
            }
        }

        //Top Left Diagonal
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
            if (piecePosition == null)
            {
                isPossibleToMove[i, j] = true;
            }
            else
            {
                isPossibleToMove[i + 1, j - 1] = true;
                break;
            }
        }

        //Bottom Right Diagonal
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
            if (piecePosition == null)
            {
                isPossibleToMove[i, j] = true;
            }
            else
            {
                isPossibleToMove[i - 1, j + 1] = true;
                break;
            }
        }

        //Top Right Diagonal
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
            if (piecePosition == null)
            {
                isPossibleToMove[i, j] = true;
            }
            else
            {
                isPossibleToMove[i - 1, j - 1] = true;
                break;
            }
        }

        //Bottom Left Diagonal
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
            if (piecePosition == null)
            {
                isPossibleToMove[i, j] = true;
            }
            else
            {
                isPossibleToMove[i + 1, j + 1] = true;
                break;
            }
        }

        return isPossibleToMove;
    }

}
