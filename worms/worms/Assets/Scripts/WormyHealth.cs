using UnityEngine;
using UnityEngine.UI;

public class WormyHealth : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public Text txtHealth;

    private void Start()
    {
        health = maxHealth;
        txtHealth.text = health.ToString();
    }

    public void ChangeHealth(int change)
    {
        health += change;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        txtHealth.text = health.ToString();
    }
}
