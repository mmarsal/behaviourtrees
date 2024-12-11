using UnityEngine;
using UnityEngine.UI;
using BehaviorDesigner.Runtime;

public class Target : MonoBehaviour
{
    public float health = 1f;

    public Image healthBar;
    public SharedTransform playerTransform;

    private void Start()
    {
        UpdateHealthBar();
        playerTransform.Value = GameObject.FindWithTag("Player").transform;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        UpdateHealthBar();
    }

    public void RestoreHealth()
    {
        health = 100f;
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        healthBar.fillAmount = health / 100f; // 100 max Gesundheit
    }
}