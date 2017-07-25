using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlatform : CharacterPlatform
{

    public EnemyStats MyEnemyStats;
    private IEnemyState currentState;

    public PlayerPlatform Target;

    public bool CanJump = true;
    private float attackTimer = 0;
    private bool canAttack;

    void Start()
    {
        base.Start();

        MyEnemyStats = GameData.MyEnemyStats;
        ChangeState(new PatrolState());
    }

    private void Update()
    {
        currentState.Execute();
    }

    private void FixedUpdate()
    {
        GroundCheck();
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
        return FacingRight ? Vector2.right : Vector2.left;
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        currentState.OnCollisionEnter(coll);

        if (coll.collider.tag == "Platform")
        {
            lastPlatformCollider = coll.collider;
        }
    }

    public void LookAtTarget()
    {
        if (Target != null)
        {
            float x = Target.transform.position.x - transform.position.x;

            if (FacingRight && x < 0 || !FacingRight && x > 0)
                ChangeDirection();
        }
    }

    public void Jump()
    {
        if (IsGrounded)
        {
            IsGrounded = false;

            MyRigidbody2D.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);

            StartCoroutine(JumpCooldown());
        }
    }

    public void GroundCheck()
    {
        IsGrounded = GroundCheckRayCast();
    }

    public IEnumerator JumpCooldown()
    {
        CanJump = false;

        float timeStamp = 0;

        while (timeStamp < 1f)
        {
            timeStamp += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        CanJump = true;
    }

    public void ChaseEnemy()
    {
        Move();

        if (CanJump)
        {
            if (Target.MyBoxCollider2D.bounds.min.y > MyBoxCollider2D.bounds.min.y)
            {
                if (Physics2D.Raycast(MyBoxCollider2D.bounds.center, Vector2.up, 5, MyLayerMask))
                    Jump();
            }
            else if (Target.MyBoxCollider2D.bounds.min.y < MyBoxCollider2D.bounds.min.y)
            {
                JumpDown();
                StartCoroutine(JumpCooldown());
            }
        }
    }

    public void Attack()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= MyEnemyStats.AttackSpeed)
        {
            canAttack = true;
            attackTimer = 0;
        }

        if (canAttack)
        {
            canAttack = false;
        }
    }
}