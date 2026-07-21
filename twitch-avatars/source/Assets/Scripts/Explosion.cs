using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float minTimeToExplode = 1f;
    public float maxTimeToExplode = 5f;

    public float explosionRadius = 5;
    public float liftForce = 10;
    public float power = 10.0f;
    public GameObject explosionSystem;


    [ContextMenu("Explode")]
    public void ActivateBomb()
    {
        var timeToExplode = Random.Range(minTimeToExplode, maxTimeToExplode);
        StartCoroutine(WaitForCallback.WaitForSecondsCallback(timeToExplode, () =>
        {
            Explode();
            var go = Instantiate(explosionSystem, transform.position, Quaternion.identity);
            Destroy(go, 10f);
            Destroy(gameObject);
   
        }));
    }

    void Explode()
    {
        Vector2 explosionPos = transform.position;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionPos, explosionRadius);
        foreach (Collider2D hit in colliders)
        {
            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();

            if (rb == null)
                continue;

            hit.gameObject.SendMessage("Explosion", SendMessageOptions.DontRequireReceiver);

            var dir = (Vector2)rb.transform.position - explosionPos;
            var distancia = 1 + dir.magnitude;
            var finalPower = power / distancia;
            rb.AddForce((dir.normalized + Vector2.up) * finalPower);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

}
