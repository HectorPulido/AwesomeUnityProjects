using UnityEngine;

public enum Faction
{
	enemy,
	ours
}

public class Troop : MonoBehaviour 
{
	public Sprite Card;
	public int price;
	public int health;
	public Faction faction;
	public float coolDown = 5;

	private SpriteRenderer sr;

	private void Start()
	{
		sr = GetComponent<SpriteRenderer> ();
		sr.sortingOrder = -(int)(transform.position.y * 1000);
	}
}
