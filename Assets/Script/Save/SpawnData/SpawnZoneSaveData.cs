using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]

public class SpawnZoneSaveData
{
    public string zoneID;
    public List<SpawnInfoZoneData> spawnInfos = new List<SpawnInfoZoneData>();
}
[System.Serializable]
public class SpawnInfoZoneData
{
    public string enemyID;
    public EnemyType enemyType;
    public int maxSpawnCount; 
    public int initialSpawn; 
    public int spawnedCount; 
    public int deadCount;
    public int currentAlive;
    public int pendingSpawn;
}

