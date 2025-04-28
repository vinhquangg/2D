using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{
    public static EnemySpawnerManager Instance { get; private set; }
    private Dictionary<BaseEnemy, SpawnZone> spawnZones = new Dictionary<BaseEnemy, SpawnZone>();

    private void Awake()
    {
        if (Instance == null) Instance = this; else Destroy(gameObject);
    }

    public void AddZone(BaseEnemy enemy, SpawnZone zone)
    {
        if (enemy != null && !spawnZones.ContainsKey(enemy))
            spawnZones.Add(enemy, zone);
    }

    public void EnemyDied(BaseEnemy enemy)
    {
        if (spawnZones.TryGetValue(enemy, out var zone))
        {
            zone.OnEnemyDied(enemy);
            spawnZones.Remove(enemy);
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