using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    [SerializeField] private PlayerStateMachine playerStateMachine;
    private string saveFileName = "save_game.json";

    public static SaveLoadManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than one SaveLoadManager in the scene");
        }
        instance = this;

        if (playerStateMachine == null)
        {
            playerStateMachine = FindObjectOfType<PlayerStateMachine>();
        }
    }

    private void Start()
    {
        string path = GetSavePath();
        if (!File.Exists(path))
        {
            NewGame();
        }
        else
        {
            LoadGame();
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
        // Lưu Player
        PlayerSaveData playerData = playerStateMachine.GetPlayerSaveData();

        // Lưu Enemy
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

        // Lưu SpawnZone
        //List<SpawnZoneSaveData> zoneSaveDataList = new();
        //var spawnZones = FindObjectsOfType<SpawnZone>();
        //foreach (var zone in spawnZones)
        //{
        //    SpawnZoneSaveData zoneData = zone.SaveData();
        //    zoneSaveDataList.Add(zoneData);
        //    Debug.Log($"Saving SpawnZone: {zoneData.zoneID}, SpawnInfos Count: {zoneData.spawnInfos.Count}");
        //}

        SaveData saveData = new SaveData
        {
            player = playerData,
            enemies = enemyList,
            //spawnZones = zoneSaveDataList
        };

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(GetSavePath(), json);
        Debug.Log("Game saved to file: " + GetSavePath());
    }

    public void LoadGame()
    {
        string path = GetSavePath();
        if (!File.Exists(path))
        {
            Debug.LogWarning("Save file not found. Starting new game.");
            NewGame();
            return;
        }

        string json = File.ReadAllText(path);
        SaveData saveData = JsonUtility.FromJson<SaveData>(json);
        ApplyLoadedData(saveData);

        Debug.Log("Game loaded from file: " + path);
    }

    public void LoadAfterSceneLoaded()
    {
        StartCoroutine(DelayThenLoadData());
    }

    private IEnumerator DelayThenLoadData()
    {
        yield return null;
        LoadGame(); // Vì chỉ còn dùng file nên gọi lại hàm LoadGame
    }

    private void ApplyLoadedData(SaveData saveData)
    {
        playerStateMachine.LoadFromData(saveData.player);

        //foreach (var enemyData in saveData.enemies)
        //{
        //    GameObject prefab = EnemySpawnerManager.Instance.GetPrefab(enemyData.type);

        //    if (prefab != null)
        //    {
        //        var enemyGO = Instantiate(prefab, enemyData.position, Quaternion.identity);
        //        var enemy = enemyGO.GetComponent<BaseEnemy>();
        //        enemy.currentHealth = enemyData.health;

        //        var zone = EnemySpawnerManager.Instance.GetZoneByID(enemyData.zoneID);

        //        if (zone != null)
        //        {
        //            enemy.pointA = Instantiate(zone.patrolPointPrefab, enemyData.patrolA, Quaternion.identity);
        //            enemy.pointB = Instantiate(zone.patrolPointPrefab, enemyData.patrolB, Quaternion.identity);
        //            enemy.currentPoint = enemy.pointA.transform;

        //            zone.OnEnemyDied(enemy);
        //            enemy.assignedZone = zone;
        //            EnemySpawnerManager.Instance.AddZone(enemy, zone);
        //            zone.UpdateZoneInfo(enemy);
        //        }
        //        else
        //        {
        //            Debug.LogError($"Zone with ID {enemyData.zoneID} not found.");
        //        }
        //    }
        //}

        //foreach (var zoneData in saveData.spawnZones)
        //{
        //    var zone = FindObjectOfType<SpawnZone>();
        //    if (zone != null)
        //    {
        //        zone.LoadData(zoneData);
        //    }
        //}
    }

    private string GetSavePath()
    {
        return Path.Combine(Application.persistentDataPath, saveFileName);
    }
}
