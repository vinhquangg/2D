using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    [SerializeField] private PlayerStateMachine playerStateMachine;
    private PlayerSaveData playerData;
    //private SpawnZone SpawnZone;

    public static SaveLoadManager instance { get;private set; }

    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("found more than one SaveLoadManager in the scene");
        }
        instance = this;

        if (playerStateMachine == null)
        {
            playerStateMachine = FindObjectOfType<PlayerStateMachine>();
        }

        //if (SpawnZone == null)
        //{
        //    SpawnZone = FindObjectOfType<SpawnZone>();
        //}
    }
    void Start()
    {
        if (!PlayerPrefs.HasKey("pending_save_data"))
        {
            if(PlayerPrefs.HasKey("player_save"))
                PlayerPrefs.DeleteKey("player_save");
            NewGame();
        }
        
    }

    public void NewGame()
    {
        PlayerSaveData playerData = playerStateMachine.GetDefaultPlayerData();

        playerStateMachine.LoadFromData(playerData);

        playerStateMachine.SwitchState(new IdleState(playerStateMachine));
    }


    public void SaveGame()
    {
        // Save player data
        PlayerSaveData playerData = playerStateMachine.GetPlayerSaveData();

        // Save enemy data
        List<EnemySaveData> enemyList = new();
        var allEnemies = FindObjectsOfType<BaseEnemy>();
        foreach (var enemy in allEnemies)
        {
            if (enemy is ISaveable saveable)
            {
                var monsterData = saveable.SaveData();

                if (monsterData is EnemySaveData e)
                {
                    enemyList.Add(e);
                }
            }
        }

        // Save spawn zone data
        List<SpawnZoneSaveData> zoneSaveDataList = new();
        var spawnZones = FindObjectsOfType<SpawnZone>();
        foreach (var zone in spawnZones)
        {
            SpawnZoneSaveData zoneData = zone.SaveData(); 
            zoneSaveDataList.Add(zoneData);
        }

        SaveData saveData = new SaveData
        {
            player = playerData,
            enemies = enemyList,
            spawnZones = zoneSaveDataList
        };

        string json = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString("saveData_save", json);
    }


    public void LoadGame()
    {
        if (!PlayerPrefs.HasKey("saveData_save"))
        {
            NewGame();
            return;
        }

        string json = PlayerPrefs.GetString("saveData_save");
        SaveData saveData = JsonUtility.FromJson<SaveData>(json);

        // Load Player
        playerStateMachine.LoadFromData(saveData.player);

        // Creade Dictionary to save enemy data
        Dictionary<EnemyType, EnemySaveData> enemyDataDictionary = new();
        foreach (var enemyData in saveData.enemies)
        {
            enemyDataDictionary[enemyData.type] = enemyData;  // Put into Dictionary with key enemyType
        }

        // load and update Enemies
        var enemies = FindObjectsOfType<BaseEnemy>();
        foreach (var enemy in enemies)
        {
            if (enemyDataDictionary.TryGetValue(enemy.enemyType, out var enemyData) && enemy is ISaveable saveable)
            {
                saveable.LoadData(enemyData);
            }
        }

        // create Dictionary to save spawn zone data
        Dictionary<string, SpawnZoneSaveData> zoneDataDictionary = new();
        foreach (var zoneData in saveData.spawnZones)
        {
            zoneDataDictionary[zoneData.zoneID] = zoneData;  // Put into Dictionary with key zoneID
        }

        // Load and update Spawn Zones
        var spawnZones = FindObjectsOfType<SpawnZone>();
        foreach (var zone in spawnZones)
        {
            if (zoneDataDictionary.TryGetValue(zone.zoneID, out var zoneData))
            {
                zone.LoadData(zoneData);  
            }
        }
    }



    public void LoadAfterSceneLoaded()
    {
        StartCoroutine(DelayThenLoadData());
    }

    private IEnumerator DelayThenLoadData()
    {
        yield return null; 
        LoadGameFromPendingData();
    }

    public void LoadGameFromPendingData()
    {

        if (!PlayerPrefs.HasKey("pending_save_data"))
        {
            return;
        }

        string json = PlayerPrefs.GetString("pending_save_data");
        PlayerPrefs.DeleteKey("pending_save_data");
        SaveData saveData = JsonUtility.FromJson<SaveData>(json);
        ApplyLoadedData(saveData);
    }


    private void ApplyLoadedData(SaveData saveData)
    {
        playerStateMachine.LoadFromData(saveData.player);

        foreach (var enemyData in saveData.enemies)
        {
            GameObject prefab = EnemySpawnerManager.Instance.GetPrefab(enemyData.type);

            if (prefab != null)
            {
                var enemyGO = Instantiate(prefab, enemyData.position, Quaternion.identity);
                var enemy = enemyGO.GetComponent<BaseEnemy>();
                enemy.currentHealth = enemyData.health;

                var zone = EnemySpawnerManager.Instance.GetZoneByID(enemyData.zoneID);
                if (zone != null)
                {
                    enemy.pointA = Instantiate(zone.patrolPointPrefab, enemyData.patrolA, Quaternion.identity);
                    enemy.pointB = Instantiate(zone.patrolPointPrefab, enemyData.patrolB, Quaternion.identity);
                    enemy.currentPoint = enemy.pointA.transform;

                    zone.OnEnemyDied(enemy);
                    enemy.assignedZone = zone;
                    EnemySpawnerManager.Instance.AddZone(enemy, zone); 
                }
            }
        }
    }
}

