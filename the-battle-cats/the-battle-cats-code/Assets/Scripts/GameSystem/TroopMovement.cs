using UnityEngine;

public class TroopMovement : MonoBehaviour
{
	public float velocity = 10;
	public int damage;
	public float candency = 1;
	public LayerMask layer;
	public GameObject prefabMario;

	private Troop tr;
	private Vector2 fwd;
	private Vector3 fwd3;
	private RaycastHit2D rh;
	private bool fighting = false;

	private void Start()
	{
		tr = GetComponent<Troop>();
		fwd = tr.faction == Faction.ours ? Vector2.left : Vector2.right;
		fwd3 = tr.faction == Faction.ours ? Vector3.left : Vector3.right;
		InvokeRepeating(nameof(Fight), candency, candency);
	}

	private void Update()
	{
		rh = Physics2D.Raycast(transform.position, fwd, 0.4f, layer);
		fighting = rh.collider != null;

		if (!fighting)
		{
			transform.position += Time.deltaTime * velocity * fwd3;
		}
		else
		{
			print(rh.collider);
		}

	}
	private void Fight()
	{
		if (!fighting)
			return;
		if (rh.collider == null)
			return;

		var b = rh.collider.GetComponent<Base>();
		var t = rh.collider.GetComponent<Troop>();

		if (b != null)
		{
			// Destroy the base
			b.health -= damage;
			if (b.health <= 0)
			{
				//WINSTAT
				GlobalGameManager.singleton.score += 1000;
				// GlobalGameManager.singleton.GoToScene("Menu");
			}


		}
		else if (t != null)
		{
			t.health -= damage;
			if (t.health <= 0)
			{
				Instantiate(prefabMario, t.transform.position, Quaternion.identity);
				Destroy(t.gameObject, 0.1f);
			}
		}
	}
}
