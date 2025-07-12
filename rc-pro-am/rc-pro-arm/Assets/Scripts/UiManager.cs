using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
	public Text textVelocity;
	public Text textPosition;
	public Text textLap;
	public GameObject gameOver;

	public Auto playerAuto;
	public Racer playerRacer;

	public static UiManager singleton;

	private void Start()
	{
		if (singleton != null)
		{
			Destroy(this);
			return;
		}
		singleton = this;

	}

	private void Update()
	{
		textVelocity.text = (int)playerAuto.currentSpeed + "MPH";
		textLap.text = "LAP: " + playerRacer.Lap + "/" + LapManager.singleton.Laps;
	}

	public void UpdatePosition(int position, int length)
	{
		textPosition.text = "POS:" + position + "/" + length;
	}

	public void ActivateGameOver()
	{
		gameOver.SetActive(true);
	}
}
