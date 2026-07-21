using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour
{
    public float startVelocity;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation ;
        rb.gravityScale = 0;

        Setup();
    }

    // Update is called once per frame
    void Setup ()
    {
        transform.position = Vector2.zero;
        rb.velocity = new Vector2(Random.Range(-1, 2), -1) * startVelocity; 	
	}
    void SetVelocity(Vector2 vel)
    {
        rb.velocity = vel;
    }
    void InverRbVelocityY()
    {
        rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y);
    }
}
