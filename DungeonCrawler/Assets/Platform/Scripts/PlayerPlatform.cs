using UnityEngine;
using System.Collections;

public class PlayerPlatform : MonoBehaviour
{
    public PlayerStats MyPlayerStats;

    private Rigidbody2D myRigidbody2D;
    private BoxCollider2D myBoxCollider2D;
    private Animator myAnimator;

    [SerializeField]
    private LayerMask myLayerMask;

    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float jumpForce;

    private bool isGrounded;
    private bool canDoubleJump;
    private bool facingRight = true;


    void Start()
    {
        MyPlayerStats = GameData.MyPlayerStats; // wczytywanie statystyk

        myBoxCollider2D = GetComponent<BoxCollider2D>();
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
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
        myAnimator.SetFloat("Speed", Mathf.Abs(horizontal));

        myRigidbody2D.velocity = new Vector2(horizontal * movementSpeed, myRigidbody2D.velocity.y);

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
            facingRight = !facingRight;
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        }
    }

    //void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;

    //    Gizmos.DrawRay(myBoxCollider2D.bounds.min, Vector2.down);
    //    Gizmos.DrawRay(new Vector2(myBoxCollider2D.bounds.center.x, myBoxCollider2D.bounds.min.y), Vector2.down);
    //    Gizmos.DrawRay(new Vector2(myBoxCollider2D.bounds.max.x, myBoxCollider2D.bounds.min.y), Vector2.down);
    //}
}
