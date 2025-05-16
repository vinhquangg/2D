using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkillManager : MonoBehaviour
{
    private BossStateMachine bossStateMachine;
    public List<BossSkillSO> skills = new List<BossSkillSO>();  
    private Animator animator;
    private BossSummoner bossSummoner;
    private bool isCastingSkill = false;
    [SerializeField] private GameObject meteorPrefab;
    [SerializeField] private BoxCollider2D bossZoneCollider;
    [SerializeField] private GameObject buffRingEffectPrefab;
    public BossSkillSO CurrentSkill { get; private set; }

    public static BossSkillManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        bossStateMachine = GetComponent<BossStateMachine>();
        animator = GetComponent<Animator>();
        bossSummoner = GetComponent<BossSummoner>();

        GameObject zoneObj = GameObject.FindWithTag("BossZone");
        if (zoneObj != null)
        {
            bossZoneCollider = zoneObj.GetComponent<BoxCollider2D>();
        }

        if (bossStateMachine?.boss == null)
        {
            Debug.LogWarning("Boss or BossStateMachine not found on BossSkillManager.");
        }
    }

    public void UseNextSkill()
    {
        if (isCastingSkill || skills.Count == 0 || bossStateMachine == null || bossStateMachine.boss == null)
            return;

        bool isPhaseTwo = bossStateMachine.boss.isPhaseTwoActive;

        List<BossSkillSO> validSkills = skills.FindAll(skill =>
            (isPhaseTwo && (skill.skillPhase == 1 || skill.skillPhase == 2)) ||
            (!isPhaseTwo && skill.skillPhase == 1)
        );

        if (validSkills.Count > 0)
        {
            int randomIndex = Random.Range(0, validSkills.Count);
            CurrentSkill = validSkills[randomIndex];
            StartCoroutine(CastSkill(CurrentSkill));
        }
    }


    public IEnumerator CastSkill(BossSkillSO skill)
    {
        isCastingSkill = true;

        animator.Play(skill.animationName);
        ExecuteSkillEffect(skill);

        yield return new WaitForSeconds(skill.castTime);
        isCastingSkill = false;
    }


    private void ExecuteSkillEffect(BossSkillSO skill)
    {
        if (skill.skillName == "CastMeteor")
        {
            CastMeteor();
        }
        if (skill.skillName == "Summoner")
        {
            SummonEnemies();
        }
        if (skill.skillName == "Hook") 
        {
            StartHook(); 
        }
        if (skill.skillName == "BuffRing")
        {
            Instantiate(buffRingEffectPrefab, transform.position, Quaternion.identity);
        }
    }

    private void SummonEnemies()
    {
        bossSummoner.SummonEnemies();
    }

    private void CastMeteor()
    {
        Vector3 bossPos = transform.position;
        float radius = 10f;
        int meteorCount = 0;
        int maxAttempts = 30;

        while (meteorCount < 6 && maxAttempts > 0)
        {
            Vector2 randomOffset = Random.insideUnitCircle * radius;
            Vector3 meteorPos = new Vector3(bossPos.x + randomOffset.x, bossPos.y + randomOffset.y, 0);

            if (bossZoneCollider.OverlapPoint(meteorPos))
            {
                GameObject meteor = Instantiate(meteorPrefab);
                meteor.GetComponent<MeteorScript>().Initialize(meteorPos, groundY: meteorPos.y);
                meteorCount++;
            }

            maxAttempts--;
        }
    }

    private void StartHook()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        Vector3 playerPos = player.transform.position;
        Vector3 bossPos = transform.position;

        Vector3 direction = (bossPos - playerPos).normalized;

        Vector3 hookTargetPos = bossPos + (-direction * 1.5f); 

        StartCoroutine(MovePlayerToPosition(player, hookTargetPos, 0.3f));
    }


    private IEnumerator MovePlayerToPosition(GameObject player, Vector3 targetPos, float duration)
    {
        float elapsed = 0f;
        Vector3 startPos = player.transform.position;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            player.transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            yield return null;
        }

        player.transform.position = targetPos;
    }

}
