using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergy : MonoBehaviour
{
    [SerializeField] private Slider energySlider;
    private PlayerCombat playerCombat;
    private float maxEnergy = 100f;
    //public float currentEnergy { get; private set; }
    //
    private void Awake()
    {
        playerCombat = GetComponent<PlayerCombat>();
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
        energySlider.value = playerCombat.currentEnergy/maxEnergy;
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
