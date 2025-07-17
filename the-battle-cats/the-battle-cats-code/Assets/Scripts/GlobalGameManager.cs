using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalGameManager : MonoBehaviour
{
	public static GlobalGameManager singleton;
	public List<Troop> sellingCats = new();
	public List<Troop> allyTroops = new();
	public List<Troop> enemyTroops = new();
	public int score;

	private void Awake()
	{
		if (singleton != null)
		{
			Destroy(this);
			return;
		}
		singleton = this;
		DontDestroyOnLoad(singleton);

		var bm = FindFirstObjectByType<BattleManager>();
		bm.Init(enemyTroops.ToArray(), allyTroops.ToArray());
	}

	// private void SceneLoaded(Scene scene, LoadSceneMode lsc)
	// {
	// 	if (scene.name != "Battle")
	// 		return;
	// 	var bm = FindFirstObjectByType<BattleManager>();
	// 	bm.Init(enemyTroops.ToArray(), allyTroops.ToArray());
	// }

	// public void GoToScene(string sceneToGo)
	// {
	// 	SceneManager.LoadScene(sceneToGo);
	// }

}
