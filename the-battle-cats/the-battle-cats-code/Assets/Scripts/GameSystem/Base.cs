using System.Collections;
using UnityEngine;
using TMPro;

public class Base : MonoBehaviour
{
	public Faction faction;
	public Troop[] troops;
	public Transform pivot;
	public int money;
	public int maxMoney = 200;
	public int health;
	public bool active;
	public TMP_Text moneyTxt;

	private float[] coolDowns;

	private IEnumerator Start()
	{
		while (!active)
		{
			yield return null;
		}

		coolDowns = new float[troops.Length];
		while (true)
		{
			for (int i = 0; i < coolDowns.Length; i++)
			{
				coolDowns[i] -= 0.1f;
			}

			if (money < maxMoney)
			{
				money++;
				if (moneyTxt != null)
					moneyTxt.text = money + "/" + maxMoney + "$";
			}
			yield return new WaitForSeconds(0.1f);
		}
	}


	public void Instantiate(int id)
	{
		if (!active)
			return;
		if (troops[id].price > money)
			return;
		if (coolDowns[id] >= 0)
			return;
		coolDowns[id] = troops[id].coolDown;
		money -= troops[id].price;
		Instantiate(troops[id], pivot.position, Quaternion.identity);
	}
}
