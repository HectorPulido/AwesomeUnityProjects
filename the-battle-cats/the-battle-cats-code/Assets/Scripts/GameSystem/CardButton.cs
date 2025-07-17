using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardButton : MonoBehaviour
{
	public Button button;
	public Image image;
	public TMP_Text text;


	public void SetCard(Sprite sprite, System.Action action, string _text)
	{
		image.sprite = sprite;
		text.text = _text;

		button.onClick.RemoveAllListeners();
		button.onClick.AddListener(action.Invoke);

	}

}
