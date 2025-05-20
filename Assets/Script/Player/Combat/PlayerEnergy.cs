using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergy : MonoBehaviour
{
    private Slider energySlider;
    private PlayerCombat playerCombat;
    private float maxEnergy = 100f;

    private void Start()
    {
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

        if (playerUI != null)
        {
            Transform energySliderTransform = playerUI.transform.Find("EnergyContainer/Slider");

            if (energySliderTransform != null)
            {
                energySlider = energySliderTransform.GetComponent<Slider>();
            }
            else
            {
                Debug.LogError("Không tìm thấy EnergySlider trong EnergyContainer.");
            }
        }
        else
        {
            Debug.LogError("PlayerUI không xuất hiện sau 1 giây.");
        }
        UpdateEnergySlider();
    }

    public void AddEnergy(float energy)
    {
        playerCombat.currentEnergy += energy;
        if (playerCombat.currentEnergy > maxEnergy)
        {
            playerCombat.currentEnergy = maxEnergy;
        }
        UpdateEnergySlider();
    }

    public void UseEnergy(float amount)
    {
        playerCombat.currentEnergy -= amount;
        if (playerCombat.currentEnergy < 0)
        {
            playerCombat.currentEnergy = 0;
        }
        UpdateEnergySlider();
    }

    public bool HasEnoughEnergy(float amount)
    {
        return playerCombat.currentEnergy >= amount;
    }

    public float GetMaxEnergy()
    {
        return maxEnergy;
    }

    public void UpdateEnergySlider()
    {
        if (energySlider != null)
        {
            energySlider.value = playerCombat.currentEnergy / maxEnergy;
        }
    }
}
