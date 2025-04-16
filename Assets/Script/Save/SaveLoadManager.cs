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
                    e.currentState = enemy.monsterState.monsterCurrentStateName;
                    enemyList.Add(e);
                }
            }
        }

        //Lưu SpawnZone
        List<SpawnZoneSaveData> zoneSaveDataList = new();
        var spawnZones = FindObjectsOfType<SpawnZone>();
        foreach (var zone in spawnZones)
        {
            SpawnZoneSaveData zoneData = zone.SaveData();
            zoneSaveDataList.Add(zoneData);
            Debug.Log($"Saving SpawnZone: {zoneData.zoneID}, SpawnInfos Count: {zoneData.spawnInfos.Count}");
        }

        SaveData saveData = new SaveData
        {
            player = playerData,
            enemies = enemyList,
            spawnZones = zoneSaveDataList
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
        // Load Player
        playerStateMachine.LoadFromData(saveData.player);

        // Load Enemy
        var allEnemies = FindObjectsOfType<BaseEnemy>();
        foreach (var enemy in allEnemies)
        {
            // Tìm data tương ứng với enemyID
            EnemySaveData data = saveData.enemies.Find(e => e.enemyID == enemy.enemyID);
            if (data != null)
            {
                if (enemy is ISaveable saveable)
                {
                    saveable.LoadData(data);
                }
            }
            else
            {
                Debug.LogWarning($"Không tìm thấy dữ liệu cho enemy ID: {enemy.enemyID}");
            }
        }

       var allZones = FindObjectsOfType<SpawnZone>();
        foreach (var zone in allZones)
        {
            // Tìm data tương ứng với zoneID
            SpawnZoneSaveData zoneData = saveData.spawnZones.Find(z => z.zoneID == zone.zoneID);
            if (zoneData != null)
            {
                foreach (var zoneInfo in zone.spawnInfos)
                {
                    // Tìm SpawnInfo tương ứng với enemyType
                    zone.LoadData(zoneData);
                    Debug.Log($"[LOAD] Zone {zone.zoneID} - EnemyType {zoneInfo.enemyType}: " +
                                    $"Spawned: {zoneInfo.spawnedCount}, Dead: {zoneInfo.deadCount}, Alive: {zoneInfo.currentAlive}");
                }
            }
            else
            {
                Debug.LogWarning($"Không tìm thấy dữ liệu cho zone ID: {zone.zoneID}");
            }
        }

    }



    private string GetSavePath()
    {
        return Path.Combine(Application.persistentDataPath, saveFileName);
    }
}
