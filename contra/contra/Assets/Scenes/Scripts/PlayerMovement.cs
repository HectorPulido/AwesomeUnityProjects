using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
	[Header("System")]
	public float dieTime = 1f;
	public float invicibleTime = 3f;

	[Header("Control")]
	public float speed = 10;
	public float jumpSpeed = 3;
	public GameObject explosion;


	[Header("GroundCheck")]
	public Transform groundCheckPosition;
	public Vector2 groundCheckSize;

	public static PlayerMovement playerSingleton;

	private SpriteRenderer sr;
	private Rigidbody2D rb;
	private BetterJump bt;
	private Animator anim;
	private Collider2D colli;
	private bool canPlay = true;
	private float hor;
	private float ver;
	private bool ground;
	private bool jumpRequest = false;
	private bool water;
	private bool canTakeDamage = true;

	public bool IsGround
	{
		get
		{
			var cc = Physics2D.BoxCast(groundCheckPosition.position, groundCheckSize, 0, Vector2.up);

			if (cc.collider == null)
				return false;
			if (cc.collider.gameObject == gameObject)
				return false;

			return true;
		}
	}

	public bool CanMove
	{
		get
		{
			return canPlay;
		}
	}

	private void Start()
	{
		sr = GetComponent<SpriteRenderer>();
		rb = GetComponent<Rigidbody2D>();
		bt = GetComponent<BetterJump>();
		anim = GetComponent<Animator>();
		colli = GetComponent<Collider2D>();
		rb.freezeRotation = true;

		playerSingleton = this;
	}
	private void OnDrawGizmosSelected()
	{
		if (!groundCheckPosition)
			return;
		Gizmos.DrawWireCube(groundCheckPosition.position, (Vector3)groundCheckSize);
	}


	private void Update()
	{
		if (!canPlay)
			return;

		ground = IsGround;

		hor = InputManager.HorizontalAxis;
		ver = InputManager.VerticalAxis;

		anim.SetFloat("X", hor);
		anim.SetFloat("Y", ver);
		anim.SetBool("Ground", ground);
		anim.SetBool("Water", water);

		if (InputManager.JumpButtonPressed && ground)
			jumpRequest = true;

		ManageFlip();

	}

	private void FixedUpdate()
	{
		if (!canPlay)
			return;
		if (ver < 0)
		{
			jumpRequest = false;
			return;
		}

		if (jumpRequest)
		{
			jumpRequest = false;
			if (!water)
				rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
		}

		rb.linearVelocity = new Vector3(hor * speed, rb.linearVelocity.y);

		bt.ApplyBetterJump();

	}
	private void ManageFlip()
	{
		if (hor != 0)
			sr.flipX = hor < 0;
	}
	private IEnumerator DelayedEvents(System.Action ev, float time)
	{
		yield return new WaitForSeconds(time);
		ev();
	}

	private void CollideWater()
	{
		water = true;
	}

	private void CollideBullet()
	{
		if (!canTakeDamage)
		{
			return;
		}
		Destroy(Instantiate(explosion, transform.position, Quaternion.identity), 1.5f);

		canPlay = false;
		sr.enabled = false;
		Color newColor = sr.color;
		newColor.a = 0.5f;
		sr.color = newColor;
		rb.bodyType = RigidbodyType2D.Kinematic;
		colli.enabled = false;
		canTakeDamage = false;

		StartCoroutine(DelayedEvents(() =>
		{
			canPlay = true;
			sr.enabled = true;
			rb.bodyType = RigidbodyType2D.Dynamic;
			colli.enabled = true;
		}, dieTime));

		StartCoroutine(DelayedEvents(() =>
		{
			canTakeDamage = true;
			Color newColor = sr.color;
			newColor.a = 1f;
			sr.color = newColor;
		}, invicibleTime));
	}

	private void CollidePlatform()
	{
		if (!water)
			return;
		rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.CompareTag("Water"))
		{
			CollideWater();
		}
		else if (col.CompareTag("Bullet"))
		{
			CollideBullet();
		}
		else if (col.CompareTag("Platform"))
		{
			CollidePlatform();
		}
	}
	private void OnTriggerExit2D(Collider2D col)
	{
		if (col.CompareTag("Water"))
		{
			water = false;
		}
	}


}
