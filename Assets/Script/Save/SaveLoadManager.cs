using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    //[SerializeField] private PlayerStateMachine playerStateMachine;
    private string saveFileName = "save_game.json";
    public static bool IsLoading = false;
    public static SaveLoadManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than one SaveLoadManager in the scene");
            return;
        }
        instance = this;
    }

    public void NewGame()
    {
        PlayerSaveData playerData = new PlayerSaveData(); 
        PlayerManager.Instance.SpawnPlayer(playerData.position); 
    }

    public void SaveGame()
    {
        PlayerSaveData playerData = PlayerManager.Instance.GetPlayerSaveData();

        InventoryData inventoryData = InventoryManager.Instance.GetInventoryData();

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

                    e.currentState = enemy.isBoss
                        ? enemy.bossState?.bossCurrentStateName
                        : enemy.monsterState?.monsterCurrentStateName;

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

        }

        SaveData saveData = new SaveData
        {
            player = playerData,
            enemies = enemyList,
            spawnZones = zoneSaveDataList,
            inventory = inventoryData
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
        IsLoading = true;
        yield return null;
        LoadGame();
        IsLoading = false;
    }

    private void ApplyLoadedData(SaveData saveData)
    {
        // Load Player
        PlayerManager.Instance.LoadPlayerData(saveData.player,true);

        // Load Enemies
        var allEnemies = FindObjectsOfType<BaseEnemy>();
        foreach (var enemy in allEnemies)
        {
            var data = saveData.enemies.Find(e => e.enemyID == enemy.enemyID);
            if (data != null && enemy is ISaveable saveable)
            {
                saveable.LoadData(data);
            }
        }

        // Load SpawnZones
        var allZones = FindObjectsOfType<SpawnZone>();
        foreach (var zone in allZones)
        {
            var zoneData = saveData.spawnZones.Find(z => z.zoneID == zone.zoneID);
            if (zoneData != null)
            {
                zone.LoadData(zoneData);
                Debug.Log($"[LOAD] Zone {zone.zoneID} - EnemyType {zoneData.zoneEnemyType}: " +
                          $"Spawned: {zoneData.spawnedCount}, Dead: {zoneData.deadCount}, Alive: {zoneData.currentAlive}");
            }
        }

        // Kiểm tra zone hiện tại player đang đứng
        SpawnZone currentZone = null;
        Vector3 playerSavedPos = saveData.player.position;

        foreach (var zone in allZones)
        {
            if (zone.GetComponent<BoxCollider2D>().OverlapPoint(playerSavedPos))
            {
                currentZone = zone;
                break;
            }
        }

        // Xác định vị trí spawn
        Vector3 spawnPosition = playerSavedPos;

        if (currentZone != null && currentZone.IsZoneUncleared())
        {
            spawnPosition = currentZone.GetEntryPointOutsideZone();
            Debug.Log($"[LOAD] Player bị đẩy ra entry point vì zone {currentZone.zoneID} chưa clear.");
        }
        else
        {
            Debug.Log($"[LOAD] Player trở về đúng vị trí đã lưu: {playerSavedPos}");
        }

        // Set vị trí cuối cùng
        PlayerManager.Instance.GetCurrentPlayer().transform.position = spawnPosition;

        if (saveData.inventory != null)
        {
            InventoryManager.Instance.LoadInventoryData(saveData.inventory);
            Debug.Log("[LOAD] Inventory đã được nạp.");
        }
        else
        {
            Debug.LogWarning("Không có InventoryData trong save.");
        }
    }

    private string GetSavePath()
    {
        return Path.Combine(Application.persistentDataPath, saveFileName);
    }
}
