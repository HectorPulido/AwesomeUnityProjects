using UnityEngine;

public class Destroyable : MonoBehaviour
{
    private void SelfDestroy(GameObject other)
    {
        Destroy(gameObject);
        if (other != null)
            Destroy(other);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Bullet bullet = other.GetComponent<Bullet>();
        Alien alien = other.GetComponent<Alien>();
        if (!bullet && !alien)
            return;
        GameObject bulletObject = null;
        if (bullet != null)
            bulletObject = other.gameObject;
        SelfDestroy(bulletObject);
    }
}
