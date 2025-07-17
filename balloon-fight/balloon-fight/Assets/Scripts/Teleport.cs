using UnityEngine;

public class Teleport : MonoBehaviour {

    public Transform otherPortal;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        collision.transform.position = new Vector2(
                            otherPortal.position.x,
                            collision.transform.position.y);

    }
}
