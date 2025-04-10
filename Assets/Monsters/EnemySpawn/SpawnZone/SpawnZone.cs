using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnZone : MonoBehaviour
{
    [System.Serializable]
    public class SpawnInfo
    {
        public GameObject enemyPrefab;
        public int maxSpawnCount;
        public int initialSpawn;
        [HideInInspector] public int currentAlive;
        [HideInInspector] public int spawnedCount;
        [HideInInspector] public int deadCount;
    }

    public List<SpawnInfo> spawnInfos;
    public List<Tilemap> obstacleTilemaps;
    public GameObject patrolPointPrefab;
    //private bool readyToAutoSpawn = false;
    private bool hasSpawned = false;
    private bool playerInsideZone = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (!hasSpawned)
        {
            SpawnInitialEnemies();
            hasSpawned = true;
            //StartCoroutine(EnableAutoSpawnAfterDelay(1f)); // chờ 1 giây rồi mới spawn bù
        }

        playerInsideZone = true;
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
        if (!playerInsideZone) return;

        foreach (var info in spawnInfos)
        {
            if (info.deadCount > 0 && info.spawnedCount < info.maxSpawnCount)
            {
                SpawnEnemy(info);
                info.deadCount--;
            }
        }
    }

    private void SpawnInitialEnemies()
    {
        foreach (var info in spawnInfos)
        {
            int toSpawn = Mathf.Min(info.initialSpawn, info.maxSpawnCount - info.spawnedCount);
            for (int i = 0; i < toSpawn; i++)
            {
                SpawnEnemy(info);
            }
        }
    }


    private void SpawnEnemy(SpawnInfo spawnInfo)
    {
        if (spawnInfo.spawnedCount >= spawnInfo.maxSpawnCount)
            return;

        Vector3 spawnPosition;
        int attempts = 0;
        do
        {
            spawnPosition = GetRandomPositionInZone();
            attempts++;
        } while (!IsTileWalkable(spawnPosition) && attempts < 10);

        if (attempts >= 10)
        {
            Debug.LogWarning("SpawnZone: Không tìm được vị trí hợp lệ để spawn enemy.");
            return;
        }

        GameObject enemyGO = Instantiate(spawnInfo.enemyPrefab, spawnPosition, Quaternion.identity);
        if (EnemySpawnerManager.Instance != null)
        {
            EnemySpawnerManager.Instance.AddZone(enemyGO, this);
        }

        Transform patrolA = Instantiate(patrolPointPrefab, spawnPosition + (Vector3)Random.insideUnitCircle * 2f, Quaternion.identity).transform;
        Transform patrolB = Instantiate(patrolPointPrefab, spawnPosition + (Vector3)Random.insideUnitCircle * 2f, Quaternion.identity).transform;

        BaseEnemy enemy = enemyGO.GetComponent<BaseEnemy>();
        if (enemy != null)
        {
            enemy.pointA = patrolA.gameObject;
            enemy.pointB = patrolB.gameObject;
            enemy.currentPoint = patrolA;
        }

        spawnInfo.spawnedCount++;
        spawnInfo.currentAlive++;
    }


    public void OnEnemyDied(GameObject enemy)
    {
        foreach (var info in spawnInfos)
        {
            if (enemy.CompareTag(info.enemyPrefab.tag))
            {
                info.currentAlive--;
                info.deadCount++;
                break;
            }
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
            if (tilemap.HasTile(cellPos))
            {
                return false;
            }
        }

        return true;
    }

    public bool IsZoneCleared()
    {
        foreach (var info in spawnInfos)
        {
            if (info.spawnedCount < info.maxSpawnCount || info.currentAlive > 0)
                return false;
        }

        return true;
    }


}
