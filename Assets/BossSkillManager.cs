using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkillManager : MonoBehaviour
{
    private BossStateMachine bossStateMachine;
    public List<BossSkillSO> skills = new List<BossSkillSO>();  // sửa thành ScriptableObject
    private int currentSkillIndex = 0;
    private Animator animator;
    private BossSummoner bossSummoner;
    [SerializeField] private GameObject meteorPrefab;
    [SerializeField] private BoxCollider2D bossZoneCollider;
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
    }

    public void UseNextSkill()
    {
        if (skills.Count > 0)
        {
            int randomIndex = Random.Range(0, skills.Count);
            CurrentSkill = skills[randomIndex];
            StartCoroutine(CastSkill(CurrentSkill));
        }
    }

    private IEnumerator CastSkill(BossSkillSO skill)
    {
        animator.Play(skill.animationName);
        yield return new WaitForSeconds(skill.castTime);  
        ExecuteSkillEffect(skill);
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
}
