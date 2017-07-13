using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlatform : CharacterPlatform {

    public EnemyStats MyEnemyStats;
    private IEnemyState currentState;

    public GameObject Target;

    void Start ()
    {
        base.Start();
        MyEnemyStats = GameData.MyEnemyStats;
        ChangeState(new PatrolState());
    }

	private void Update ()
    {
		currentState.Execute();
	}

    public void ChangeState(IEnemyState newState)
    {
        if (currentState != null)
            currentState.Exit();

        currentState = newState;
        currentState.Enter(this);
    }

    public void Move()
    {
        transform.Translate(GetDirection() * MyEnemyStats.MovementSpeed * Time.deltaTime);
    }

    public Vector2 GetDirection()
    {
        return facingRight ? Vector2.right : Vector2.left;
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        
        currentState.OnCollisionEnter(coll);
    }

    public void LookAtTarget()
    {
        if (Target != null)
        {
            float x = Target.transform.position.x - transform.position.x;

            if (facingRight && x < 0 || !facingRight && x > 0)
                ChangeDirection();
        }
    }
}
