using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSystem : MonoBehaviour
{
    public List<GameObject> cubes;
    public List<int> quantities;
    public int id;
    [Space]
    public GameObject explosionPrefab;


    RaycastHit rh;
    Ray r;
    void Update ()
    {
        //PONER BLOQUES 
        if (Input.GetKeyDown(KeyCode.Joystick1Button4))
        {
            r = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(r, out rh))
            {
                if (quantities[id] > 0)
                {
                    quantities[id]--;
                    Instantiate(cubes[id], rh.normal + rh.collider.transform.position, Quaternion.identity);
                }  

            }
        }
        //QUITAR BLOQUES
        if (Input.GetKeyDown(KeyCode.Joystick1Button5))
        {
            r = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(r, out rh))
                if (rh.collider != null)
                    if (rh.collider.GetComponent<Block>() != null)
                    {
                        var block = rh.collider.GetComponent<Block>();
                        StartCoroutine(delayedDestroy(rh.collider.gameObject, block.times));
                    }
                }
        if (Input.GetKeyUp(KeyCode.Joystick1Button5))
        {
            StopAllCoroutines();
        }
    }
    IEnumerator delayedDestroy(GameObject target, int time)
    {
        bool destroy = true;
        for (int i = 0; i < time; i++)
        {
            yield return new WaitForSeconds(0.5f);
            r = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(r, out rh))
                if (rh.collider != null)
                {
                    if (rh.collider.gameObject != target)
                        destroy = false;
                }
                else
                    destroy = false;
            else
                destroy = false;
        }
        if (destroy)
        {
            if (target.GetComponent<Block>() != null)
            {
                var block = target.GetComponent<Block>();
                quantities[block.id]++;
            }

            var go = Instantiate(explosionPrefab, target.transform.position, Quaternion.identity);
            Destroy(go, 5);
            Destroy(target);
        }

    }


}
