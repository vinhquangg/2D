//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Tilemaps;

//public class SpawnZone : MonoBehaviour
//{
//    [System.Serializable]
//    public class SpawnInfo
//    {
//        public GameObject enemyPrefab;
//        public string enemyID;

//        public EnemyType enemyType;
//        public int maxSpawnCount;
//        public int initialSpawn;
//        public int currentAlive;
//        public int spawnedCount;
//        public int deadCount;
//        public int pendingSpawn;
//    }
//    public string zoneID;
//    public List<SpawnInfo> spawnInfos;
//    public List<Tilemap> obstacleTilemaps;
//    public GameObject patrolPointPrefab;
//    private bool hasSpawned = false;
//    private bool playerInsideZone = false;

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (!collision.CompareTag("Player")) return;

//        if (!hasSpawned)
//        {
//            SpawnInitialEnemies();
//            hasSpawned = true;
//        }

//        playerInsideZone = true;
//    }

//    private void OnTriggerExit2D(Collider2D collision)
//    {
//        if (collision.CompareTag("Player"))
//        {
//            playerInsideZone = false;
//        }
//    }
//    private void Update()
//    {
//        CheckSpawn();
//    }

//    private void CheckSpawn()
//    {
//        if (!playerInsideZone) return;

//        foreach (var info in spawnInfos)
//        {
//            int canSpawn = info.maxSpawnCount - info.spawnedCount;
//            int toSpawn = Mathf.Min(info.deadCount - info.pendingSpawn, canSpawn);
            
//            if (canSpawn > 0)
//            {

//                for (int i = 0; i< toSpawn; i++)
//                {
//                    SpawnEnemy(info);
//                    info.pendingSpawn++;
//                }
//                info.pendingSpawn -= toSpawn;
//            }
//        }
//    }

//    private void SpawnInitialEnemies()
//    {
//        foreach (var info in spawnInfos)
//        {
           
//            int toSpawn = Mathf.Min(info.initialSpawn - info.spawnedCount,
//                                    info.maxSpawnCount - info.spawnedCount);
//            for (int i = 0; i < toSpawn; i++)
//            {
//                SpawnEnemy(info);
//            }
//            Debug.Log($"[SpawnZone] Spawning {toSpawn} enemy(s) in zone {zoneID}");

//        }
//    }



//    private void SpawnEnemy(SpawnInfo spawnInfo)
//    {
//        if (spawnInfo.spawnedCount >= spawnInfo.maxSpawnCount)
//            return;

//        Vector3 spawnPosition;
//        int attempts = 0;
//        do
//        {
//            spawnPosition = GetRandomPositionInZone();
//            attempts++;
//        } while (!IsTileWalkable(spawnPosition) && attempts < 10);

//        if (attempts >= 10)
//        {
//            Debug.LogWarning("SpawnZone: Không tìm được vị trí hợp lệ để spawn enemy.");
//            return;
//        }

//        BaseEnemy enemy = Instantiate(spawnInfo.enemyPrefab, spawnPosition, Quaternion.identity).GetComponent<BaseEnemy>();
//        if (enemy != null)
//        {
//            spawnInfo.enemyType = enemy.enemyType;  
//        }

//        if (EnemySpawnerManager.Instance != null)
//        {
//            EnemySpawnerManager.Instance.AddZone(enemy, this);
//        }

//        Transform patrolA = Instantiate(patrolPointPrefab, spawnPosition + (Vector3)Random.insideUnitCircle * 2f, Quaternion.identity).transform;
//        Transform patrolB = Instantiate(patrolPointPrefab, spawnPosition + (Vector3)Random.insideUnitCircle * 2f, Quaternion.identity).transform;

//        //BaseEnemy enemy = enemyGO.GetComponent<BaseEnemy>();
//        if (enemy != null)
//        {
//            enemy.pointA = patrolA.gameObject;
//            enemy.pointB = patrolB.gameObject;
//            enemy.currentPoint = patrolA;
//            enemy.assignedZone = this;
//        }

//        spawnInfo.spawnedCount++;
//        spawnInfo.currentAlive++;

//    }


//    public void OnEnemyDied(BaseEnemy enemy)
//    {
//        foreach (var info in spawnInfos)
//        {
//            if (enemy.CompareTag(info.enemyPrefab.tag))
//            {
//                info.currentAlive--;
//                info.deadCount++;
//                break;
//            }
//        }
//    }


//    private Vector3 GetRandomPositionInZone()
//    {
//        BoxCollider2D box = GetComponent<BoxCollider2D>();
//        Vector2 center = (Vector2)transform.position + box.offset;
//        Vector2 size = box.size;

