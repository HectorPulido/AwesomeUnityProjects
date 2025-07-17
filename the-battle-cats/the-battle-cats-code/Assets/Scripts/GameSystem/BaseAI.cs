using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAI : MonoBehaviour
{
	public Base baseToControll;

	private void Start()
	{
		InvokeRepeating(nameof(SpawnEnemy), 0.5f, 0.5f);
	}

	private void SpawnEnemy()
	{
		baseToControll.Instantiate(Random.Range(0, baseToControll.troops.Length));
	}
}
