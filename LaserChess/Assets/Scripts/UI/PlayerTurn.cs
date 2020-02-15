using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTurn : MonoBehaviour
{
    private Text playerTurn;

    private void Start()
    {
        playerTurn = GetComponent<Text>();
    }
    void Update()
    {
        playerTurn.text = GameManager.instance.isPlayerTurn ? "Player Turn" : "AI Turn";
    }
}
