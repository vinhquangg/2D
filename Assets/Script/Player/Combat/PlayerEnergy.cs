using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergy : MonoBehaviour
{
    private Slider energySlider;
    private PlayerCombat playerCombat;
    private float maxEnergy = 100f;

    private void Awake()
    {
        playerCombat = GetComponent<PlayerCombat>();

        GameObject playerUI = GameObject.FindGameObjectWithTag("PlayerUI");

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
            Debug.LogError("Không tìm thấy PlayerUI (tag PlayerUI).");
        }
    }

    private void Start()
    {
        playerCombat.currentEnergy = maxEnergy;
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
