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
        public EnemyType enemyType;
        public int maxSpawnCount;
        public int initialSpawn;
        public int currentAlive;
        public int spawnedCount;
        public int deadCount;
        //public int pendingSpawn;
    }
    private bool allowSpawnCheck = true;
    public string zoneID;
    public List<SpawnInfo> spawnInfos;
    public List<Tilemap> obstacleTilemaps;
    public GameObject patrolPointPrefab;
    private int enemyIDCount = 0;
    private bool hasSpawned = false;
    private bool playerInsideZone = false;
    private bool isZoneCleared = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (!hasSpawned && !isZoneCleared && TotalSpawnedCount() < TotalMaxCount())
        {
            //SpawnInitialEnemies();
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
        if (isZoneCleared) return; 
        CheckSpawn();
    }
    private int TotalSpawnedCount()
    {
        int total = 0;
        foreach (var info in spawnInfos)
        {
            total += info.spawnedCount;
        }
        return total;
    }

    private int TotalMaxCount()
    {
        int total = 0;
        foreach (var info in spawnInfos)
        {
            total += info.maxSpawnCount;
        }
        return total;
    }

    private void CheckSpawn()
    {
        if (!playerInsideZone || !allowSpawnCheck) return; // ✅ thêm chặn check

        foreach (var info in spawnInfos)
        {
            if (info.deadCount + info.currentAlive >= info.maxSpawnCount) continue;
            if (info.currentAlive >= info.initialSpawn) continue;

            int availableSpawn = info.maxSpawnCount - info.spawnedCount;
            int neededSpawn = info.initialSpawn - info.currentAlive;
            int toSpawn = Mathf.Min(availableSpawn, neededSpawn);

            for (int i = 0; i < toSpawn; i++)
            {
                SpawnEnemy(info);
            }
        }

        if (IsZoneCleared())
        {
            isZoneCleared = true;
            Debug.Log($"[SpawnZone] Zone {zoneID} has been cleared. No more spawns.");
        }
    }



    //private void SpawnInitialEnemies()
    //{
    //    foreach (var info in spawnInfos)
    //    {
    //        // Nếu đã spawn đủ tổng số lượng thì không cần spawn nữa
    //        if (info.spawnedCount >= info.maxSpawnCount)
    //        {
    //            Debug.Log($"[SpawnInitialEnemies] Đã spawn đủ {info.spawnedCount}/{info.maxSpawnCount}, bỏ qua");
    //            continue;
    //        }

    //        // Nếu số quái còn sống đã đủ ban đầu thì không cần spawn nữa
    //        if (info.currentAlive >= info.initialSpawn)
    //        {
    //            Debug.Log($"[SpawnInitialEnemies] currentAlive đã đủ {info.currentAlive}/{info.initialSpawn}, bỏ qua");
    //            continue;
    //        }

    //        int availableSpawn = info.maxSpawnCount - info.spawnedCount;
    //        int neededSpawn = info.initialSpawn - info.currentAlive;

    //        int toSpawn = Mathf.Min(availableSpawn, neededSpawn);

    //        for (int i = 0; i < toSpawn; i++)
    //        {
    //            SpawnEnemy(info);
    //        }

    //        Debug.Log($"[SpawnInitialEnemies] Đã spawn thêm {toSpawn} con cho {info.enemyType}");
    //    }
    //}





    private void SpawnEnemy(SpawnInfo spawnInfo)
    {
        if (isZoneCleared) return;
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
            enemy.enemyID = enemyIDCount;
            enemy.zoneID = this.zoneID;
            enemyIDCount++;
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
        Debug.Log($"[OnEnemyDied] Enemy {enemy.name} died in zone {zoneID}");

        foreach (var info in spawnInfos)
        {
            if (enemy.enemyType == info.enemyType)
            {
                info.currentAlive--;
                info.deadCount++;

                Debug.Log($"[OnEnemyDied] Update: currentAlive={info.currentAlive}, deadCount={info.deadCount}");
                break;
            }
        }

        if (IsZoneCleared())
        {
            isZoneCleared = true;
            Debug.Log($"[SpawnZone] Zone {zoneID} has been cleared due to all enemies dead.");
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

    private void SpawnRemainingEnemies()
    {
        foreach (var info in spawnInfos)
        {
            // Nếu đã spawn đủ thì không spawn nữa
            if (info.spawnedCount >= info.maxSpawnCount)
                continue;

            // Nếu đang có đủ số lượng ban đầu sống thì không cần spawn
            if (info.currentAlive >= info.initialSpawn)
                continue;

            int availableSpawn = info.maxSpawnCount - info.spawnedCount;
            int neededSpawn = info.initialSpawn - info.currentAlive;

            int toSpawn = Mathf.Min(availableSpawn, neededSpawn);

            for (int i = 0; i < toSpawn; i++)
            {
                SpawnEnemy(info);
            }

            Debug.Log($"[SpawnZone] SpawnRemainingEnemies: {toSpawn}, currentAlive: {info.currentAlive}, deadCount: {info.deadCount}, spawnedCount: {info.spawnedCount}");
        }
    }



    public SpawnZoneSaveData SaveData()
    {
        SpawnZoneSaveData data = new SpawnZoneSaveData();
        data.zoneID = this.zoneID;
        data.isZoneCleared = isZoneCleared;
        // Kiểm tra spawnInfos trước khi thêm vào
        foreach (var info in this.spawnInfos)
        {
            SpawnInfoZoneData infoData = new SpawnInfoZoneData
            {
                enemyType = info.enemyType,
                maxSpawnCount = info.maxSpawnCount,
                initialSpawn = info.initialSpawn,
                spawnedCount = info.spawnedCount,
                deadCount = info.deadCount,
                currentAlive = info.currentAlive,
                //pendingSpawn = info.pendingSpawn
            };
            data.spawnInfos.Add(infoData);

            // Debug thông tin để chắc chắn rằng chúng được thêm vào đúng cách
            Debug.Log($"Saving SpawnZone {data.zoneID}: EnemyType={info.enemyType}, SpawnedCount={info.spawnedCount}, DeadCount={info.deadCount}");
        }

        return data;
    }

    public void LoadData(SpawnZoneSaveData data)
    {
        this.zoneID = data.zoneID;
        this.isZoneCleared = data.isZoneCleared;
        foreach (var infoData in data.spawnInfos)
        {
            var matchingInfo = spawnInfos.Find(i => i.enemyType == infoData.enemyType);
            if (matchingInfo != null)
            {
                matchingInfo.maxSpawnCount = infoData.maxSpawnCount;
                matchingInfo.initialSpawn = infoData.initialSpawn;
                matchingInfo.spawnedCount = infoData.spawnedCount;
                matchingInfo.deadCount = infoData.deadCount;
                matchingInfo.currentAlive = infoData.currentAlive;
                //matchingInfo.pendingSpawn = infoData.pendingSpawn;

                Debug.Log($"[LoadData] Restored info for {infoData.enemyType}: {infoData.spawnedCount} spawned, {infoData.deadCount} dead");
            }
            else
            {
                Debug.LogWarning($"[LoadData] Không tìm thấy SpawnInfo tương ứng với EnemyType {infoData.enemyType} trong zone {zoneID}");
            }
        }
        if (!isZoneCleared)
        {
            SpawnRemainingEnemies();
            allowSpawnCheck = false;
        }
        hasSpawned = true;
    }
}
