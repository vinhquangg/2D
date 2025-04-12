using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private float currentHealth;

    public void UpdateHealthBarPlayer(float currentHealth, float maxHealth)
    {
        slider.value = currentHealth / maxHealth;
    }

    public void SetCurrentHealth(float currentHealth)
    {
        this.currentHealth = currentHealth;
    }
}
