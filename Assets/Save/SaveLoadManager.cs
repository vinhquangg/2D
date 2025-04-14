using System;
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
    }
    void Start()
    {
        NewGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            SaveGame();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadGame();
        }
    }

    private void NewGame()
    {
        Vector3 startPos = Vector3.zero;
        float health = playerStateMachine.playerData.maxHealth;
        float energy = playerStateMachine.playerCombat.playerEnergy.GetMaxEnergy();
        string State = "IdleState";
        playerData = new PlayerSaveData(startPos,health,State,energy);

        playerStateMachine.transform.position = playerData.position;
        playerStateMachine.playerCombat.currentHealth = playerData.health;
        playerStateMachine.playerCombat.currentEnergy = playerData.energy;

        ApplyPlayerData(playerData);

        playerStateMachine.SwitchState(new IdleState(playerStateMachine));
    }

    private void SaveGame()
    {
        //Save Player
        Vector3 pos = playerStateMachine.transform.position;
        string stateName = playerStateMachine.currentStateName;
        float health = playerStateMachine.playerCombat.currentHealth;
        float energy = playerStateMachine.playerCombat.currentEnergy;
        PlayerSaveData playerData = new PlayerSaveData(pos, health , stateName, energy);
       
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
        Debug.Log("Saved All: " + json);
    }

    private void LoadGame()
    {
        if (!PlayerPrefs.HasKey("saveData_save"))
        {
            Debug.Log("No save data found. Starting new game.");
            NewGame(); 
            return;
        }
        string json = PlayerPrefs.GetString("saveData_save");
        SaveData saveData = JsonUtility.FromJson<SaveData>(json);
        //PlayerPrefs.DeleteKey("player_save");

        //Load Player
        ApplyPlayerData(saveData.player);


        //Load Monster
        var enemies = FindObjectsOfType<BaseEnemy>();
        foreach( var enemy in enemies )
        {
            var found = saveData.enemies.Find(e => e.type == enemy.enemyType);
            if(found != null && enemy is ISaveable saveable )
            {
                saveable.LoadData(found);
            }
        }
        Debug.Log("Loaded All: " + json);
        //Debug.Log("Loaded: " + json);
    }

    private void ApplyPlayerData(PlayerSaveData data)
    {
        playerStateMachine.transform.position = data.position;
        playerStateMachine.playerCombat.currentHealth = data.health;
        playerStateMachine.playerCombat.currentEnergy = data.energy;

        playerStateMachine.playerCombat.GetComponent<PlayerHealth>()?.UpdateHealthBarPlayer(data.health, playerStateMachine.playerData.maxHealth);
        playerStateMachine.playerCombat.GetComponent<PlayerEnergy>()?.UpdateEnergySlider();

        if (playerStateMachine.stateFactory.TryGetValue(data.currentState, out var createState))
        {
            playerStateMachine.SwitchState(createState());
        }
        else
        {
            playerStateMachine.SwitchState(new IdleState(playerStateMachine));
        }
    }
}

