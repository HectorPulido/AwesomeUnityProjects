using System.Collections.Generic;
using UnityEngine;

public class FirstWaypoint : MonoBehaviour
{
	public Racer[] racers;
	readonly List<Collider2D> racerList = new();
	private bool ready;

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (ready)
			return;

		if (!col.CompareTag("Racer"))
			return;

		if (racerList.Contains(col))
		{
			racerList.Remove(col);
			return;
		}

		racerList.Add(col);
	}
	private void OnTriggerExit2D(Collider2D col)
	{
		if (racerList.Count != racers.Length)
			return;
		tag = "Waypoint";
		ready = true;
	}
}
