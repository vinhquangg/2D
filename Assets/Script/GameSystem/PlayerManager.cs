using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    public GameObject playerPrefab;

    private GameObject playerObj;
    private PlayerHealth playerHealth;
    private PlayerEnergy playerEnergy;
    private PlayerSoul playerSoul;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SpawnPlayer(Vector3 spawnPos)
    {
        if (playerObj == null)
        {
            InitializePlayer(spawnPos);
        }
        else
        {
            playerObj.transform.position = spawnPos;
        }
    }

    public void InitializePlayer(Vector3 spawnPos)
    {
        if (playerObj == null)
        {
            playerObj = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
            DontDestroyOnLoad(playerObj);

            playerHealth = playerObj.GetComponent<PlayerHealth>();
            playerEnergy = playerObj.GetComponent<PlayerEnergy>();
            playerSoul = playerObj.GetComponent<PlayerSoul>();
        }
        else
        {
            playerObj.transform.position = spawnPos;
        }
    }

    public GameObject GetCurrentPlayer()
    {
        return playerObj;
    }

    public PlayerSaveData GetPlayerSaveData()
    {
        return playerObj.GetComponent<PlayerStateMachine>()?.GetPlayerSaveData();
    }

    public PlayerSaveData GetDefaultPlayer()
    {
        if (playerObj == null)
        {
            InitializePlayer(Vector3.zero);
        }

        var stateMachine = playerObj.GetComponent<PlayerStateMachine>();
        if (stateMachine == null)
        {
            Debug.LogError("PlayerStateMachine is missing on the player object.");
            return null;
        }

        return stateMachine.GetDefaultPlayerData(); 
    }

    public void LoadPlayerData(PlayerSaveData saveData, bool overridePosition = true)
    {
        if (overridePosition)
        {
            SpawnPlayer(saveData.position);
        }
        var stateMachine = playerObj.GetComponent<PlayerStateMachine>();
        stateMachine?.LoadFromData(saveData);
    }
}
