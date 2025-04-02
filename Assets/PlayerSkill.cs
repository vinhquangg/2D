using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    public GameObject skillImage;  // Tham chiếu tới đối tượng Skill Image chứa Animator
    public GameObject blastWavePrefab;  // Prefab của kỹ năng
    private Animator skillAnimator;  // Animator của Skill Image
    private float delayBeforeNextCast = 1f;
    private bool nextCastTime = true;

    private void Awake()
    {
        skillAnimator = skillImage.GetComponent<Animator>();
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
        nextCastTime = false;

        if (skillAnimator != null)
        {
            skillAnimator.SetTrigger("PlayPopup");
        }

        Instantiate(blastWavePrefab, transform.position, Quaternion.identity);

        Invoke(nameof(EnableSkill), delayBeforeNextCast);
    }

    void EnableSkill()
    {
        nextCastTime = true;
    }
}
