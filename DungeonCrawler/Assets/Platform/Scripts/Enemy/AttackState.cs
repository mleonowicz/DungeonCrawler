using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IEnemyState
{
    private EnemyPlatform enemy;

    public void Execute()
    {
        if (enemy.Target == null)
            enemy.ChangeState(new PatrolState());

        enemy.LookAtTarget();

        enemy.ChaseEnemy();
    }

    public void Enter(EnemyPlatform enemy)
    {
        this.enemy = enemy;
    }

    public void Exit()
    {
       
    }

    public void OnCollisionEnter(Collision2D coll)
    {
        
    }
}
