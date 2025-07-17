using UnityEngine;

public class ScreenBarrier : MonoBehaviour
{
    public Vector2 direction;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("alien"))
            return;

        AlienManager.singleton.ChangeDirection(direction);
    }
}