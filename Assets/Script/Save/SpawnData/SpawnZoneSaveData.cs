[System.Serializable]
public class SpawnZoneSaveData
{
    public string zoneID;
    public bool isZoneCleared;
    public EnemyType zoneEnemyType;

    public int maxSpawnCount;
    public int initialSpawn;
    public int spawnedCount;
    public int deadCount;
    public int currentAlive;

    public int enemyIDCount;
}
