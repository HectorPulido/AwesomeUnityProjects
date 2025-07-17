using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class ListOfAliens
{
    public Alien[] aliens;
}

public class AlienManager : MonoBehaviour
{
    public static AlienManager singleton;
    public GameObject explosionPrefab;
    public GameObject bulletPrefab;
    public ListOfAliens[] aliens;
    public float moveFrequency;
    public float step = 0.5f;
    public float stepDown = 0.1f;
    public float stepFrequency = 0.005f;
    public float timeToShoot = 1;

    private Vector2 currentDirection = Vector2.right;

    private void Start()
    {
        if (singleton != null)
        {
            Destroy(this);
        }
        singleton = this;

        StartCoroutine(nameof(MoveAliens));
        StartCoroutine(nameof(AlienShoot));
    }

    private IEnumerator AlienShoot()
    {
        yield return new WaitForSeconds(timeToShoot);
        while (true)
        {
            int rx = Random.Range(0, aliens.Count());
            int ry = Random.Range(0, aliens[aliens.Count() - 1].aliens.Length);

            Alien selected_alien = aliens[rx].aliens[ry];
            if (selected_alien != null)
            {
                Instantiate(bulletPrefab, selected_alien.transform.position, Quaternion.identity);
            }
            yield return new WaitForSeconds(timeToShoot);
        }
    }



    private IEnumerator MoveAliens()
    {
        while (true)
        {
            var direction = currentDirection;
            yield return MoveAliensToDirection(direction, step);
        }
    }

    private IEnumerator MoveAliensToDirection(Vector2 direction, float step)
    {
        yield return new WaitForSeconds(moveFrequency);
        for (var i = aliens.Length - 1; i >= 0; i--)
        {
            foreach (var alien in aliens[i].aliens)
            {
                if (alien == null)
                    continue;
                alien.Move(direction * step);
            }
            yield return new WaitForSeconds(moveFrequency / 10);
        }
    }

    public void ChangeDirection(Vector2 direction)
    {
        currentDirection = direction;
        StartCoroutine(MoveAliensToDirection(Vector2.down, stepDown));
    }

    public IEnumerator RestartScene()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


}