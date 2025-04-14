using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]

public class SpawnZoneSaveData
{
    public string zoneID;
    public bool hasSpawned;
    public List<SpawnInfoZoneData> spawnInfos = new List<SpawnInfoZoneData>();
}
[System.Serializable]
public class SpawnInfoZoneData
{
    public EnemyType enemyType;
    public int maxSpawnCount; 
    public int initialSpawn; 
    public int spawnedCount; 
    public int deadCount;
    public int currentAlive;
    public int pendingSpawn;
}

