using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{
    public static EnemySpawnerManager Instance { get; private set; }
    private Dictionary<BaseEnemy, SpawnZone> spawnZones = new Dictionary<BaseEnemy, SpawnZone>();

    private void Awake()
    {
        Instance = this;
    }

    public void AddZone(BaseEnemy enemy, SpawnZone zone)
    {
        if (!spawnZones.ContainsKey(enemy))
        {
            spawnZones.Add(enemy, zone);
        }
    }

    public void EnemyDied(BaseEnemy enemy)
    {
        if (spawnZones.TryGetValue(enemy, out SpawnZone zone))
        {
            zone.OnEnemyDied(enemy);
            spawnZones.Remove(enemy);
        }
    }

    public SpawnZone GetZoneByID(string id)
    {
        foreach (var zone in FindObjectsOfType<SpawnZone>())
        {
            if (zone.zoneID == id)
                return zone;
        }
        return null;
    }

    public GameObject GetPrefab(EnemyType type)
    {
        return null;
    }


}
