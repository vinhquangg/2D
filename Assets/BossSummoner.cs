using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSummoner : MonoBehaviour
{
    [Header("Summon Settings")]
    [SerializeField] private float summonRadius = 3f;
    [SerializeField] private BoxCollider2D bossZoneCollider;
    private BaseBoss boss;
    private SpawnZone[] spawnZones;

    private void Awake()
    {
        boss = GetComponent<BaseBoss>();
        if (boss == null)
        {
            Debug.LogError("BossSummoner requires BaseBoss on the same GameObject!");
            enabled = false;
            return;
        }
        GameObject zoneObj = GameObject.FindWithTag("BossZone");
        if (zoneObj != null)
        {
            bossZoneCollider = zoneObj.GetComponent<BoxCollider2D>();
        }
        spawnZones = FindObjectsOfType<SpawnZone>();
    }

    public void SummonEnemies()
    {
        if (!boss.isPhaseTwoActive)
        {
            Debug.Log("[BossSummoner] Boss chưa chuyển sang Phase 2 → không triệu hồi.");
            return;
        }

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
                {
                    Debug.LogWarning("Không tìm được vị trí hợp lệ trong bossZoneCollider để triệu hồi quái.");
                    continue;
                }

                GameObject enemy = ObjectPooling.Instance.Spawn(zone.zoneEnemyType, summonPos, Quaternion.identity);

                if (enemy != null)
                {
                    var baseEnemy = enemy.GetComponent<BaseEnemy>();
                    if (baseEnemy != null)
                    {
                        baseEnemy.zoneID = zone.zoneID;
                        baseEnemy.assignedZone = zone;
                        baseEnemy.ResetEnemy();

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

        Debug.Log($"[BossSummoner] Tổng cộng đã triệu hồi {totalSummoned} quái xung quanh boss.");
    }

}
