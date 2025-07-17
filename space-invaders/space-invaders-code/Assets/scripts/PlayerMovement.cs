using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public Transform shooter;
    public GameObject bulletPrefab;

    public float speed;
    private Rigidbody2D rb;

    public ScreenBarrier leftBarrier;
    public ScreenBarrier rightBarrier;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private float inputHorizontal;
    private void Update()
    {
        inputHorizontal = InputManager.Horizontal;

        if (InputManager.ShootDown)
        {
            var bullet = Instantiate(bulletPrefab, shooter.position, Quaternion.identity);
            Destroy(bullet, 3);
        }
    }

    private void FixedUpdate()
    {
        var direction = new Vector3(speed * inputHorizontal, 0);
        var new_position = transform.position + direction * Time.fixedDeltaTime;
        new_position.x = Mathf.Clamp(new_position.x, leftBarrier.transform.position.x, rightBarrier.transform.position.x);
        rb.MovePosition(new_position);
    }


    private void Destroyed(GameObject other)
    {
        AlienManager.singleton.StartCoroutine(nameof(AlienManager.singleton.RestartScene));
        gameObject.SetActive(false);
        Destroy(other);
        var explosion = Instantiate(
            AlienManager.singleton.explosionPrefab,
            transform.position,
            Quaternion.identity
            );
        Destroy(explosion, 1);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var bullet = other.GetComponent<Bullet>();
        if (bullet && bullet.friend)
            return;

        var barrier = other.GetComponent<ScreenBarrier>();
        if (barrier)
            return;


        Destroyed(other.gameObject);
    }
}