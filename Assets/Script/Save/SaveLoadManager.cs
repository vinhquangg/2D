using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    //[SerializeField] private PlayerStateMachine playerStateMachine;
    private string saveFileName = "save_game.json";

    public static SaveLoadManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than one SaveLoadManager in the scene");
            return;
        }
        instance = this;

        GameObject playerObj = PlayerManager.Instance.GetCurrentPlayer();

        //if (playerObj != null)
        //{
        //    //playerStateMachine = playerObj.GetComponent<PlayerStateMachine>();
        //}
        //else
        //{
        //    Debug.LogWarning("Không tìm thấy Player trong scene!");
        //}
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
        PlayerSaveData playerData = new PlayerSaveData(); // hoặc dùng default
        PlayerManager.Instance.SpawnPlayer(playerData.position); // Gọi spawn

        //PlayerManager.Instance.LoadPlayerData(playerData);
        //GameObject playerObj = PlayerManager.Instance.GetCurrentPlayer();
        //playerStateMachine = playerObj.GetComponent<PlayerStateMachine>();
        //playerStateMachine.LoadFromData(playerData);
        //playerStateMachine.SwitchState(new IdleState(playerStateMachine));


        //PlayerSaveData playerData = playerStateMachine.GetDefaultPlayerData();
        //PlayerManager.Instance.SpawnPlayer(playerData.position); // Tạo player tại vị trí mặc định
        //playerStateMachine.SwitchState(new IdleState(playerStateMachine));

        //playerStateMachine.LoadFromData(playerData);
        // Tạo player tại vị trí khởi tạo (spawn position, ví dụ là vị trí ban đầu)
        // Chuyển trạng thái của player về IdleState (hoặc trạng thái ban đầu của bạn)
        //PlayerSaveData playerData = playerStateMachine.GetDefaultPlayerData();
        //playerStateMachine.LoadFromData(playerData);
        //playerStateMachine.SwitchState(new IdleState(playerStateMachine));
    }

    public void SaveGame()
    {
        // Lưu Player
        //PlayerSaveData playerData = playerStateMachine.GetPlayerSaveData();

        PlayerSaveData playerData = PlayerManager.Instance.GetPlayerSaveData();

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
            //Debug.Log($"Saving SpawnZone: {zoneData.zoneID}, SpawnInfos Count: {zoneData.spawnInfos.Count}");
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
        LoadGame();
    }

    private void ApplyLoadedData(SaveData saveData)
    {
        // Load Player
        PlayerManager.Instance.LoadPlayerData(saveData.player);

        //PlayerManager.Instance.SpawnPlayer(saveData.player.position);

        //GameObject playerObj = PlayerManager.Instance.GetCurrentPlayer();
        //playerStateMachine = playerObj.GetComponent<PlayerStateMachine>();
        //playerStateMachine.LoadFromData(saveData.player);

        // Load Enemy
        var allEnemies = FindObjectsOfType<BaseEnemy>();
        foreach (var enemy in allEnemies)
        {
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
            SpawnZoneSaveData zoneData = saveData.spawnZones.Find(z => z.zoneID == zone.zoneID);
            if (zoneData != null)
            {
                // Thay thế load dữ liệu từ spawnInfos bằng các giá trị trực tiếp từ zoneData
                zone.LoadData(zoneData);
                Debug.Log($"[LOAD] Zone {zone.zoneID} - EnemyType {zoneData.zoneEnemyType}: " +
                            $"Spawned: {zoneData.spawnedCount}, Dead: {zoneData.deadCount}, Alive: {zoneData.currentAlive}");
            }
            else
            {
                Debug.LogWarning($"Không tìm thấy dữ liệu cho zone ID: {zone.zoneID}");
            }
        }

        // Xác định vị trí Player cần load về
        SpawnZone currentZone = null;
        Vector3 playerSavedPos = saveData.player.position;
        foreach (var zone in allZones)
        {
            BoxCollider2D box = zone.GetComponent<BoxCollider2D>();
            if (box != null && box.OverlapPoint(playerSavedPos))
            {
                currentZone = zone;
                break;
            }
        }

        if (currentZone != null && currentZone.IsZoneUncleared())
        {
            // Zone chưa clear → đưa player ra cửa zone
            PlayerManager.Instance.GetCurrentPlayer().transform.position = currentZone.GetEntryPointOutsideZone();
            Debug.Log($"[LOAD] Player được đưa ra entry point của zone chưa clear: {currentZone.zoneID}");
        }
        else
        {
            // Zone đã clear → đưa player về đúng vị trí khi save
            PlayerManager.Instance.GetCurrentPlayer().transform.position = saveData.player.position;
            Debug.Log($"[LOAD] Player trở về đúng vị trí đã lưu");
        }

    }
    private string GetSavePath()
    {
        return Path.Combine(Application.persistentDataPath, saveFileName);
    }
}
