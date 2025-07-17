using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class Player : MonoBehaviour
{
    public Transform groundPosition;
    public Vector2 groundSize;
    public float speed;
    public float jumpSpeed;

    public int ballon = 2;
    public float maxVerticalVelocity = 15;

    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private BoxCollider2D col;
    private float horizontal = 0;
    private bool jump = false;
    private bool jumpRequest;
    private bool CheckGround
    {
        get
        {
            var ground = Physics2D.BoxCast(groundPosition.position, groundSize, 0, Vector2.zero);

            if (ground.collider == null)
                return false;

            return true;
        }
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();

        anim.SetFloat("Ballons", ballon);
        rb.freezeRotation = true;
    }
    private void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        jump = Input.GetKeyDown(KeyCode.Space);

        var ground = CheckGround;
        anim.SetBool("Ground", ground);

        if (jump)
        {
            jumpRequest = true;
        }

        if (ground)
        {
            if (horizontal == 0)
            {
                anim.speed = 0;
            }
        }
        else
        {
            anim.speed = 1;
        }

        if (horizontal != 0)
        {
            sr.flipX = horizontal > 0;
            anim.speed = 1;
        }

    }
    private void FixedUpdate()
    {
        if (jumpRequest)
        {
            jumpRequest = false;
            rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        }
        var verticalVelocity = rb.linearVelocity.y > maxVerticalVelocity ? maxVerticalVelocity : rb.linearVelocity.y;
        rb.linearVelocity = new Vector3(horizontal * speed, verticalVelocity);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(groundPosition.position, groundSize);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Water"))
            return;
        Die();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Enemy"))
            return;

        if (collision.collider.transform.position.y <= transform.position.y)
            return;

        ballon--;
        anim.SetFloat("Ballons", ballon);

        if (ballon <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        col.enabled = false;
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 1;
        Global.singleton.GameOver();
        enabled = false;
    }
}
