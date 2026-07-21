using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public int sizeX;
    public int sizeY;
    public float yScale;

    public GameObject cubePrefab;

    void Start()
    {
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                print(Mathf.PerlinNoise(i / (float)sizeX, j / (float)sizeY) * yScale);
                for (int k = 0; k < Mathf.PerlinNoise(i / (float)sizeX, j / (float)sizeY) * yScale; k++)
                {
                    Instantiate(cubePrefab, new Vector3(i, k, j), Quaternion.identity, transform);
                }

            }
        }

    }
}
