﻿using System.Collections;
using UnityEngine;

public class WormyManager : MonoBehaviour
{
    private Wormy[] wormies;
    public Transform wormyCamera;

    public static WormyManager singleton;

    private int currentWormy;

    void Start()
    {
        if (singleton != null)
        {
            Destroy(gameObject);
            return;
        }

        singleton = this;

        wormies = FindObjectsByType<Wormy>(FindObjectsSortMode.InstanceID);
        wormyCamera = Camera.main.transform;

        for (int i = 0; i < wormies.Length; i++)
        {
            wormies[i].wormId = i;
        }
    }

    public void NextWorm()
    {
        StartCoroutine(NextWormCoroutine());
    }

    public IEnumerator NextWormCoroutine()
    {
        var nextWorm = currentWormy + 1;
        currentWormy = -1;

        yield return new WaitForSeconds(2);

        currentWormy = nextWorm;
        if (currentWormy >= wormies.Length)
        {
            currentWormy = 0;
        }

        Camera.main.orthographicSize = 2f;
        wormyCamera.SetParent(wormies[currentWormy].transform);
        wormyCamera.localPosition = Vector3.zero + Vector3.back * 10;
    }


    public bool IsMyTurn(int i)
    {
        return i == currentWormy;
    }

}
