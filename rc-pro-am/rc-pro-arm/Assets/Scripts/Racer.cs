using System.Collections.Generic;
using UnityEngine;

public class Racer : MonoBehaviour
{
	public int Lap;

	public Transform[] checkpoints;
	public int currentCheckpoint;
	private readonly List<Collider2D> waypoints = new();

	public float NextChekpointDistance
	{
		get
		{
			Transform nextCheckpoint;
			if (currentCheckpoint >= checkpoints.Length - 1)
				nextCheckpoint = checkpoints[0];
			else
				nextCheckpoint = checkpoints[currentCheckpoint + 1];

			return Vector3.SqrMagnitude(nextCheckpoint.position - transform.position);
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (!col.CompareTag("Waypoint"))
			return;
		if (waypoints.Contains(col))
		{
			waypoints.Remove(col);

			currentCheckpoint--;
			if (currentCheckpoint < 0)
			{
				Lap--;
				currentCheckpoint = checkpoints.Length - 1;
			}
		}
		else
		{
			waypoints.Add(col);

			currentCheckpoint++;
			if (currentCheckpoint >= checkpoints.Length)
				currentCheckpoint = 0;

			if (waypoints.Count == LapManager.singleton.waypoints.Length)
			{
				waypoints.Clear();
				Lap++;
				if (Lap >= LapManager.singleton.Laps)
				{

					UiManager.singleton.ActivateGameOver();
				}
			}
		}

	}
}
