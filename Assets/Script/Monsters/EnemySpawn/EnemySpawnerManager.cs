using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{
    public static EnemySpawnerManager Instance { get; private set; }
    private Dictionary<GameObject, SpawnZone> spawnZones = new Dictionary<GameObject, SpawnZone>();

    private void Awake()
    {
        Instance = this;
    }

    public void AddZone(GameObject enemy, SpawnZone zone)
    {
        if (!spawnZones.ContainsKey(enemy))
        {
            spawnZones.Add(enemy, zone);
        }
    }

    public void EnemyDied(GameObject enemy)
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
        // Tùy bạn tổ chức theo type → prefab
        // Có thể dùng Dictionary<EnemyType, GameObject> map trước
        return null;
    }


}
