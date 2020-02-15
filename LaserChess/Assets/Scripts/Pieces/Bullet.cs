using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 70f;

    private BasePiece _target;
    private int _dmg = 0;

    public void AtkTarget(BasePiece target, int dmg)
    {
        _target = target;
        _dmg = dmg;
    }

    private void Update()
    {
        if (_target == null)
        {
            return;
        }        

        var direction = new Vector3(_target.transform.position.x, 0.5f, _target.transform.position.z) - this.transform.position;
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
        Damage(_target, _dmg);
        Destroy(gameObject);
        
        return;
    }

    private void Damage(BasePiece target, int dmg)
    {
        target.TakeDmg(dmg);
    }
}
