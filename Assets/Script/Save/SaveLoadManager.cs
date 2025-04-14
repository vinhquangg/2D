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
        List<SpawnZoneSaveData> zoneSaveDataList = new();
        var spawnZones = FindObjectsOfType<SpawnZone>();
        foreach (var zone in spawnZones)
        {
            SpawnZoneSaveData zoneData = zone.SaveData();  // Gọi phương thức SaveData từ SpawnZone
            zoneSaveDataList.Add(zoneData);

            // Debug để chắc chắn rằng SpawnZone được lưu
            Debug.Log($"Saving SpawnZone: {zoneData.zoneID}, SpawnInfos Count: {zoneData.spawnInfos.Count}");
        }

        // Tạo và lưu toàn bộ dữ liệu
        SaveData saveData = new SaveData
        {
            player = playerData,
            enemies = enemyList,
            spawnZones = zoneSaveDataList
        };

        string json = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString("saveData_save", json);
        Debug.Log("Game Saved: " + json);  // Debug log để kiểm tra toàn bộ dữ liệu
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

        // Tải Player
        playerStateMachine.LoadFromData(saveData.player);

        // Tải Enemy
        Dictionary<EnemyType, EnemySaveData> enemyDataDictionary = new();
        foreach (var enemyData in saveData.enemies)
        {
            enemyDataDictionary[enemyData.type] = enemyData;
        }

        // Tải SpawnZone
        foreach (var zoneData in saveData.spawnZones)
        {
            var zone = FindObjectOfType<SpawnZone>();  // Tìm SpawnZone để tải dữ liệu
            if (zone != null)
            {
                zone.LoadData(zoneData);  // Gọi phương thức LoadData để khôi phục dữ liệu
            }
        }

        // Debug toàn bộ dữ liệu sau khi tải
        Debug.Log("Game Loaded: " + json);  // Log dữ liệu đã tải
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
            // Lấy prefab của enemy từ type đã lưu
            GameObject prefab = EnemySpawnerManager.Instance.GetPrefab(enemyData.type);

            if (prefab != null)
            {
                // Spawn enemy tại vị trí đã lưu
                var enemyGO = Instantiate(prefab, enemyData.position, Quaternion.identity);
                var enemy = enemyGO.GetComponent<BaseEnemy>();

                // Cập nhật các thông tin của enemy từ dữ liệu đã lưu
                enemy.currentHealth = enemyData.health;

                // Tìm zone dựa trên zoneID đã lưu
                var zone = EnemySpawnerManager.Instance.GetZoneByID(enemyData.zoneID);

                if (zone != null)
                {
                    // Khôi phục lại patrol points
                    enemy.pointA = Instantiate(zone.patrolPointPrefab, enemyData.patrolA, Quaternion.identity);
                    enemy.pointB = Instantiate(zone.patrolPointPrefab, enemyData.patrolB, Quaternion.identity);

                    // Gán currentPoint của enemy
                    enemy.currentPoint = enemy.pointA.transform;

                    // Gọi phương thức OnEnemyDied để cập nhật lại thông tin số lượng quái sống trong zone
                    zone.OnEnemyDied(enemy);

                    // Gán zone cho enemy
                    enemy.assignedZone = zone;

                    // Thêm enemy vào zone thông qua EnemySpawnerManager
                    EnemySpawnerManager.Instance.AddZone(enemy, zone);

                    // Cập nhật thông tin khác liên quan đến zone (ví dụ: spawn lại enemy nếu cần)
                    zone.UpdateZoneInfo(enemy); // Phương thức này cần được bạn viết trong `Zone` để cập nhật thông tin zone
                }
                else
                {
                    Debug.LogError($"Zone with ID {enemyData.zoneID} not found.");
                }
            }
        }
    }

}

