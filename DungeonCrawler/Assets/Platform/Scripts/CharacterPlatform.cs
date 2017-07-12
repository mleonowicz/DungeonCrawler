using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterPlatform : MonoBehaviour
{
    public Rigidbody2D myRigidbody2D;
    public BoxCollider2D myBoxCollider2D;

    public bool facingRight = true;

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
}
