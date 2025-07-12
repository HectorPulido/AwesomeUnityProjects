using System.Collections;
using UnityEngine;

public class LapManager : MonoBehaviour
{
	public Collider2D[] waypoints;
	public int Laps;
	public Racer[] racers;
	public static LapManager singleton;

	private int PlayerPosition
	{
		get
		{
			for (int i = 0; i < racers.Length; i++)
			{
				if (racers[i].name == "Player")
				{
					return i;
				}
			}
			return -1;
		}
	}

	private void Start()
	{

		if (singleton != null)
		{
			Destroy(this);
			return;
		}
		singleton = this;

		StartCoroutine(UpdateListCoroutine());

	}
	private IEnumerator UpdateListCoroutine()
	{
		while (true)
		{
			yield return new WaitForSeconds(0);
			UpdateList();
		}
	}
	public void UpdateList()
	{
		bool sw = true;
		while (sw)
		{
			sw = false;
			for (int i = 0; i < racers.Length - 1; i++)
			{
				if (racers[i].Lap < racers[i + 1].Lap)
				{
					(racers[i + 1], racers[i]) = (racers[i], racers[i + 1]);
					sw = true;
				}
				else if (racers[i].Lap == racers[i + 1].Lap)
				{
					if (racers[i].currentCheckpoint < racers[i + 1].currentCheckpoint)
					{
						(racers[i + 1], racers[i]) = (racers[i], racers[i + 1]);
						sw = true;
					}
					else if (racers[i].checkpoints == racers[i + 1].checkpoints)
					{
						if (racers[i].NextChekpointDistance > racers[i + 1].NextChekpointDistance)
						{
							(racers[i + 1], racers[i]) = (racers[i], racers[i + 1]);
							sw = true;
						}
					}
				}
			}
		}

		UiManager.singleton.UpdatePosition(PlayerPosition + 1, racers.Length);
	}
}
