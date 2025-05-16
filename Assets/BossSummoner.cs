using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BossSummoner : MonoBehaviour
{
    [Header("Summon Settings")]
    [SerializeField] private float summonRadius = 3f;
    [SerializeField] private BoxCollider2D bossZoneCollider;
    [SerializeField] private float minionsKillTime;
    [SerializeField] private TextMeshProUGUI countdownText;
    private BaseBoss boss;
    private SpawnZone[] spawnZones;
    private List<BaseEnemy> summonedEnemies = new List<BaseEnemy>();
    private void Awake()
    {
        boss = GetComponent<BaseBoss>();
        if (boss == null)
        {
            enabled = false;
            return;
        }

    }

    private void Start()
    {
        GameObject zoneObj = GameObject.FindWithTag("BossZone");
        if (zoneObj != null)
        {
            bossZoneCollider = zoneObj.GetComponent<BoxCollider2D>();
        }
        spawnZones = FindObjectsOfType<SpawnZone>();

        GameObject canvas = GameObject.Find("Canvas");
        if (canvas != null)
        {
            Transform summonTransform = canvas.transform.Find("SummonTime");
            if (summonTransform != null)
            {
                countdownText = summonTransform.GetComponent<TMPro.TextMeshProUGUI>();
            }
        }
    }

    public void SummonEnemies()
    {
        if (!boss.isPhaseTwoActive)
            return;

        foreach (var enemy in summonedEnemies)
        {
            if (enemy != null && !enemy.isDead && enemy.gameObject.activeInHierarchy)
            {
                Debug.Log("Cannot summon: previous summoned enemies still alive or not returned to pool.");
                return;
            }
        }

        summonedEnemies.Clear();

        int totalSummoned = 0;

        foreach (var zone in spawnZones)
        {
            int summonCount = Mathf.FloorToInt(zone.maxSpawnCount * 0.5f);

            for (int i = 0; i < summonCount / 2; i++)
            {
                Vector2 summonPos = Vector2.zero;
                int maxAttempts = 20;
                bool validPosFound = false;

                while (maxAttempts > 0 && !validPosFound)
                {
                    Vector2 randomOffset = Random.insideUnitCircle.normalized * summonRadius;
                    Vector2 potentialPos = (Vector2)transform.position + randomOffset;

                    if (bossZoneCollider != null && bossZoneCollider.OverlapPoint(potentialPos))
                    {
                        summonPos = potentialPos;
                        validPosFound = true;
                    }
                    else
                    {
                        maxAttempts--;
                    }
                }

                if (!validPosFound)
                    continue;

                GameObject enemy = ObjectPooling.Instance.Spawn(zone.zoneEnemyType, summonPos, Quaternion.identity);

                if (enemy != null)
                {
                    var baseEnemy = enemy.GetComponent<BaseEnemy>();
                    if (baseEnemy != null)
                    {
                        baseEnemy.zoneID = zone.zoneID;
                        baseEnemy.assignedZone = zone;
                        baseEnemy.ResetEnemy();
                        summonedEnemies.Add(baseEnemy);

                        Transform a = Instantiate(zone.patrolPointPrefab, summonPos + Random.insideUnitCircle * 2f, Quaternion.identity).transform;
                        Transform b = Instantiate(zone.patrolPointPrefab, summonPos + Random.insideUnitCircle * 2f, Quaternion.identity).transform;

                        baseEnemy.pointA = a.gameObject;
                        baseEnemy.pointB = b.gameObject;
                        baseEnemy.currentPoint = a;
                    }

                    EnemySpawnerManager.Instance.AddZone(baseEnemy, zone);
                    totalSummoned++;
                }
            }
        }

        StartCoroutine(CheckMinionsKillTime());
    }


    private IEnumerator CheckMinionsKillTime()
    {
        float timeLeft = minionsKillTime;
        countdownText.gameObject.SetActive(true);

        while (timeLeft > 0f)
        {
            countdownText.text = $"{Mathf.CeilToInt(timeLeft)}s";
            yield return new WaitForSeconds(1f);
            timeLeft--;

            if (timeLeft <= minionsKillTime / 3f)
                countdownText.color = Color.red;
            else
                countdownText.color = Color.white;

            bool allDead = true;
            foreach (var enemy in summonedEnemies)
            {
                if (enemy != null && !enemy.isDead)
                {
                    allDead = false;
                    break;
                }
            }

            if (allDead)
            {
                countdownText.gameObject.SetActive(false);
                summonedEnemies.Clear();
                yield break;
            }
        }


        int aliveCount = 0;
        foreach (var enemy in summonedEnemies)
        {
            if (enemy != null && !enemy.isDead)
            {
                aliveCount++;
            }
        }

        float healAmount = aliveCount * 100f;
        boss.Heal(healAmount);

        foreach (var enemy in summonedEnemies)
        {
            if (enemy != null && !enemy.isDead)
            {
                enemy.isDead = true; 
                ObjectPooling.Instance.ReturnToPool(enemy.enemyType, enemy.gameObject);
            }
        }

        countdownText.gameObject.SetActive(false);

    }



}
