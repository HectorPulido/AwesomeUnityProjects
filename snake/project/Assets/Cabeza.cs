using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cabeza : MonoBehaviour {

    enum dir
    {
        up,
        down,
        right,
        left
    }
        
    dir direction;

    public List<Transform> tail;
    public float updateRate = 0.5f;
    public float stepRate;

    public GameObject tailPrefab;

    public Vector2 verticalLimits;
    public Vector2 horizontalLimits;

    void Start ()
    {
        InvokeRepeating("Move", 1, updateRate);
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            direction = dir.left;        
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            direction = dir.up;        
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            direction = dir.right;    
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))       
            direction = dir.down;
        
    }

    void Move()
    {
        lastPos = transform.position;

        Vector3 nextPos = Vector3.zero;

        if(direction == dir.down)
            nextPos = Vector2.down;
        else if(direction == dir.left)
            nextPos = Vector2.left;
        else if (direction == dir.up)
            nextPos = Vector2.up;
        else if (direction == dir.right)
            nextPos = Vector2.right;

        nextPos *= stepRate;
        transform.position += nextPos;

        MoveTail();
    }

    Vector3 lastPos;
    void MoveTail()
    {
        for (int i = 0; i < tail.Count; i++)
        {
            Vector3 temp = tail[i].position;
            tail[i].position = lastPos;
            lastPos = temp;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Block"))
        {
            print("Juego Terminado " + col.name);
            
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
        else if (col.CompareTag("Food"))
        {
            tail.Add(Instantiate(tailPrefab, tail[tail.Count - 1].position, Quaternion.identity).transform);
            col.transform.position = new Vector2(Random.Range(horizontalLimits.x, horizontalLimits.y), Random.Range(verticalLimits.x, verticalLimits.y));
        }
    }

}
