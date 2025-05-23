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

    private bool bossDeadHandled = false;
    private BaseBoss boss;
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

        GameObject canvas = GameObject.Find("Canvas");
        if (canvas != null)
        {
            Transform summonTransform = canvas.transform.Find("SummonTime");
            if (summonTransform != null)
            {
                countdownText = summonTransform.GetComponent<TextMeshProUGUI>();
            }
        }
    }

    private void Update()
    {
        if (boss != null && boss.currentHealth <= 0 && !bossDeadHandled)
        {
            bossDeadHandled = true;
            ReturnAllSummonedToPool();
        }
    }

    private void ReturnAllSummonedToPool()
    {
        foreach (var enemy in summonedEnemies)
        {
            if (enemy != null && enemy.gameObject.activeInHierarchy)
            {
                enemy.isDead = true;
                ObjectPooling.Instance.ReturnToPool(enemy.enemyType, enemy.gameObject);
            }
        }

        summonedEnemies.Clear();

        if (countdownText != null)
            countdownText.gameObject.SetActive(false);

        Debug.Log("[BossSummoner] Boss chết - các quái triệu hồi đã bị thu hồi về pool.");
    }

    public void SummonEnemies()
    {
        if (!boss.isPhaseTwoActive)
            return;

        foreach (var enemy in summonedEnemies)
        {
            if (enemy != null && !enemy.isDead && enemy.gameObject.activeInHierarchy)
                return;
        }

        summonedEnemies.Clear();

        SpawnEnemiesOfType(EnemyType.Mage, 6);
        SpawnEnemiesOfType(EnemyType.Assassin, 2);

        StartCoroutine(CheckMinionsKillTime());
    }

    private void SpawnEnemiesOfType(EnemyType type, int count)
    {
        for (int i = 0; i < count; i++)
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

            GameObject enemyObj = ObjectPooling.Instance.Spawn(type, summonPos, Quaternion.identity);
            if (enemyObj != null)
            {
                BaseEnemy baseEnemy = enemyObj.GetComponent<BaseEnemy>();
                if (baseEnemy != null)
                {
                    baseEnemy.ResetEnemy();
                    summonedEnemies.Add(baseEnemy);

                    Transform a = Instantiate(baseEnemy.patrolPointPrefab, summonPos + Random.insideUnitCircle * 2f, Quaternion.identity).transform;
                    Transform b = Instantiate(baseEnemy.patrolPointPrefab, summonPos + Random.insideUnitCircle * 2f, Quaternion.identity).transform;

                    baseEnemy.pointA = a.gameObject;
                    baseEnemy.pointB = b.gameObject;
                    baseEnemy.currentPoint = a;
                }
            }
        }
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

            countdownText.color = (timeLeft <= minionsKillTime / 3f) ? Color.red : Color.white;

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
                enemy.isDead = true;
                ObjectPooling.Instance.ReturnToPool(enemy.enemyType, enemy.gameObject);
            }
        }

        float healAmount = aliveCount * 100f;
        boss.Heal(healAmount);
        countdownText.gameObject.SetActive(false);

        if (boss.currentHealth > boss.bossState.bossData.maxHealth * 0.5f)
        {
            boss.isPhaseTwoActive = false;
        }
    }
}
