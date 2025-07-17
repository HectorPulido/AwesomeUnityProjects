using UnityEngine;

public class ConstantMovement : MonoBehaviour
{
    public Vector3 direction;
    public float speed;

    private void Update()
    {
        transform.position += speed * Time.deltaTime * direction;
    }
}
