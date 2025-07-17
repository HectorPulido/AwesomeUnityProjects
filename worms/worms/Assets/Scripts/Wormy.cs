using UnityEngine;

public class Wormy : MonoBehaviour
{
    public Rigidbody2D bulletPrefab;
    public Transform currentGun;

    public float wormySpeed = 1;
    public float maxRelativeVelocity;
    public float misileForce = 5;

    public bool IsTurn { get { return WormyManager.singleton.IsMyTurn(wormId); } }

    public int wormId;
    private WormyHealth wormyHealth;
    private SpriteRenderer ren;

    private void Start()
    {
        wormyHealth = GetComponent<WormyHealth>();
        ren = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!IsTurn)
            return;

        RotateGun();

        var hor = Input.GetAxis("Horizontal");
        var shoot = Input.GetKeyDown(KeyCode.Mouse0);
        if (hor == 0)
        {
            currentGun.gameObject.SetActive(true);

            ren.flipX = currentGun.eulerAngles.z < 180;

            if (shoot)
            {
                var p = Instantiate(bulletPrefab,
                                   currentGun.position - currentGun.right,
                                   currentGun.rotation);
                p.AddForce(-currentGun.right * misileForce, ForceMode2D.Impulse);
                WormyManager.singleton.NextWorm();
            }
        }
        else
        {
            currentGun.gameObject.SetActive(false);
            transform.position += hor * Time.deltaTime * wormySpeed * Vector3.right;
            ren.flipX = hor > 0;
        }


    }

    private void RotateGun()
    {
        var diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        currentGun.rotation = Quaternion.Euler(0f, 0f, rot_z + 180);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.magnitude > maxRelativeVelocity)
        {
            wormyHealth.ChangeHealth(-3);
            if (IsTurn)
                WormyManager.singleton.NextWorm();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Explosion"))
        {
            wormyHealth.ChangeHealth(-10);
            if (IsTurn)
                WormyManager.singleton.NextWorm();
        }

    }
}
