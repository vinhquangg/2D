using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private Slider slider;
    private float currentHealth;
    private PlayerStateMachine player;
    private PlayerCombat playerCombat;

    private void Awake()
    {
<<<<<<< HEAD
        player = GetComponent<PlayerStateMachine>();
        playerCombat = GetComponent<PlayerCombat>();
        StartCoroutine(WaitForPlayerUI());
    }

    private IEnumerator WaitForPlayerUI()
    {
        GameObject playerUI = null;
        float timeout = 1f;
        float timer = 0f;

        while (playerUI == null && timer < timeout)
        {
            playerUI = GameObject.FindGameObjectWithTag("PlayerUI");
            timer += Time.deltaTime;
            yield return null;
        }
=======
        GameObject playerUI = GameObject.FindGameObjectWithTag("PlayerUI");
>>>>>>> 908abd4085243b70d8b93f5cfa633115f6e4ff13

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

    public void FullRestore(float maxHealth)
    {
        currentHealth = maxHealth;
        if (playerCombat != null)
        {
            playerCombat.currentHealth = maxHealth;
        }

        UpdateHealthBarPlayer(currentHealth, maxHealth);
    }


}
