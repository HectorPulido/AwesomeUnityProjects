using UnityEngine;

[System.Serializable]
public struct SpriteAngle
{
	public int angle;
	public Sprite sprite;
	public bool flipX;
}

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Auto : MonoBehaviour
{
	public float rotation;
	public bool accel;
	public float steerSpeed;
	public float maxSpeed;
	public float acceleration;

	public SpriteAngle[] spritesAngles;

	[HideInInspector] public float currentSpeed;

	private Rigidbody2D rb;
	private SpriteRenderer sr;

	private void Start()
	{
		sr = GetComponent<SpriteRenderer>();
		rb = GetComponent<Rigidbody2D>();
		rb.gravityScale = 0;
		rb.freezeRotation = true;
	}
	public void UpdateAuto()
	{
		rotation = DegreeNormalization(rotation);
		(sr.sprite, sr.flipX) = GetClosestSprite(rotation);

		int sign = accel ? 1 : -1;
		currentSpeed += sign * maxSpeed * acceleration * Time.deltaTime;
		currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
	}

	private (Sprite, bool) GetClosestSprite(float angle)
	{
		var closest = 9999f;
		var closesIds = -1;
		for (int i = 0; i < spritesAngles.Length; i++)
		{
			float distance = Mathf.Abs(angle - spritesAngles[i].angle);
			if (distance < closest)
			{
				closesIds = i;
				closest = distance;
			}
				
		}

		print(closest + "; " + angle + "; " + spritesAngles[closesIds].angle);
		return (spritesAngles[closesIds].sprite, spritesAngles[closesIds].flipX);
	}

	private void FixedUpdate()
	{
		Vector2 direction = CarDirection() * currentSpeed;
		rb.linearVelocity = direction;
	}

	private Vector2 CarDirection()
	{
		return new Vector2(Mathf.Sin(rotation * Mathf.Deg2Rad), Mathf.Cos(rotation * Mathf.Deg2Rad));
	}

	private float DegreeNormalization(float degree)
	{
		if (degree < 0)
			return DegreeNormalization(360 + degree);
		if (degree >= 360)
			return DegreeNormalization(degree - 360);
		return degree;
	}
}
