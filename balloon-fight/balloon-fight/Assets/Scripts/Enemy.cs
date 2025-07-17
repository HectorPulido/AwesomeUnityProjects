using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float period;
    private bool ballon = true;
    private Transform player;
    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private BoxCollider2D col;
    private Vector2 tentativeVelocity;
    private IEnumerator Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        anim.SetBool("Ballon", ballon);

        while (true)
        {
            if (!ballon)
            {
                yield return new WaitForSeconds(period);
                continue;
            }

            if (Random.value > 0.5f)
            {
                tentativeVelocity = Random.onUnitSphere;
                tentativeVelocity.y = tentativeVelocity.y < 0 ? 0 : tentativeVelocity.y;
            }
            else
            {
                tentativeVelocity = player.position - transform.position;
            }
            tentativeVelocity.Normalize();

            sr.flipX = tentativeVelocity.x > 0;
            rb.AddForce(tentativeVelocity * speed, ForceMode2D.Impulse);
            yield return new WaitForSeconds(period);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Water"))
            return;
        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!ballon)
            return;

        if (!collision.collider.CompareTag("Player"))
            return;

        if (collision.collider.transform.position.y <= transform.position.y)
            return;

        ballon = false;
        anim.SetBool("Ballon", ballon);
        rb.gravityScale = 1;
        col.enabled = false;
        Global.singleton.AddScore();
        StartCoroutine(IEDestroy());
    }

    private IEnumerator IEDestroy()
    {
        var enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (enemyCount == 1)
        {
            Global.singleton.YouWin();
        }
        yield return new WaitForSeconds(3);
        DestroyImmediate(gameObject);
    }

}
