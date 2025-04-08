using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public PlayerStateMachine playerStateMachine;
    public PlayerCombat playerCombat;
    private Player playerData;
    //public PlayerHealth playerHealth;
    //public PlayerEnergy playerEnergy;

    public static SaveLoadManager instance { get;private set; }

    private Dictionary<string, System.Func<IPlayerState>> stateFactory;

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
        stateFactory = new Dictionary<string, System.Func<IPlayerState>>()
        {
            { "Idle", () => new IdleState(playerStateMachine) },
            { "MoveState", () => new MoveState(playerStateMachine) },
            { "Attack", () => new AttackState(playerStateMachine) },
        };
        NewGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
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
        //string stateName = playerStateMachine.currentStateName;
        float health = playerStateMachine.playerData.maxHealth;
        float energy = playerStateMachine.playerCombat.currentEnergy;
        string State = "Idle";
        this.playerData = new Player(startPos,health,State,energy);

        playerStateMachine.transform.position = playerData.position;
        playerCombat.currentHealth = playerData.health;
        playerCombat.currentEnergy = playerData.energy;

        playerCombat.GetComponent<PlayerHealth>()?.UpdateHealthBarPlayer(playerData.health, playerStateMachine.playerData.maxHealth);
        playerCombat.GetComponent<PlayerEnergy>()?.UpdateEnergySlider();

        playerStateMachine.SwitchState(new IdleState(playerStateMachine));
    }

    private void SaveGame()
    {
        Vector3 pos = playerStateMachine.transform.position;
        string stateName = playerStateMachine.currentStateName;
        float health = playerCombat.currentHealth;
        float energy = playerCombat.currentEnergy;

        Player data = new Player(pos, health , stateName, energy);
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("player_save", json);
        Debug.Log("Saved: " + json);
    }

    private void LoadGame()
    {
        if (!PlayerPrefs.HasKey("player_save"))
        {
            Debug.Log("No save data found. Starting new game.");
            NewGame(); 
            return;
        }
        string json = PlayerPrefs.GetString("player_save");
        PlayerPrefs.DeleteKey("player_save");

        Player data = JsonUtility.FromJson<Player>(json);

        playerStateMachine.transform.position = data.position;
        playerCombat.currentHealth = data.health;
        playerCombat.currentEnergy = data.energy;

        playerCombat.GetComponent<PlayerHealth>()?.UpdateHealthBarPlayer(data.health, playerStateMachine.playerData.maxHealth);
        playerCombat.GetComponent<PlayerEnergy>()?.UpdateEnergySlider();

        if (stateFactory.TryGetValue(data.currentState, out var createState))
        {
            playerStateMachine.SwitchState(createState());
        }
        else
        {
            //Debug.LogWarning("Unknown state: " + data.currentState + ". Switching to Idle.");
            playerStateMachine.SwitchState(new IdleState(playerStateMachine));
        }

        Debug.Log("Loaded: " + json);
    }


}

