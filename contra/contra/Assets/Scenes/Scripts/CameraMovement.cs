using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
	public Transform target;

	private float currentX;
	private void LateUpdate()
	{
		if (target.position.x > currentX)
		{
			currentX = target.position.x;
		}
		transform.position = new Vector3(currentX, transform.position.y, transform.position.z);
	}
}
