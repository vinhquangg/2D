using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkillManager : MonoBehaviour
{
    private BossStateMachine bossStateMachine;
    public List<BossSkillSO> skills = new List<BossSkillSO>();
    private Animator animator;
    private BossSummoner bossSummoner;
    public bool isCastingSkill { get; set; } = false;

    [SerializeField] private GameObject meteorPrefab;
    [SerializeField] private BoxCollider2D bossZoneCollider;
    [SerializeField] private GameObject bulletCirclePrefab;

    public BossSkillSO CurrentSkill { get; set; }
    public static BossSkillManager Instance { get; private set; }

    private Dictionary<BossSkillSO, float> skillCooldownTimers = new Dictionary<BossSkillSO, float>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        bossStateMachine = GetComponent<BossStateMachine>();
        animator = GetComponent<Animator>();
        bossSummoner = GetComponent<BossSummoner>();

        GameObject zoneObj = GameObject.FindWithTag("BossZone");
        if (zoneObj != null) bossZoneCollider = zoneObj.GetComponent<BoxCollider2D>();

        if (bossStateMachine?.boss == null)
            Debug.LogWarning("Boss or BossStateMachine not found on BossSkillManager.");
        foreach (var skill in skills)
        {
            skillCooldownTimers[skill] = 0f;
        }
    }

    void Update()
    {
        List<BossSkillSO> keys = new List<BossSkillSO>(skillCooldownTimers.Keys);
        foreach (var skill in keys)
        {
            if (skillCooldownTimers[skill] > 0)
            {
                skillCooldownTimers[skill] -= Time.deltaTime;
                if (skillCooldownTimers[skill] < 0)
                    skillCooldownTimers[skill] = 0;
            }
        }
    }

    public void UseNextSkill()
    {
        if (isCastingSkill || skills.Count == 0 || bossStateMachine == null || bossStateMachine.boss == null)
            return;

        bool isPhaseTwo = bossStateMachine.boss.isPhaseTwoActive;

        List<BossSkillSO> validSkills = skills.FindAll(skill =>
            ((isPhaseTwo && (skill.skillPhase == 1 || skill.skillPhase == 2)) ||
             (!isPhaseTwo && skill.skillPhase == 1))
            && skillCooldownTimers[skill] <= 0f
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

        float waitTime = skill.castTime * 0.7f;
        yield return new WaitForSeconds(waitTime);

        ExecuteSkillEffect(skill);

        yield return new WaitForSeconds(skill.castTime - waitTime);

        skillCooldownTimers[skill] = skill.specialAbilityCD;

        isCastingSkill = false;
    }


    private void ExecuteSkillEffect(BossSkillSO skill)
    {
        if (skill.skillName == "CastMeteor")
        {
            CastMeteor();
        }
        else if (skill.skillName == "Summoner")
        {
            SummonEnemies();
        }

        else if(skill.skillName == "BulletRing")
        {
            CastBulletCircleOrbs();
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

    private void CastBulletCircleOrbs()
    {
        int numberOfCircles = 4;
        int bulletsPerCircle = 16;
        float baseRadius = 1.5f;
        float radiusStep = 0.8f;

        float maxScale = 1.2f;
        float blinkSpeed = 6f;
        float damage = 10f;
        float moveSpeed = 1.2f;

        Vector3 bossPos = transform.position;

        for (int c = 0; c < numberOfCircles; c++)
        {
            float currentRadius = baseRadius + c * radiusStep;

            for (int i = 0; i < bulletsPerCircle; i++)
            {
                float angle = (360f / bulletsPerCircle) * i;
                Vector3 dir = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0f);
                Vector3 spawnPos = bossPos + dir * currentRadius;

                GameObject orb = Instantiate(bulletCirclePrefab, spawnPos, Quaternion.identity);
                Bullet orbScript = orb.GetComponent<Bullet>();
                orbScript.Initialize(dir, maxScale, blinkSpeed, damage, moveSpeed);
            }
        }
    }


}
