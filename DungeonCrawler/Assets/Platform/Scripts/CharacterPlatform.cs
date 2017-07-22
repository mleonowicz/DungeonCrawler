using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterPlatform : MonoBehaviour
{
    public float JumpForce;

    public Rigidbody2D MyRigidbody2D;
    public BoxCollider2D MyBoxCollider2D;
    

    public LayerMask MyLayerMask;
    public bool FacingRight = true;
    public bool IsGrounded;
    public Collider2D lastPlatformCollider;

    public void Start()
    {
        MyBoxCollider2D = GetComponent<BoxCollider2D>();
        MyRigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void ChangeDirection()
    {
        FacingRight = !FacingRight;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
    }

    public bool GroundCheckRayCast()
    {
        return Physics2D.Raycast(MyBoxCollider2D.bounds.min, Vector2.down, 0.1f, MyLayerMask) ||
                Physics2D.Raycast(new Vector2(MyBoxCollider2D.bounds.center.x, MyBoxCollider2D.bounds.min.y),
                    Vector2.down,
                    0.1f, MyLayerMask) ||
                Physics2D.Raycast(new Vector2(MyBoxCollider2D.bounds.max.x, MyBoxCollider2D.bounds.min.y), Vector2.down,
                    0.1f, MyLayerMask);
    }

    public void JumpDown()
    {
        if (IsGrounded)
        {
            Physics2D.IgnoreCollision(MyBoxCollider2D, lastPlatformCollider, true);

            Invoke("ActivatePlatform", 0.2f);
        }
    }

    void ActivatePlatform()
    {
        Physics2D.IgnoreCollision(MyBoxCollider2D, lastPlatformCollider, false);
    }
}
