using UnityEngine;
using System.Collections;

public class PlayerPlatform : CharacterPlatform
{
    public PlayerStats MyPlayerStats;
    public Collider2D AttackTrigger;

    private bool canDoubleJump;
    private bool canMove;
    private bool attacking;

    void Start()
    {
        MyPlayerStats = GameData.MyPlayerStats; // wczytywanie statystyk
        base.Start();

        canMove = true;
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        
        Flip(horizontal);
        HandleInput(horizontal);
    }

    void FixedUpdate()
    {
        GroundCheck();
    }

    void HandleInput(float horizontal)
    {
        MyAnimator.SetFloat("Speed", Mathf.Abs(horizontal));
        
        if (canMove)
            MyRigidbody2D.velocity = new Vector2(horizontal * MyPlayerStats.MovementSpeeed, MyRigidbody2D.velocity.y);

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (IsGrounded || canDoubleJump)
            {
                MyRigidbody2D.velocity = new Vector2(MyRigidbody2D.velocity.x, 0);
                var jForce = JumpForce;

                if (canDoubleJump)
                {
                    jForce /= 1.1f;
                    canDoubleJump = false;
                }

                MyRigidbody2D.AddForce(Vector2.up * jForce, ForceMode2D.Impulse);
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
            if (!attacking)                
                StartCoroutine(Attack());      

        if (Input.GetKeyDown(KeyCode.DownArrow))
            JumpDown();
    }

    void GroundCheck()
    {
        MyAnimator.SetBool("Jumping", false);

        if (GroundCheckRayCast())
        {
            IsGrounded = true;
            canDoubleJump = true;
        }
        else
        {
            IsGrounded = false;
            MyAnimator.SetBool("Jumping", true);
        }
    }

    void Flip(float horizontal)
    {
        if (horizontal > 0 && !FacingRight || horizontal < 0 && FacingRight)
            ChangeDirection();
    }

    //void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;

    //    Gizmos.DrawRay(MyBoxCollider2D.bounds.min, Vector2.down);
    //    Gizmos.DrawRay(new Vector2(MyBoxCollider2D.bounds.center.x, MyBoxCollider2D.bounds.min.y), Vector2.down);
    //    Gizmos.DrawRay(new Vector2(MyBoxCollider2D.bounds.max.x, MyBoxCollider2D.bounds.min.y), Vector2.down);
    //}

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.collider.tag == "Platform")
        {
            lastPlatformCollider = coll.collider;
        }
    }

    private IEnumerator Attack()
    {
        SetAttackValues(true);

        yield return new WaitForSeconds(0.2f);

        SetAttackValues(false);
    }

    private void SetAttackValues(bool b)
    {
        MyAnimator.SetBool("Attacking", b);
        attacking = b;
        AttackTrigger.enabled = b;
    }
    //IEnumerator Knockback(float x)
    //{
    //    canMove = false;

    //    var v = x < 0 ? Vector2.right : Vector2.left;

    //    MyRigidbody2D.AddForce((v + Vector2.up) * JumpForce / 1.5f, ForceMode2D.Impulse);

    //    yield return new WaitForSeconds(0.2f);
    //    canMove = true;
    //}
}