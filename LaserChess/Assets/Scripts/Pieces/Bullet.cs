using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 70f;

    private Vector3 _target;

    public void AtkTarget(Vector3 target)
    {
        _target = target;
    }

    private void Update()
    {
        if (_target == null)
        {
            return;
        }

        var direction = _target - this.transform.position;
        var distancePerFrame = speed * Time.deltaTime;

        //checking if current distance is lower than distance in the current frame a.k.a if we have already hit the target
        if (direction.magnitude <= distancePerFrame)
        {
            HitTheTarget();
            return;
        }

        //normalized in order to move in constant speed
        transform.Translate(direction.normalized * distancePerFrame, Space.World);
    }

    private void HitTheTarget()
    {
        Destroy(gameObject);
        
        return;
    }
}
