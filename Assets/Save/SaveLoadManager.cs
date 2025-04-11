using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    [SerializeField] private PlayerStateMachine playerStateMachine;
    private PlayerSaveData playerData;

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
        PlayerSaveData playerData = playerStateMachine.GetPlayerSaveData();
        //Save Enemy
        List<EnemySaveData> enemyList = new();
        var allEnemies = FindObjectsOfType<BaseEnemy>();
        foreach( var enemy in allEnemies )
        {
            if(enemy is ISaveable saveable)
            {
                var monsterData = saveable.SaveData();

                if(monsterData is EnemySaveData e)
                {
                    enemyList.Add(e);
                }
            }
        }

        SaveData saveData = new SaveData
        {
            player = playerData,
            enemies = enemyList
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


        //Load Player
        playerStateMachine.LoadFromData(saveData.player);


        //Load Monster
        var enemies = FindObjectsOfType<BaseEnemy>();
        foreach (var enemy in enemies)
        {
            var found = saveData.enemies.Find(e => e.type == enemy.enemyType);
            if (found != null && enemy is ISaveable saveable)
            {
                saveable.LoadData(found);
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

        var enemies = FindObjectsOfType<BaseEnemy>();
        foreach (var enemy in enemies)
        {
            var found = saveData.enemies.Find(e => e.type == enemy.enemyType);
            if (found != null && enemy is ISaveable saveable)
            {
                saveable.LoadData(found);
            }
        }
    }


    
}

