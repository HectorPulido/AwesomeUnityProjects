using UnityEngine;
using UnityEngine.UI;


public class CanvasManager : MonoBehaviour
{

	public Text scoreText;
	public GameObject shop;
	public Transform shopLayout;
	public CardButton cardButtonPrefab;

	private void Start()
	{
		UpdateShop();
	}

	private void UpdateShop()
	{
		GlobalGameManager ggm = GlobalGameManager.singleton;

		scoreText.text = ggm.score.ToString();

		foreach (Transform item in shopLayout)
		{
			Destroy(item.gameObject);
		}

		for (int i = 0; i < ggm.sellingCats.Count; i++)
		{
			var go = Instantiate(cardButtonPrefab, shopLayout);
			int u = i;

			go.SetCard(ggm.sellingCats[u].Card, () =>
			{
				if (ggm.score >=
					ggm.sellingCats[u].price)
				{
					ggm.score -=
						ggm.sellingCats[u].price;

					ggm.allyTroops.Add(ggm.sellingCats[u]);

					ggm.sellingCats.RemoveAt(u);
					UpdateShop();
				}

			},
				ggm.sellingCats[u].price.ToString());
		}
	}

	public void GoToShop()
	{
		shop.SetActive(!shop.activeInHierarchy);
	}
	// public void GoToBattle()
	// {
	// 	GlobalGameManager.singleton.GoToScene("Battle");
	// }
}
