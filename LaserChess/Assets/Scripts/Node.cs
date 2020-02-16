using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Color hoverColor = Color.grey;

    private Renderer _renderer;
    private Color _startColor;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _startColor = _renderer.material.color;
    }

    private void OnMouseEnter()
    {
        if (!BoardManager.instance.isBuildFinishied)
        {
            return;
        }

        _renderer.material.color = hoverColor;
    }

    private void OnMouseExit()
    {
        if (!BoardManager.instance.isBuildFinishied)
        {
            return;
        }

        _renderer.material.color = _startColor;
    }

    private void OnMouseDown()
    {
        if (!BoardManager.instance.isBuildFinishied)
        {
            return;
        }

        _renderer.material.color = Color.blue;
    }

}
