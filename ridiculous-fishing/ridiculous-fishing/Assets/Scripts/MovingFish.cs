using System.Collections;
using UnityEngine;

public class MovingFish : Fish
{
    public float directionChange;
    public float fishSpeed;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(Movement());
    }

    private IEnumerator Movement()
    {
        while (rb.bodyType != RigidbodyType2D.Kinematic)
        {
            transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y + 180, 0);
            rb.linearVelocity = -transform.right * fishSpeed;
            yield return new WaitForSeconds(directionChange);
        }
    }
    public override void Fished(Transform Hook)
    {
        base.Fished(Hook);
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
    }
    public override void Released()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        base.Released();
    }
}
