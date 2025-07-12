using UnityEngine;

[RequireComponent(typeof(Auto))]
public class PlayerController : MonoBehaviour
{
	private Auto auto;

	private void Start()
	{
		auto = GetComponent<Auto>();
	}

	private void Update()
	{
		auto.accel = Input.GetAxis("Vertical") > 0;
		auto.rotation += (int)(auto.steerSpeed * Time.deltaTime * Input.GetAxis("Horizontal"));
		auto.UpdateAuto();
	}
}
