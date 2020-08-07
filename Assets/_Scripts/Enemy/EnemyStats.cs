using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : Stats
{
    [SerializeField] private EnemyController _enemy;
    

    public override void Damage(float amount)
    {
        if (!_enemy.IsSpawned)
        {
            return;
        }
        base.Damage(amount);
    }
}
