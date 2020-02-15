using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public GameObject buletPrefab;

    private void Update()
    {
        if (!GameManager.instance.isPlayerTurn)
        {

            var allAIPieces = new List<BasePiece>();
            var allDrones = new List<BasePiece>();
            var allDreadnought = new List<BasePiece>();
            var allCU = new List<BasePiece>();
            var rows = BoardManager.instance.GetRows();
            var cols = BoardManager.instance.GetCols();

            //get all alive AI pieces
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (BoardManager.instance.BasePieces[i,j] != null && !BoardManager.instance.BasePieces[i, j].isPlayer)
                    {
                        if (BoardManager.instance.BasePieces[i, j] as Drone)
                        {
                            allDrones.Add(BoardManager.instance.BasePieces[i, j]);
                        }

                        if (BoardManager.instance.BasePieces[i, j] as Dreadnought)
                        {
                            allDreadnought.Add(BoardManager.instance.BasePieces[i, j]);
                        }

                        if (BoardManager.instance.BasePieces[i, j] as ComandUnit)
                        {
                            allCU.Add(BoardManager.instance.BasePieces[i, j]);
                        }
                        allAIPieces.Add(BoardManager.instance.BasePieces[i, j]);
                    }
                }
            }

            //Drones
            //var randomNumb = UnityEngine.Random.Range(0, allDrones.Count);

            //var piece = allDrones[randomNumb];

            //TODO this could be level difficulty based
            foreach (var piece in allDrones)
            {
                DroneBehaviour(rows, cols, piece);

            }
            GameManager.instance.EndTurn();





            //drones moves first -> move and atk if possible - if not end of turn

            //when all drones are moved move dreadnoughts
            //move 1 space to the nearest target and atk if possible - if not end of turn

            //CU moves when all DN have moved
        }
        
    }

    private void DroneBehaviour(int rows, int cols, BasePiece piece)
    {
        if (!piece.hasBeenMoved)
        {
            BoardManager.instance.allowedMoves = BoardManager.instance.BasePieces[piece.CurrentX, piece.CurrentY].IsPossibleMove();

            for (int ii = 0; ii < rows; ii++)
            {
                for (int jj = 0; jj < cols; jj++)
                {
                    //if there is available move
                    if (BoardManager.instance.allowedMoves[ii, jj])
                    {
                        //moving
                        MovePiece(piece, ii, jj);

                        //attacking
                        if (piece.hasBeenMoved)
                        {
                            BoardManager.instance.allowedAttacks = BoardManager.instance.BasePieces[piece.CurrentX, piece.CurrentY].IsPossibleAttack();
                            int x = -1, y = -1;
                            for (int k = 0; k < rows; k++)
                            {
                                for (int t = 0; t < cols; t++)
                                {
                                    if (BoardManager.instance.allowedAttacks[k, t])
                                    {
                                        x = k;
                                        y = t;
                                        break;
                                    }
                                }
                            }

                            if (x != -1 && y != -1)
                            {
                                AttackPiece(piece, x, y);

                            }
                        }
                    }
                }
            }
        }
    }

    private void AttackPiece(BasePiece piece, int x, int y)
    {
        var bulletX = BoardManager.instance.GetCenterNode(piece.CurrentX, piece.CurrentY).x;
        var bulletZ = BoardManager.instance.GetCenterNode(piece.CurrentX, piece.CurrentY).z;

        GameObject bulletGameObject = (GameObject)Instantiate(buletPrefab, 
                                                             new Vector3(bulletX, 0.5f ,bulletZ), 
                                                             Quaternion.identity);
        var bulletRenderer = bulletGameObject.GetComponent<Renderer>();
        bulletRenderer.material.color = Color.red;
        Bullet bullet = bulletGameObject.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.AtkTarget(BoardManager.instance.BasePieces[x, y],
                             BoardManager.instance.BasePieces[piece.CurrentX, piece.CurrentY].Attack);
        }
    }

    private void MovePiece(BasePiece piece, int x, int y)
    {
        BoardManager.instance.BasePieces[piece.CurrentX, piece.CurrentY] = null;
        piece.transform.position = BoardManager.instance.GetCenterNode(x, y);
        piece.SetPosition(x, y);
        BoardManager.instance.BasePieces[x, y] = piece;
        piece.hasBeenMoved = true;
    }
}