//        float x = Random.Range(center.x - size.x / 2, center.x + size.x / 2);
//        float y = Random.Range(center.y - size.y / 2, center.y + size.y / 2);
//        return new Vector2(x, y);
//    }

//    private bool IsTileWalkable(Vector3 worldPosition)
//    {
//        foreach (Tilemap tilemap in obstacleTilemaps)
//        {
//            Vector3Int cellPos = tilemap.WorldToCell(worldPosition);
//            if (tilemap.HasTile(cellPos))
//            {
//                return false;
//            }
//        }

//        return true;
//    }

//    public bool IsZoneCleared()
//    {
//        foreach (var info in spawnInfos)
//        {
//            if (info.spawnedCount < info.maxSpawnCount || info.currentAlive > 0)
//                return false;
//        }

//        return true;
//    }

//    public void UpdateZoneInfo(BaseEnemy enemy)
//    {
//        // Cập nhật thông tin cho tất cả các SpawnInfo trong zone
//        foreach (var info in spawnInfos)
//        {
//            // Kiểm tra xem loại enemy có khớp với enemy hiện tại không
//            if (enemy.enemyType == info.enemyType)
//            {
//                // Cập nhật thông tin sống/chết của enemy
//                if (enemy.currentHealth <= 0)
//                {
//                    info.currentAlive--;
//                    info.deadCount++;
//                }
//                else
//                {
//                    info.currentAlive++;
//                }

//                // Kiểm tra và cập nhật các thông tin khác nếu cần thiết
//                // Ví dụ: Cập nhật số lượng đã spawn
//                info.spawnedCount = Mathf.Min(info.spawnedCount, info.maxSpawnCount);

//                // Debug thông tin để chắc chắn rằng chúng được cập nhật đúng cách
//                Debug.Log($"Updated SpawnZone Info for EnemyType={info.enemyType}, " +
//                          $"SpawnedCount={info.spawnedCount}, AliveCount={info.currentAlive}, DeadCount={info.deadCount}");
//            }
//        }
//    }



//    public SpawnZoneSaveData SaveData()
//    {
//        SpawnZoneSaveData data = new SpawnZoneSaveData();
//        data.zoneID = this.zoneID;

//        // Kiểm tra spawnInfos trước khi thêm vào
//        foreach (var info in this.spawnInfos)
//        {
//            SpawnInfoZoneData infoData = new SpawnInfoZoneData
//            {
//                enemyID = info.enemyID,
//                enemyType = info.enemyType,
//                maxSpawnCount = info.maxSpawnCount,
//                initialSpawn = info.initialSpawn,
//                spawnedCount = info.spawnedCount,
//                deadCount = info.deadCount,
//                currentAlive = info.currentAlive,
//                pendingSpawn = info.pendingSpawn
//            };
//            data.spawnInfos.Add(infoData);

//            // Debug thông tin để chắc chắn rằng chúng được thêm vào đúng cách
//            Debug.Log($"Saving SpawnZone {data.zoneID}: EnemyType={info.enemyType}, SpawnedCount={info.spawnedCount}, DeadCount={info.deadCount}");
//        }

//        return data;
//    }

//    public void LoadData(SpawnZoneSaveData data)
//    {
//        this.zoneID = data.zoneID;

//        foreach (var infoData in data.spawnInfos)
//        {
//            var matchingInfo = spawnInfos.Find(i => i.enemyType == infoData.enemyType);
//            if (matchingInfo != null)
//            {
//                matchingInfo.enemyID = infoData.enemyID;
//                matchingInfo.maxSpawnCount = infoData.maxSpawnCount;
//                matchingInfo.initialSpawn = infoData.initialSpawn;
//                matchingInfo.spawnedCount = infoData.spawnedCount;
//                matchingInfo.deadCount = infoData.deadCount;
//                matchingInfo.currentAlive = infoData.currentAlive;
//                matchingInfo.pendingSpawn = infoData.pendingSpawn;

//                Debug.Log($"[LoadData] Restored info for {infoData.enemyType}: {infoData.spawnedCount} spawned, {infoData.deadCount} dead");
//            }
//            else
//            {
//                Debug.LogWarning($"[LoadData] Không tìm thấy SpawnInfo tương ứng với EnemyType {infoData.enemyType} trong zone {zoneID}");
//            }
//        }
//        if (!hasSpawned)
//        {
//            SpawnInitialEnemies();
//            hasSpawned = true;
//        }
//    }
//}
