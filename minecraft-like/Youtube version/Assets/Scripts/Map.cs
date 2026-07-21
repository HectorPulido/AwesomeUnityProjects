using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

	public GameObject prefab;

	public Vector2Int mapSize;
	public float yScale;


	void Start () 
	{
		for (int i = 0; i < mapSize.x; i++) {
			for (int j = 0; j < mapSize.y; j++) {

				float noise = Mathf.PerlinNoise ((float)i / (float) mapSize.x, 
					(float)j /(float) mapSize.y) 
					* yScale;

				for (int k = 0; k < noise; k++) {
					Instantiate (prefab, 
						new Vector3 (i, k, j), 
						Quaternion.identity, transform);
				}

			}

		}	
	}
	

}
