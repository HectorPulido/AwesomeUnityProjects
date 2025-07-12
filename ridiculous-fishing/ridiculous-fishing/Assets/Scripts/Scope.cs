using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Scope : MonoBehaviour
{
    public float scopeSpeed;
    public GameObject bloodParticles;
    public GameObject deadFish;

    private Rigidbody2D rb;
    private readonly List<Collider2D> fishes = new();
    private float h, v;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        if (!Input.GetKeyDown(KeyCode.Space))
            return;
        Shoot();

    }

    private void Shoot()
    {
        if (this.fishes.Count <= 0)
            return;

        var fishes = this.fishes.ToList();
        foreach (var fish in fishes)
        {
            if (!fish.gameObject.activeInHierarchy)
                continue;
            Instantiate(bloodParticles, transform.position, bloodParticles.transform.rotation);
            Instantiate(deadFish, fish.transform.position, fish.transform.rotation);
            fish.gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        float tempVelY = scopeSpeed * v;
        float tempVelX = scopeSpeed * h;

        rb.linearVelocity = new Vector2(tempVelX, tempVelY);
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Fish"))
            return;

        fishes.Add(col);
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (!col.CompareTag("Fish"))
            return;
        if (!fishes.Contains(col))
            return;

        fishes.Remove(col);
    }
}
