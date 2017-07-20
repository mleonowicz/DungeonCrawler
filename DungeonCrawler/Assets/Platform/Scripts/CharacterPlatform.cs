using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterPlatform : MonoBehaviour
{
    public float JumpForce;

    public Rigidbody2D myRigidbody2D;
    public BoxCollider2D myBoxCollider2D;
    

    public LayerMask myLayerMask;
    public bool facingRight = true;
    public bool isGrounded;

    public void Start()
    {
        myBoxCollider2D = GetComponent<BoxCollider2D>();
        myRigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void ChangeDirection()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
    }

    public bool GroundCheckRayCast()
    {
        return Physics2D.Raycast(myBoxCollider2D.bounds.min, Vector2.down, 0.1f, myLayerMask) ||
                Physics2D.Raycast(new Vector2(myBoxCollider2D.bounds.center.x, myBoxCollider2D.bounds.min.y),
                    Vector2.down,
                    0.1f, myLayerMask) ||
                Physics2D.Raycast(new Vector2(myBoxCollider2D.bounds.max.x, myBoxCollider2D.bounds.min.y), Vector2.down,
                    0.1f, myLayerMask);
    }
}
