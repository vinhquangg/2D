using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnZone : MonoBehaviour
{
    public List<Tilemap> obstacleTilemaps;
    public GameObject patrolPointPrefab;
    [System.Serializable]
    public class SpawnInfo
    {
        public GameObject enemyPrefab;
        public int spawnCount;
    }

    public List<SpawnInfo> spawnInfos;

    private void Start()
    {
        if(EnemySpawnerManager.Instance != null)
            EnemySpawnerManager.Instance.AddZone(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SpawnEnemies();
        }
    }
    public void SpawnEnemies()
    {
        foreach (var spawnInfo in spawnInfos)
        {

            //Vector3 spawnPosition;
            //int attempts = 0;
            //do
            //{
            //    spawnPosition = GetRandomPositionInZone();
            //    attempts++;
            //} while (!IsTileWalkable(spawnPosition) && attempts < 10);

            //if (attempts >= 10)
            //{
            //    Debug.LogWarning("SpawnZone: Không tìm được vị trí hợp lệ để spawn enemy.");
            //    continue;
            //}

            //GameObject enemyGO = Instantiate(spawnInfo.enemyPrefab, spawnPosition, Quaternion.identity);

            for (int i = 0; i < spawnInfo.spawnCount; i++)
            {
                //Vector3 spawnPosition = GetRandomPositionInZone()/*transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0)*/;
                //GameObject enemyGO = Instantiate(spawnInfo.enemyPrefab, spawnPosition, Quaternion.identity);
                ////GameObject enemyGO = Instantiate(spawnInfo.enemyPrefab, spawnPosition)
                ///
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
                    continue;
                }

                GameObject enemyGO = Instantiate(spawnInfo.enemyPrefab, spawnPosition, Quaternion.identity);

                Transform patrolA = Instantiate(patrolPointPrefab, spawnPosition + (Vector3)Random.insideUnitCircle * 2f, Quaternion.identity).transform;
                Transform patrolB = Instantiate(patrolPointPrefab, spawnPosition + (Vector3)Random.insideUnitCircle * 2f, Quaternion.identity).transform;

                BaseEnemy enemy = enemyGO.GetComponent<BaseEnemy>();
                if (enemy != null)
                {
                    enemy.pointA = patrolA.gameObject;
                    enemy.pointB = patrolB.gameObject;
                    enemy.currentPoint = patrolA;
                }


            }
        }
    }
    Vector3 GetRandomPositionInZone()
    {
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        Vector2 center = (Vector2)transform.position + box.offset;
        Vector2 size = box.size;

        float x = Random.Range(center.x - size.x / 2, center.x + size.x / 2);
        float y = Random.Range(center.y - size.y / 2, center.y + size.y / 2);
        return new Vector2(x, y);
    }
    bool IsTileWalkable(Vector3 worldPosition)
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
}
