using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerSoul : MonoBehaviour
{
    private TextMeshProUGUI soulText;
    private int currentSoul;
    private PlayerCombat playerCombat;
    private bool isUIReady = false;

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
            Transform textTransform = playerUI.transform.Find("SoulContainer/Amount");

            if (textTransform != null)
            {
                soulText = textTransform.GetComponent<TextMeshProUGUI>();
                isUIReady = true;
                SetSoul(currentSoul);
            }
            else
            {
                Debug.LogError("Không tìm thấy SoulText trong SoulContainer.");
            }
        }
        else
        {
            Debug.LogError("PlayerUI không xuất hiện sau 1 giây.");
        }
    }

    public void AddSoul(int amount)
    {
        currentSoul += amount;
        if (playerCombat == null)
            playerCombat = GetComponent<PlayerCombat>();

        if (playerCombat != null)
            playerCombat.currentSoul = currentSoul;

        UpdateSoulUI();
        //UpdateSoulUI();
    }

    public void SetSoul(int value)
    {
        currentSoul = value;
        UpdateSoulUI();
    }

    public int GetSoul()
    {
        return currentSoul;
    }

    public void UpdateSoulUI()
    {
        if (isUIReady && soulText != null)
            soulText.text = currentSoul.ToString();
    }

    public bool SpendSoul(int amount)
    {
        if (currentSoul >= amount)
        {
            currentSoul -= amount;
            UpdateSoulUI();
            return true;
        }
        else
        {
            return false;
        }
    }
}
