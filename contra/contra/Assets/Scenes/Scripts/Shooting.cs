using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
	public Transform canon0;
	public Transform canon45;
	public Transform canon90;
	public Transform canon135;
	public Transform canon180;
	public Transform canon225;
	public Transform canon315;

	public GameObject bulletPrefab;
	public float bulletSpeed;

	private SpriteRenderer r;
	private Dictionary<(int, int, bool), Transform> shootDict;
	private void Start()
	{
		r = GetComponent<SpriteRenderer>();

		shootDict = new Dictionary<(int v, int h, bool flip), Transform>
		{
			{ (1, 1, false), canon45 },
			{ (1, -1, false), canon45 },
			{ (1, 0, false), canon90 },
			{ (-1, 1, false), canon315 },
			{ (-1, -1, false), canon315 },
			{ (0, 1, false), canon0 },
			{ (0, -1, false), canon0 },
			{ (1, 1, true), canon135 },
			{ (1, -1, true), canon135 },
			{ (1, 0, true), canon90 },
			{ (-1, 1, true), canon225 },
			{ (-1, -1, true), canon225 },
			{ (0, 1, true), canon180 },
			{ (0, -1, true), canon180 },
			{ (0, 0, false), canon0 },
			{ (0, 0, true), canon180 },
			{ (-1, 0, false), canon0 },
			{ (-1, 0, true), canon180 },
		};
	}

	private void Update()
	{
		if (!InputManager.AttackButtonPressed)
			return;
		int v = (int)InputManager.VerticalAxis;
		int h = (int)InputManager.HorizontalAxis;
		print(v + ", " + h);
		bool flip = r.flipX;

		var key = (v, h, flip);
		if (shootDict.TryGetValue(key, out Transform cannon))
		{
			Shoot(cannon);
		}
	}
	private void Shoot(Transform canon)
	{
		var b = Instantiate(bulletPrefab, canon.position, canon.rotation).GetComponent<Rigidbody2D>();
		b.AddForce(canon.right * bulletSpeed, ForceMode2D.Impulse);
		Destroy(b.gameObject, 10);
	}
}
