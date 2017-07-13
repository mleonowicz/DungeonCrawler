using UnityEngine;
using System.Collections;

public class PlayerPlatform : CharacterPlatform
{
    public PlayerStats MyPlayerStats;
    public Animator myAnimator;

    [SerializeField]
    private LayerMask myLayerMask;

    [SerializeField]
    private float jumpForce;

    private bool isGrounded;
    private bool canDoubleJump;

    private bool canMove;
    void Start()
    {
        MyPlayerStats = GameData.MyPlayerStats; // wczytywanie statystyk
        myAnimator = GetComponent<Animator>();
        base.Start();

        canMove = true;
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        
        Flip(horizontal);
        HandleInput(horizontal);

        //if (Input.GetKeyDown(KeyCode.S))
        //    myRigidbody2D.AddForce(Vector2.up * 1300, ForceMode2D.Impulse);
    }

    void FixedUpdate()
    {
        GroundCheck();
    }

    void HandleInput(float horizontal)
    {
        myAnimator.SetFloat("Speed", Mathf.Abs(horizontal));
        
        if (canMove)
            myRigidbody2D.velocity = new Vector2(horizontal * MyPlayerStats.MovementSpeeed, myRigidbody2D.velocity.y);

        if (Input.GetKeyDown(KeyCode.X)) // jumping and double jumping
        {
            if (isGrounded || canDoubleJump)
            {
                myRigidbody2D.velocity = new Vector2(myRigidbody2D.velocity.x, 0);
                var jForce = jumpForce;

                if (canDoubleJump)
                {
                    jForce /= 1.1f;
                    canDoubleJump = false;
                }

                myRigidbody2D.AddForce(Vector2.up * jForce, ForceMode2D.Impulse);
            }
        }

        if (Input.GetKeyDown(KeyCode.C) && !myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            myAnimator.SetTrigger("Attack");
        }

    }

    void GroundCheck()
    {
        //Debug.Log(myRigidbody2D.position);
        //Debug.Log(transform.position.x - myBoxCollider2D.size.x);

        myAnimator.SetBool("Jumping", false);

        if (Physics2D.Raycast(myBoxCollider2D.bounds.min, Vector2.down, 0.1f, myLayerMask) ||
            Physics2D.Raycast(new Vector2(myBoxCollider2D.bounds.center.x, myBoxCollider2D.bounds.min.y), Vector2.down,
                0.1f, myLayerMask) ||
            Physics2D.Raycast(new Vector2(myBoxCollider2D.bounds.max.x, myBoxCollider2D.bounds.min.y), Vector2.down,
                0.1f, myLayerMask))
        {
            isGrounded = true;
            canDoubleJump = true;
        }

        else
        {
            isGrounded = false;
            myAnimator.SetBool("Jumping", true);
        }
    }

    void Flip(float horizontal)
    {
        if ((horizontal > 0 && !facingRight || horizontal < 0 && facingRight))
        {
            ChangeDirection();
        }
    }

    //void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;

    //    Gizmos.DrawRay(myBoxCollider2D.bounds.min, Vector2.down);
    //    Gizmos.DrawRay(new Vector2(myBoxCollider2D.bounds.center.x, myBoxCollider2D.bounds.min.y), Vector2.down);
    //    Gizmos.DrawRay(new Vector2(myBoxCollider2D.bounds.max.x, myBoxCollider2D.bounds.min.y), Vector2.down);
    //}

    private void TakeDamage(int d)
    {
        MyPlayerStats.HP -= d;

        if (MyPlayerStats.HP <= 0)
            Debug.Log("Przegrana");
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Enemy")
        {
            var x = coll.transform.position.x - transform.position.x;
            TakeDamage(coll.transform.parent.GetComponent<EnemyPlatform>().MyEnemyStats.Damage);
            StartCoroutine(Knockback(x));
        }
    }

    IEnumerator Knockback(float x)
    {
        canMove = false;

        var v = x < 0 ? Vector2.right : Vector2.left;

        myRigidbody2D.AddForce((v + Vector2.up) * jumpForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.2f);
        canMove = true;
    }
}
