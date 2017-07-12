using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IEnemyState
{
    private EnemyPlatform enemy;

    public void Execute()
    {
        enemy.Move();
    }

    public void Enter(EnemyPlatform enemy)
    {
        this.enemy = enemy;
    }

    public void Exit()
    {
        
    }

    public void OnTriggerEnter(Collider2D coll)
    {
        
    }
}
