using UnityEngine;

[RequireComponent(typeof(Auto))]
public class RivalCar : MonoBehaviour
{
	private Auto auto;
	public int currentWaypoint;

	public Transform[] waypoints;

	private void Start()
	{
		auto = GetComponent<Auto>();
	}
	private void Update()
	{
		auto.accel = true;

		Vector3 targetPos = waypoints[currentWaypoint].position;
		Vector2 toTarget = (targetPos - transform.position).normalized;

		float desiredRot = -Mathf.Atan2(toTarget.y, toTarget.x) * Mathf.Rad2Deg + 90f;

		float smoothRot = Mathf.MoveTowardsAngle(
			auto.rotation,
			desiredRot,
			auto.steerSpeed * Time.deltaTime
		);
		auto.rotation = (int)smoothRot;

		auto.UpdateAuto();

		if (Vector2.Distance(targetPos, transform.position) < 1f)
		{
			currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
		}
	}
}
