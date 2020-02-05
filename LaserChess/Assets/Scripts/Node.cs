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

    void OnMouseEnter()
    {
        if (!BoardBuilder.isBuildFinishied)
        {
            return;
        }

        _renderer.material.color = hoverColor;
    }

    void OnMouseExit()
    {
        if (!BoardBuilder.isBuildFinishied)
        {
            return;
        }

        _renderer.material.color = _startColor;
    }

    void OnMouseDown()
    {
        if (!BoardBuilder.isBuildFinishied)
        {
            return;
        }

    }
}
