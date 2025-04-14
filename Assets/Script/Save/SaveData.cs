using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public PlayerSaveData player;
    public List<EnemySaveData> enemies = new();
    public List<SpawnZoneSaveData> spawnZones;
}
