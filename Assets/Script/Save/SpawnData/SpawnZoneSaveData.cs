using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]

public class SpawnZoneSaveData
{
    public string zoneID;
    public List<SpawnInfoZoneData> spawnInfos;
}
[System.Serializable]
public class SpawnInfoZoneData
{
    public EnemyType enemyType;
    public int spawnedCount;
    public int deadCount;
    public int currentAlive;
}
