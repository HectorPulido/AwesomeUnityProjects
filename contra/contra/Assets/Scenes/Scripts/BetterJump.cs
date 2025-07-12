using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterJump : MonoBehaviour
{

	public float fallMultiplier = 2.5f;
	public float lowJumpMultiplier = 2;
	private Rigidbody2D rb;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	public void ApplyBetterJump()
	{
		if (rb.linearVelocity.y < 0)
		{
			rb.gravityScale = fallMultiplier;
			return;
		}
		else if (rb.linearVelocity.y > 0 && !InputManager.JumpButton)
		{
			rb.gravityScale = lowJumpMultiplier;
			return;
		}
		rb.gravityScale = 1;
	}
}