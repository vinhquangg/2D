using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{
    public static EnemySpawnerManager Instance { get; private set; }
    public List<SpawnZone> spawnZones = new List<SpawnZone>();

    private void Awake()
    {
        Instance = this;
    }

    public void AddZone(SpawnZone zone)
    {
        if (!spawnZones.Contains(zone))
        {
            spawnZones.Add(zone);
        }
    }
    //public void SpawnEnemiesInAllZones()
    //{
    //    foreach (var zone in spawnZones)
    //    {
    //        zone.SpawnEnemies();
    //    }
    //}
}
