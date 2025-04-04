using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerSkill : MonoBehaviour
{
    public TextMeshProUGUI cooldownText;
    public GameObject skillImage;  
    public GameObject blastWavePrefab;  
    public float skillEnergyCost=10f;
    private PlayerEnergy playerEnergy;
    public Image cooldownImage;
    private float cooldownTimer = 0f;
    private float skillCooldown = 5f;
    private bool isCooldownActive = false;
    private bool nextCastTime = true;

    private void Awake()
    {
        playerEnergy = GetComponent<PlayerEnergy>();
        cooldownText.gameObject.SetActive(false);
        cooldownImage.fillAmount = 1f;
    }

    void Start()
    {
        

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
            nextCastTime = false;
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
        nextCastTime = true;
        isCooldownActive = false;
        cooldownText.gameObject.SetActive(false);
        cooldownImage.fillAmount = 1f;
    }
}
