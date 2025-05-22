using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{
    public static EnemySpawnerManager Instance { get; private set; }
    private Dictionary<int, SpawnZone> spawnZones = new Dictionary<int, SpawnZone>();


    private void Awake()
    {
        if (Instance == null) Instance = this; else Destroy(gameObject);
    }

    public void AddZone(BaseEnemy enemy, SpawnZone zone)
    {
        if (enemy != null && !spawnZones.ContainsKey(enemy.enemyID))
            spawnZones.Add(enemy.enemyID, zone);
    }

    public void EnemyDied(BaseEnemy enemy)
    {
        if (spawnZones.TryGetValue(enemy.enemyID, out var zone))
        {
            zone.OnEnemyDied(enemy);
            spawnZones.Remove(enemy.enemyID);
        }

        ObjectPooling.Instance.ReturnToPool(enemy.enemyType, enemy.gameObject);
    }



    public SpawnZone GetZoneByID(string id)
    {
        foreach (var zone in FindObjectsOfType<SpawnZone>())
            if (zone.zoneID == id) return zone;
        return null;
    }
}