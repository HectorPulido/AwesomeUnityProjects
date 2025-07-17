using UnityEngine;

public class Player : MonoBehaviour
{
    public float[] yRotations;
    public float initialSpeed = 1;
    public float speedUp = 1f;
    public Rigidbody2D rb;
    public ParticleSystem explosion;

    private bool invertIndex = false;
    private float offsetRotation = 0;
    private int indexRotation = 0;
    private Vector3 initialPosition;


    private float speed;

    private void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        SetRotation(yRotations[indexRotation]);

        initialPosition = transform.position;

        speed = initialSpeed;
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space))
            return;

        speed += speedUp;

        if (invertIndex)
        {
            indexRotation--;
        }
        else
        {
            indexRotation++;
        }
        indexRotation %= yRotations.Length;

        if (indexRotation < 0)
        {
            indexRotation = yRotations.Length + indexRotation;
        }

        SetRotation(yRotations[indexRotation]);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = transform.right * speed;
    }

    private void SetRotation(float rotation)
    {
        var currentRotation = transform.eulerAngles;
        currentRotation.z = rotation + offsetRotation;
        transform.eulerAngles = currentRotation;
    }

    private void ChangeRotation()
    {
        invertIndex = !invertIndex;
        offsetRotation += 180;
        SetRotation(yRotations[indexRotation]);
    }

    private void Lose()
    {
        explosion.transform.position = transform.position;
        explosion.Play();

        invertIndex = false;
        offsetRotation = 0;
        indexRotation = 0;
        SetRotation(yRotations[indexRotation]);
        transform.position = initialPosition;

        speed = initialSpeed;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Collider"))
        {
            Lose();
            return;
        }

        if (col.CompareTag("ChangeRotation"))
        {
            ChangeRotation();
        }
    }
}
