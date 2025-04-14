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
        public EnemyType enemyType;
        public int currentAlive;
        public int spawnedCount;
        public int deadCount;
        public int pendingSpawn;
    }
    public string zoneID;
    public List<SpawnInfo> spawnInfos;
    public List<Tilemap> obstacleTilemaps;
    public GameObject patrolPointPrefab;
    private bool hasSpawned = false;
    private bool playerInsideZone = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (!hasSpawned)
        {
            SpawnInitialEnemies();
            hasSpawned = true;
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
        CheckSpawn();
    }

    private void CheckSpawn()
    {
        if (!playerInsideZone) return;

        foreach (var info in spawnInfos)
        {
            int canSpawn = info.maxSpawnCount - info.spawnedCount;
            int toSpawn = Mathf.Min(info.deadCount - info.pendingSpawn, canSpawn);
            
            if (canSpawn > 0)
            {

                for (int i = 0; i< toSpawn; i++)
                {
                    SpawnEnemy(info);
                    info.pendingSpawn++;
                }   
                
            }
        }
    }

    private void SpawnInitialEnemies()
    {
        foreach (var info in spawnInfos)
        {
           
            int toSpawn = Mathf.Min(info.initialSpawn - info.spawnedCount,
                                    info.maxSpawnCount - info.spawnedCount);
            for (int i = 0; i < toSpawn; i++)
            {
                SpawnEnemy(info);
            }
            Debug.Log($"[SpawnZone] Spawning {toSpawn} enemy(s) in zone {zoneID}");

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

        BaseEnemy enemy = Instantiate(spawnInfo.enemyPrefab, spawnPosition, Quaternion.identity).GetComponent<BaseEnemy>();
        if (enemy != null)
        {
            spawnInfo.enemyType = enemy.enemyType;  
        }

        if (EnemySpawnerManager.Instance != null)
        {
            EnemySpawnerManager.Instance.AddZone(enemy, this);
        }

        Transform patrolA = Instantiate(patrolPointPrefab, spawnPosition + (Vector3)Random.insideUnitCircle * 2f, Quaternion.identity).transform;
        Transform patrolB = Instantiate(patrolPointPrefab, spawnPosition + (Vector3)Random.insideUnitCircle * 2f, Quaternion.identity).transform;

        //BaseEnemy enemy = enemyGO.GetComponent<BaseEnemy>();
        if (enemy != null)
        {
            enemy.pointA = patrolA.gameObject;
            enemy.pointB = patrolB.gameObject;
            enemy.currentPoint = patrolA;
            enemy.assignedZone = this;
        }

        spawnInfo.spawnedCount++;
        spawnInfo.currentAlive++;

    }


    public void OnEnemyDied(BaseEnemy enemy)
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
    public SpawnZoneSaveData SaveData()
    {
        SpawnZoneSaveData data = new SpawnZoneSaveData();
        data.zoneID = this.zoneID;

        foreach (var infoData in data.spawnInfos)
        {
            Debug.Log($"Loading enemyType: {infoData.enemyType}");

            var spawnInfo = spawnInfos.Find(info => info.enemyType == infoData.enemyType);

            if (spawnInfo != null)
            {
                Debug.Log($"Found match for {infoData.enemyType}");
                spawnInfo.maxSpawnCount = infoData.maxSpawnCount;
                spawnInfo.initialSpawn = infoData.initialSpawn;
                spawnInfo.spawnedCount = infoData.spawnedCount;
                spawnInfo.deadCount = infoData.deadCount;
                spawnInfo.currentAlive = infoData.currentAlive;
                spawnInfo.pendingSpawn = infoData.pendingSpawn;
            }
            else
            {
                Debug.LogWarning($"No match found in spawnInfos for enemyType: {infoData.enemyType}");
            }
        }


        return data;
    }

    public void LoadData(SpawnZoneSaveData data)
    {
        this.zoneID = data.zoneID;
        this.spawnInfos = new List<SpawnInfo>(); 

        foreach (var infoData in data.spawnInfos)
        {
            SpawnInfo newInfo = new SpawnInfo();
            newInfo.enemyType = infoData.enemyType;
            newInfo.maxSpawnCount = infoData.maxSpawnCount;
            newInfo.initialSpawn = infoData.initialSpawn;
            newInfo.spawnedCount = infoData.spawnedCount;
            newInfo.deadCount = infoData.deadCount;
            newInfo.currentAlive = infoData.currentAlive;
            newInfo.pendingSpawn = infoData.pendingSpawn;

            this.spawnInfos.Add(newInfo);
        }
    }




}
