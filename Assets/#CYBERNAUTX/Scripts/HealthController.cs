using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class HealthController : MonoBehaviour
{
    [SerializeField]
    private HealthBar healthBar;

    [SerializeField]
    private float maxHealth = 100f;

    [SerializeField]
    private float minHealth = 0f;

    [SerializeField]
    private float currentHealth;

    private void Start()
    {
        SetHealth(maxHealth);

        if (healthBar != null)
            healthBar.Initialize(minHealth, maxHealth, currentHealth);
    }

    public void Damage(float value) => SetHealth(currentHealth - value);


    public void Heal(float value) => SetHealth(currentHealth + value);

    public void Die()
    {
        currentHealth = minHealth;
    }

    [Button]
    public void SetHealth(float value)
    {
        currentHealth = Mathf.Clamp(value, minHealth, maxHealth);

        if (healthBar != null)
            healthBar.SetValue(currentHealth, 0.5f);

        if (currentHealth <= minHealth)
            Die();
    }
}
