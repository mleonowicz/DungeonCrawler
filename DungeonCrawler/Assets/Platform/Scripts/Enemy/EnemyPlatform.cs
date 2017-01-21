using UnityEngine;
using System.Collections;

public class EnemyPlatform : MonoBehaviour
{

    private bool facingRight = false;
    private IEnemyPlatformState currentState;

    void Start()
    {
        ChangeState(new PatrolState());
        ChangeDirection();
    }

    void Update()
    {
        currentState.Execute();
    }

    void ChangeDirection()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
    }

    private void ChangeState(IEnemyPlatformState newState)
    {
        if (currentState != null)
            currentState.Exit();

        currentState = newState;
        currentState.Enter(this);
    }

    //public void OnCollisionEnter2D(Collider2D other)
    //{
        
    //    currentState.OnTriggerEnter(other);
    //}
}