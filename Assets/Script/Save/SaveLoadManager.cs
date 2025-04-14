using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    [SerializeField] private PlayerStateMachine playerStateMachine;
    [SerializeField] private List<BaseEnemy> allEnemies = new List<BaseEnemy>();
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

        allEnemies = new List<BaseEnemy>(FindObjectsOfType<BaseEnemy>());

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

    public void AddEnemyToList(BaseEnemy enemy)
    {
        if (!allEnemies.Contains(enemy))
        {
            allEnemies.Add(enemy);
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

        var enemies = FindObjectsOfType<BaseEnemy>();
        foreach (var enemy in enemies)
        {
            playerStateMachine.SwitchState(new IdleState(playerStateMachine));
            var found = saveData.enemies.Find(e => e.type == enemy.enemyType);
            if (found != null && enemy is ISaveable saveable)
            {
                saveable.LoadData(found);
            }
        }


        //foreach (var enemyData in saveData.enemies)
        //{
        //    GameObject prefab = EnemySpawnerManager.Instance.GetPrefab(enemyData.type);

        //    if (prefab != null)
        //    {
        //        var enemyGO = Instantiate(prefab, enemyData.position, Quaternion.identity);
        //        var enemy = enemyGO.GetComponent<BaseEnemy>();

        //        enemy.currentHealth = enemyData.health;
        //        enemy.healthBar.UpdateHealBar(enemy.currentHealth, enemy.monsterState.monsterData.maxHealth); // Cập nhật thanh máu

        //        var zone = EnemySpawnerManager.Instance.GetZoneByID(enemyData.zoneID);

        //        if (zone != null)
        //        {
        //            enemy.pointA = Instantiate(zone.patrolPointPrefab, enemyData.patrolA, Quaternion.identity);
        //            enemy.pointB = Instantiate(zone.patrolPointPrefab, enemyData.patrolB, Quaternion.identity);

        //            enemy.currentPoint = enemy.pointA.transform;
        //            enemy.assignedZone = zone;

        //            // Thêm zone vào EnemySpawnerManager
        //            EnemySpawnerManager.Instance.AddZone(enemy, zone);
        //            zone.UpdateZoneInfo(enemy);

        //            // Nếu quái đã chết (health <= 0), gọi TakeDamage để xử lý trạng thái chết
        //            if (enemyData.health <= 0)
        //            {
        //                enemy.TakeDamage(9999, Vector2.zero); // Quái chết ngay lập tức khi load lại
        //            }
        //        }
        //        else
        //        {
        //            Debug.LogError($"Zone with ID {enemyData.zoneID} not found.");
        //        }
        //    }
        //}
    }


}

