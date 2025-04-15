//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class EnemySpawnerManager : MonoBehaviour
//{
//    public static EnemySpawnerManager Instance { get; private set; }
//    private Dictionary<BaseEnemy, SpawnZone> spawnZones = new Dictionary<BaseEnemy, SpawnZone>();
//    public List<EnemyTypePrefabPair> enemyPrefabList;
//    private void Awake()
//    {
//        Instance = this;
//    }

//    public void AddZone(BaseEnemy enemy, SpawnZone zone)
//    {
//        if (!spawnZones.ContainsKey(enemy))
//        {
//            spawnZones.Add(enemy, zone);
//        }
//    }

//    public void EnemyDied(BaseEnemy enemy)
//    {
//        if (spawnZones.TryGetValue(enemy, out SpawnZone zone))
//        {
//            zone.OnEnemyDied(enemy);
//            spawnZones.Remove(enemy);
//        }
//    }

//    public SpawnZone GetZoneByID(string id)
//    {
//        foreach (var zone in FindObjectsOfType<SpawnZone>())
//        {
//            if (zone.zoneID == id)
//                return zone;
//        }
//        return null;
//    }

//    public GameObject GetPrefab(EnemyType type)
//    {
//        foreach (var pair in enemyPrefabList)
//        {
//            if (pair.enemyType == type)
//                return pair.prefab;
//        }
//        Debug.LogWarning($"[EnemySpawnerManager] Không tìm thấy prefab cho EnemyType: {type}");
//        return null;
//    }

//    // Sửa lại để nhận enemy làm tham số và thêm vào SaveLoadManager
//    //public void AddEnemyToSave(BaseEnemy enemy)
//    //{
//    //    if (enemy != null)
//    //    {
//    //        saveLoadManager.AddEnemyToList(enemy); // Thêm quái vào danh sách quản lý của SaveLoadManager
//    //    }
//    //}

//    // Phương thức spawn quái
//    public BaseEnemy SpawnEnemy(EnemyType type, Vector3 spawnPosition, SpawnZone zone)
//    {
//        GameObject enemyPrefab = GetPrefab(type);
//        if (enemyPrefab != null)
//        {
//            GameObject enemyObj = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
//            BaseEnemy enemy = enemyObj.GetComponent<BaseEnemy>();

//            // Thêm quái vào SpawnZone
//            AddZone(enemy, zone);

//            // Thêm quái vào danh sách SaveLoadManager
//            //AddEnemyToSave(enemy);

//            return enemy;
//        }
//        return null;
//    }

//}
