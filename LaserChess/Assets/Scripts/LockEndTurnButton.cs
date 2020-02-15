using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockEndTurnButton : MonoBehaviour
{
    private Button button;
    private void Start()
    {
        button = GetComponent<Button>();
    }

    private void Update()
    {
        GameManager.OnTurnChange += ChangeButtonStatus;
    }

    public void ChangeButtonStatus(bool isPlayer)
    {
        if (isPlayer)
            button.interactable = true;
        else
            button.interactable = false;
    }
}
