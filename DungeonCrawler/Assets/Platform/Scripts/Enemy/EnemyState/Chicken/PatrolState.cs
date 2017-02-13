using UnityEngine;
using System.Collections;
using System;

public class PatrolState : IEnemyPlatformState
{

    private EnemyPlatform myEnemy;

    private float speed = 0.04f;

    public void Execute()
    {
        myEnemy.transform.position += new Vector3(speed, 0);
    }

    public void Enter(EnemyPlatform enemy)
    {
        myEnemy = enemy;
    }

    public void Exit()
    {
        throw new System.NotImplementedException();
    }

    public void OnCollisionEnter2D(Collider2D other)
    {
        
    }

    public void OnTriggerEnter(Collider2D other)
    {
        if (other.gameObject.tag == "Wall")
        {
            speed *= -1;

            //currentState.
        }

        //Debug.Log("dsa"); ;
    }
}