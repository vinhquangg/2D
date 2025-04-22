using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private Slider slider;
    private float currentHealth;

    private void Awake()
    {
        GameObject playerUI = GameObject.FindGameObjectWithTag("PlayerUI");

        if (playerUI != null)
        {
            Transform sliderTransform = playerUI.transform.Find("HealthContainer/Slider");

            if (sliderTransform != null)
            {
                slider = sliderTransform.GetComponent<Slider>();
            }
            else
            {
                Debug.LogError("Không tìm thấy Slider trong HealContainer.");
            }
        }
        else
        {
            Debug.LogError("Không tìm thấy PlayerUI có tag PlayerUI.");
        }
    }

    public void UpdateHealthBarPlayer(float currentHealth, float maxHealth)
    {
        if (slider != null)
            slider.value = currentHealth / maxHealth;
    }

    public void SetCurrentHealth(float currentHealth)
    {
        this.currentHealth = currentHealth;
    }
}
