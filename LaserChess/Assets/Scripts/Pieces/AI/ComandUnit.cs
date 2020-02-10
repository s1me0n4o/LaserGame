using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComandUnit : BasePiece
{
    public HealthBar healthBar;
    private int _health = 5;
    private int _atk = 0;

    private void Start()
    {
        healthBar.SetMaxHealth(_health);
        SetHealth(_health);
        SetAttack(_atk);
    }
    public override void TakeDmg(int damage)
    {
        _health -= damage;
        healthBar.SetHealth(_health);

        if (_health <= 0)
        {
            Die();
        }
    }

    //TODO implement movements
}
