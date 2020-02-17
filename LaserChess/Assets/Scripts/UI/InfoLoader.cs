using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InfoLoader : MonoBehaviour
{
    public GameObject qMark;
    public GameObject infoTab;

    private bool qMarkAlive = true;
    private bool infoTablAlive = false;
    public void ActivateInfo()
    {
        if (qMarkAlive && !infoTablAlive)
        {
            qMark.SetActive(false);
            infoTab.SetActive(true);
            infoTablAlive = true;
            qMarkAlive = false;
        }
        else
        {
            qMark.SetActive(true);
            infoTab.SetActive(false);
            infoTablAlive = false;
            qMarkAlive = true;
        }

    }
}
