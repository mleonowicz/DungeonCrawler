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
        enemy.Move();

        if (enemy.Target.myBoxCollider2D.bounds.min.y > enemy.myBoxCollider2D.bounds.min.y)
            if (Physics2D.Raycast(enemy.myBoxCollider2D.bounds.center, Vector2.up, 5, enemy.myLayerMask)) 
                enemy.Jump();
     
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
