using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    private float health = 100f;
    public Vector3 respawnPosition;
    public float respawnDelay = 5f;

    public Image healthBar;

    public void TakeDamage(float amount)
    {
        health -= amount;
        UpdateHealthBar();
        if (health <= 0f)
        {
            Die();
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = health / 100f; // 100 max gesundheit
        }
    }

    void Die()
    {
        healthBar.gameObject.SetActive(false);
        gameObject.SetActive(false);
        Invoke(nameof(Respawn), respawnDelay);
    }

    void Respawn()
    {
        health = 100f; 
        transform.position = respawnPosition;
        gameObject.SetActive(true);
        healthBar.gameObject.SetActive(true);
        UpdateHealthBar(); // healthbar bei respawn zurücksetzen
    }
}