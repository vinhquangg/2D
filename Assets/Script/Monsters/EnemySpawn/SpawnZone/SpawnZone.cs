// (Các using như cũ)
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnZone : MonoBehaviour
{
    public string zoneID;
    public EnemyType zoneEnemyType;
    public int maxSpawnCount;
    public int initialSpawn;
    public List<Transform> entryPointOutsideZone;
    public List<Tilemap> obstacleTilemaps;
    public GameObject patrolPointPrefab;
    private Transform bossPointA;
    private Transform bossPointB;

    private int spawnedCount = 0;
    private int deadCount = 0;
    private int currentAlive = 0;
    private int enemyIDCount = 0;
    private bool hasSpawned = false;
    private bool allowSpawnCheck = true;
    private bool playerInsideZone = false;
    private bool isZoneCleared = false;

    private void Start()
    {
        Debug.Log($"Zone {zoneID} - entry points count: {entryPointOutsideZone.Count}");
        foreach (var ep in entryPointOutsideZone)
            Debug.Log($"Entry Point: {ep?.name}");

        GameObject a = GameObject.Find("BossPatrolA");
        GameObject b = GameObject.Find("BossPatrolB");

        if (a != null && b != null)
        {
            bossPointA = a.transform;
            bossPointB = b.transform;
        }
        else
        {
            Debug.LogWarning($"[SpawnZone] BossPointA hoặc BossPointB không tìm thấy trong scene.");
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        playerInsideZone = true;

        if (!hasSpawned && !isZoneCleared && spawnedCount < maxSpawnCount)
        {
            SpawnInitialEnemies();
            hasSpawned = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInsideZone = false;
        }
    }

    private void Update()
    {
        if (isZoneCleared || !playerInsideZone || !allowSpawnCheck) return;

        int availableSpawn = maxSpawnCount - spawnedCount;
        int neededSpawn = initialSpawn - currentAlive;
        int toSpawn = Mathf.Min(availableSpawn, neededSpawn);

        for (int i = 0; i < toSpawn; i++)
        {
            SpawnEnemy();
        }

        if (IsZoneCleared())
        {
            isZoneCleared = true;
            Debug.Log($"[SpawnZone] Zone {zoneID} has been cleared.");
        }
    }

    private void SpawnInitialEnemies()
    {
        int availableSpawn = maxSpawnCount - spawnedCount;
        int neededSpawn = initialSpawn - currentAlive;
        int toSpawn = Mathf.Min(availableSpawn, neededSpawn);

        for (int i = 0; i < toSpawn; i++)
        {
            SpawnEnemy();
        }
    }

    public void SpawnEnemy()
    {
        if (isZoneCleared || spawnedCount >= maxSpawnCount) return;

        Vector3 pos;
        if (zoneEnemyType == EnemyType.Boss)
        {
            BoxCollider2D box = GetComponent<BoxCollider2D>();
            Vector2 center = (Vector2)transform.position + box.offset;
            pos = center;
        }
        else
        {
            int attempts = 0;
            do { pos = GetRandomPositionInZone(); attempts++; }
            while (!IsTileWalkable(pos) && attempts < 10);
            if (attempts >= 10) { Debug.LogWarning("No valid spawn pos."); return; }
        }


        GameObject go = ObjectPooling.Instance.Spawn(zoneEnemyType, pos, Quaternion.identity);
        if (go == null)
        {
            return;
        }
        else
        {

        }

        var enemy = go.GetComponent<BaseEnemy>();
        enemy.zoneID = zoneID;
        enemy.enemyID = enemyIDCount++;
        enemy.assignedZone = this;
        EnemySpawnerManager.Instance.AddZone(enemy, this);
        if (zoneEnemyType == EnemyType.Boss)
        {
            if (bossPointA != null && bossPointB != null)
            {
                enemy.pointA = bossPointA.gameObject;
                enemy.pointB = bossPointB.gameObject;
                enemy.currentPoint = bossPointA;
            }
            else
            {
                Debug.LogWarning($"[SpawnZone] Boss patrol points not set in zone {zoneID}");
            }
        }
        else
        {
            enemy.ResetEnemy();
            Transform a = Instantiate(patrolPointPrefab, pos + (Vector3)Random.insideUnitCircle * 2f, Quaternion.identity).transform;
            Transform b = Instantiate(patrolPointPrefab, pos + (Vector3)Random.insideUnitCircle * 2f, Quaternion.identity).transform;
            enemy.pointA = a.gameObject;
            enemy.pointB = b.gameObject;
            enemy.currentPoint = a;
        }

        spawnedCount++;
        currentAlive++;
    }

    public void OnEnemyDied(BaseEnemy enemy)
    {
        if (enemy.enemyType != zoneEnemyType) return;
        currentAlive--;
        deadCount++;

        if (IsZoneCleared())
        {
            isZoneCleared = true;
            Debug.Log($"[SpawnZone] Zone {zoneID} has been cleared (enemy dead).");
        }
    }

    private Vector3 GetRandomPositionInZone()
    {
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        Vector2 center = (Vector2)transform.position + box.offset;
        Vector2 size = box.size;

        float x = Random.Range(center.x - size.x / 2, center.x + size.x / 2);
        float y = Random.Range(center.y - size.y / 2, center.y + size.y / 2);
        return new Vector2(x, y);
    }

    private bool IsTileWalkable(Vector3 worldPosition)
    {
        foreach (Tilemap tilemap in obstacleTilemaps)
        {
            Vector3Int cellPos = tilemap.WorldToCell(worldPosition);
            if (tilemap.HasTile(cellPos)) return false;
        }
        return true;
    }

    public bool IsZoneCleared()
    {
        return spawnedCount >= maxSpawnCount && currentAlive <= 0;
    }

    public bool IsZoneUncleared()
    {
        return !IsZoneCleared();
    }

    public Vector3 GetEntryPointOutsideZone()
    {
        if (entryPointOutsideZone != null && entryPointOutsideZone.Count > 0)
        {
            int randomIndex = Random.Range(0, entryPointOutsideZone.Count);
            return entryPointOutsideZone[randomIndex].position;
        }
        return GetDefaultStartPosition();
    }

    private Vector3 GetDefaultStartPosition()
    {
        return new Vector3(0, 0, 0);
    }

    public Transform GetBossPointA()
    {
        return bossPointA;
    }

    public Transform GetBossPointB()
    {
        return bossPointB;
    }

    public SpawnZoneSaveData SaveData()
    {
        return new SpawnZoneSaveData
        {
            zoneID = zoneID,
            isZoneCleared = isZoneCleared,
            enemyIDCount = enemyIDCount,
            zoneEnemyType = zoneEnemyType,
            maxSpawnCount = maxSpawnCount,
            initialSpawn = initialSpawn,
            spawnedCount = spawnedCount,
            deadCount = deadCount,
            currentAlive = currentAlive
        };
    }

    public void LoadData(SpawnZoneSaveData data)
    {
        zoneID = data.zoneID;
        isZoneCleared = data.isZoneCleared;
        enemyIDCount = data.enemyIDCount;
        zoneEnemyType = data.zoneEnemyType;
        maxSpawnCount = data.maxSpawnCount;
        initialSpawn = data.initialSpawn;
        spawnedCount = data.spawnedCount;
        deadCount = data.deadCount;
        currentAlive = data.currentAlive;

        if (!isZoneCleared)
        {
            spawnedCount = 0;
            deadCount = 0;
            currentAlive = 0;
            enemyIDCount = 0;
            allowSpawnCheck = true;
            hasSpawned = false;

            DespawnEnemiesInZone();
        }
        else
        {
            allowSpawnCheck = false;
            hasSpawned = true;
            DespawnEnemiesInZone();
        }
    }

    private void DespawnEnemiesInZone() 
    {
        BaseEnemy[] allEnemies = FindObjectsOfType<BaseEnemy>();
        foreach (BaseEnemy enemy in allEnemies)
        {
            if (enemy.zoneID == zoneID)
            {
                ObjectPooling.Instance.ReturnToPool(enemy.enemyType, enemy.gameObject);
            }
        }
    }
}
