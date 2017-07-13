using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IEnemyState
{
    private EnemyPlatform enemy;

    public void Execute()
    {
        enemy.Move();

        if (enemy.Target != null)
            enemy.ChangeState(new AttackState());
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
        if (coll.collider.tag == "Wall")
            enemy.ChangeDirection();
    }
}
