using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacketPiece : MonoBehaviour {

    public float velocity;
    public Vector2 direction;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Ball"))
        {
            col.SendMessage("SetVelocity", direction.normalized * velocity);
        }
    }
}
