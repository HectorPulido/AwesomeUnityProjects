using UnityEngine;

public class Candy : MonoBehaviour
{
    public float force = 10f;
    public Vector2 scale = new(0.5f, 1.5f);
    public Rigidbody2D rb;
    public GameObject thrower;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == thrower)
            return;

        var avatar = other.GetComponent<Avatar>();

        if (avatar == null)
            return;

        avatar.transform.localScale = scale;
        Destroy(gameObject);

        avatar.StartCoroutine(WaitForCallback.WaitForSecondsCallback(20f, () => {
            avatar.transform.localScale = avatar.originalScale;
        }));
    }

    public void Throw(Vector2 direction, GameObject thrower)
    {
        this.thrower = thrower;
        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }
}
