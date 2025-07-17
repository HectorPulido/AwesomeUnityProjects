using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Global : MonoBehaviour
{
    public static Global singleton;
    public Text scoreText;
    public GameObject gameOverGameObject;
    public GameObject youWinGameObject;

    private int score;

    private void Start()
    {
        if (singleton != null)
        {
            Destroy(gameObject);
            return;
        }
        singleton = this;
        scoreText.text = "Score: " + score.ToString();
    }

    public void AddScore()
    {
        score++;
        scoreText.text = "Score: " + score.ToString();
    }

    public void YouWin()
    {
        StartCoroutine(IEYouWin());
    }
    public void GameOver()
    {
        StartCoroutine(IEGameOver());
    }
    private IEnumerator IEGameOver()
    {
        yield return new WaitForSeconds(1);
        gameOverGameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(0);
    }
    private IEnumerator IEYouWin()
    {
        yield return new WaitForSeconds(1);
        youWinGameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(0);
    }


}
