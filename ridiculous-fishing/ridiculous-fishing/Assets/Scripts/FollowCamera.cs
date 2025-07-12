using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform pivot;
    private float offsetY;
    private void Start()
    {
        offsetY = transform.position.y - pivot.position.y;
    }

    private void LateUpdate()
    {
        if (pivot.position.y >= 0)
            return;
        transform.position = new Vector3(transform.position.x, pivot.position.y + offsetY, transform.position.z);
    }
}
