using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEnemy : MonoBehaviour
{


	public int lifes;
	public GameObject explosion;

	public float speed;
	public Transform[] nodes;

	private Animator anim;
	private SpriteRenderer sr;
	private int currentNode = 0;
	private bool moving = true;
	private Vector3 direction;


	private void Start()
	{
		sr = GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();
	}


	private void Update()
	{
		anim.SetBool("Running", moving);
		if (!moving)
			return;

		direction = nodes[currentNode].position - transform.position;
		direction.Normalize();

		if (direction.x != 0)
			sr.flipX = direction.x > 0;

		transform.position += direction * speed * Time.deltaTime;

		if (Vector3.Distance(transform.position, nodes[currentNode].position) < 0.1f)
		{
			currentNode++;
			if (currentNode >= nodes.Length)
				currentNode = 0;

			moving = false;

			StartCoroutine(DelayedEvents(() =>
				{
					moving = true;

				}, 1));
		}
	}
	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.CompareTag("Bullet"))
		{
			Destroy(col.gameObject);
			lifes--;

			if (lifes <= 0)
			{
				Destroy(gameObject);
				Destroy(Instantiate(explosion, transform.position, Quaternion.identity), 1.2f);
			}
		}
	}
	private IEnumerator DelayedEvents(System.Action ev, float time)
	{
		yield return new WaitForSeconds(time);
		ev();
	}

}