using UnityEngine;

public class PlayerSkill : MonoBehaviour
{

    public GameObject skillImage;  
    public GameObject blastWavePrefab;  
    public float skillEnergyCost=10f;
    private Animator skillAnimator;  
    private PlayerEnergy playerEnergy; 

    private float delayBeforeNextCast = 1f;
    private bool nextCastTime = true;

    private void Awake()
    {
        skillAnimator = skillImage.GetComponent<Animator>();
        playerEnergy = GetComponent<PlayerEnergy>();
    }

    void Start()
    {
        

    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && nextCastTime)
        {
            ActivateSkill();
        }
    }

    void ActivateSkill()
    {
        if (playerEnergy.HasEnoughEnergy(skillEnergyCost))
        {
            nextCastTime = false;

            if (skillAnimator != null)
            {
                skillAnimator.SetTrigger("PlayPopup");
            }

            Instantiate(blastWavePrefab, transform.position, Quaternion.identity);

            Invoke(nameof(EnableSkill), delayBeforeNextCast);
            playerEnergy.UseEnergy(skillEnergyCost);
            playerEnergy.UpdateEnergySlider();
        }

    }

    void EnableSkill()
    {
        nextCastTime = true;
    }
}
