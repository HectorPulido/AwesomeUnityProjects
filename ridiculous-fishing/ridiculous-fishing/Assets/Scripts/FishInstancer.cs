﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishInstancer : MonoBehaviour
{
    public GameObject[] fishTypes;
    public Vector2 fishInstancePeriod;
    public Transform leftLimit;
    public Transform rightLimit;
    private IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(fishInstancePeriod.x, fishInstancePeriod.y));

            if (!isActiveAndEnabled)
            {
                continue;
            }

            var f = Instantiate(fishTypes[Random.Range(0, fishTypes.Length)]);
            f.transform.position = new Vector3(
                Random.Range(rightLimit.position.x, leftLimit.position.x),
                Random.Range(rightLimit.position.y, leftLimit.position.y),
                0) +
                transform.position;

            f.transform.position = new Vector3(f.transform.position.x, f.transform.position.y, 0);
            f.transform.localScale = Vector2.one * Random.Range(0.5f, 1.5f);
            f.transform.rotation = Quaternion.Euler(0, Random.value > 0.5 ? 0 : 180, 0);
        }
    }
}
