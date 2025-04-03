using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergy : MonoBehaviour
{
    [SerializeField] private Slider energySlider;
    private float maxEnergy = 100f;
    private float currentEnergy;

    private void Start()
    {
        currentEnergy = maxEnergy;
        UpdateEnergySlider();
    }
    public void AddEnergy(float energy)
    {
        currentEnergy += energy;
        if (currentEnergy > maxEnergy)
        {
            currentEnergy = maxEnergy;
        }
        energySlider.value = currentEnergy;
    }
    public void UseEnergy(float amount)
    {
        currentEnergy -= amount;
        if (currentEnergy < 0)
        {
            currentEnergy = 0;
        }
    }
    public bool HasEnoughEnergy(float amount)
{
    return currentEnergy >= amount;
}

    public float GetMaxEnergy()
    {
        return currentEnergy;
    }

    public void UpdateEnergySlider()
    {
        if (energySlider != null)
        {
            energySlider.value = currentEnergy / maxEnergy;
        }
    }
}
