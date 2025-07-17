using UnityEngine;

public class Alien : MonoBehaviour
{
    public void Move(Vector3 dir)
    {
        transform.position += dir;
    }

    private void Destroyed(GameObject other)
    {
        Destroy(gameObject);
        Destroy(other);
        var explosion = Instantiate(
            AlienManager.singleton.explosionPrefab,
            transform.position,
            Quaternion.identity
            );
        Destroy(explosion, 1);

        AlienManager.singleton.moveFrequency -= AlienManager.singleton.stepFrequency;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var bullet = other.GetComponent<Bullet>();
        if (!bullet)
            return;
        if (!bullet.friend)
            return;
        Destroyed(other.gameObject);
    }
}