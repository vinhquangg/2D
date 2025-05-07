using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class PlayerSkill : MonoBehaviour
{
    public GameObject playerUIPrefab;  // Reference to Player UI prefab
    private TextMeshProUGUI cooldownText;
    private Image cooldownImage;
    private GameObject skillImage;
    public GameObject blastWavePrefab;
    public float skillEnergyCost = 10f;
    private PlayerEnergy playerEnergy;
    private float cooldownTimer = 0f;
    private float skillCooldown = 5f;
    private bool isCooldownActive = false;

    private void Start()
    {
        playerEnergy = GetComponent<PlayerEnergy>();
        StartCoroutine(WaitForPlayerUI());
    }

    private IEnumerator WaitForPlayerUI()
    {
        GameObject playerUIInstance = null;

        // Đợi tối đa 1 giây (có thể chỉnh) cho tới khi tìm thấy PlayerUI
        float timeout = 1f;
        float timer = 0f;

        while (playerUIInstance == null && timer < timeout)
        {
            playerUIInstance = GameObject.FindGameObjectWithTag("PlayerUI");
            timer += Time.deltaTime;
            yield return null;
        }

        if (playerUIInstance != null)
        {
            Transform skill1 = playerUIInstance.transform.Find("SkillContainer/Skill 1");

            if (skill1 != null)
            {
                cooldownText = skill1.Find("Txt")?.GetComponent<TextMeshProUGUI>();
                cooldownImage = skill1.Find("Skill Image")?.GetComponent<Image>();
                skillImage = skill1.Find("SkillCD")?.gameObject;

                if (cooldownText != null) cooldownText.gameObject.SetActive(false);
                if (cooldownImage != null) cooldownImage.fillAmount = 1f;
            }
            else
            {
                Debug.LogError("Không tìm thấy Skill1 trong SkillContainer.");
            }
        }
        else
        {
            Debug.LogError("PlayerUI không xuất hiện sau 1 giây.");
        }
    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !isCooldownActive)
        {
            ActivateSkill();
        }
        if (isCooldownActive)
        {
            cooldownTimer += Time.deltaTime;
            float fillAmount = cooldownTimer / skillCooldown;
            float remainingTime = Mathf.Clamp(cooldownTimer, 0, skillCooldown);

            cooldownText.text = $"{remainingTime:F2}";
            cooldownImage.fillAmount = fillAmount;

            if (cooldownTimer >= skillCooldown)
            {
                EnableSkill();
            }
        }
    }

    void ActivateSkill()
    {
        if (playerEnergy.HasEnoughEnergy(skillEnergyCost))
        {
            isCooldownActive = true;
            cooldownTimer = 0f;
            cooldownText.gameObject.SetActive(true);
            cooldownImage.fillAmount = 0f;

            Instantiate(blastWavePrefab, transform.position, Quaternion.identity);

            Invoke(nameof(EnableSkill), skillCooldown);
            playerEnergy.UseEnergy(skillEnergyCost);
            playerEnergy.UpdateEnergySlider();
        }
    }

    void EnableSkill()
    {
        isCooldownActive = false;
        cooldownText.gameObject.SetActive(false);
        cooldownImage.fillAmount = 1f;
    }
}
