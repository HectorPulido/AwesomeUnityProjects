using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Ball"))
        {
            col.SendMessage("InverRbVelocityY");

            Destroy(gameObject);
        }
    }
}
