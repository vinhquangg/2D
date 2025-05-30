﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private Slider slider;
    private float currentHealth;
    private PlayerStateMachine player;
    private PlayerCombat playerCombat;

    private void Start()
    {
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

        if (playerUI != null)
        {
            Transform sliderTransform = playerUI.transform.Find("HealthContainer/Slider");

            if (sliderTransform != null)
            {
                slider = sliderTransform.GetComponent<Slider>();

                if (playerCombat != null && player != null)
                {
                    UpdateHealthBarPlayer(playerCombat.currentHealth, player.playerData.maxHealth);
                }
            }

        }
        else
        {
            Debug.LogError("PlayerUI không xuất hiện sau 1 giây.");
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
