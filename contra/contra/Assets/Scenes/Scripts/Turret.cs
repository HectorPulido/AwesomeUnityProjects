using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
	public float minDistance;
	public Transform canon;
	public GameObject prefab;
	public float rate;
	public float bulletSpeed;
	public int lifes;
	public GameObject explosion;

	private Transform player;

	private void Start()
	{
		player = GameObject.FindWithTag("Player").transform;
		InvokeRepeating(nameof(Shoot), rate, rate);
	}

	private Vector3 diff;
	private void Update()
	{
		diff = player.position - transform.position;
		var diffNorm = diff.normalized;
		float rot_z = Mathf.Atan2(diffNorm.y, diffNorm.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
	}
	private void Shoot()
	{
		if (diff.magnitude > minDistance)
		{
			return;
		}

		var b = Instantiate(prefab, canon.position, canon.rotation).GetComponent<Rigidbody2D>();
		b.AddForce(canon.right * bulletSpeed, ForceMode2D.Impulse);
		Destroy(b.gameObject, 10);
	}
	private void OnTriggerEnter2D(Collider2D col)
	{
		if (!col.CompareTag("Bullet"))
			return;

		Destroy(col.gameObject);
		lifes--;

		if (lifes <= 0)
		{
			Destroy(gameObject);
			Destroy(Instantiate(explosion, transform.position, Quaternion.identity), 1.2f);
		}

	}
}
