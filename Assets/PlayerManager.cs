using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    [Header("Player Prefab")]
    public GameObject playerPrefab;  // Prefab của player

    private GameObject playerObj;  // Đối tượng player trong scene
    private PlayerHealth playerHealth;  // Script quản lý máu của player
    private PlayerEnergy playerEnergy;  // Script quản lý năng lượng của player

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


    // Hàm này được gọi để instantiate player
    public void SpawnPlayer(Vector3 spawnPos)
    {
        if (playerObj == null)
        {
            playerObj = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
            DontDestroyOnLoad(playerObj);

            playerHealth = playerObj.GetComponent<PlayerHealth>();
            playerEnergy = playerObj.GetComponent<PlayerEnergy>();
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


    // Hàm này được gọi từ SaveLoadManager để load dữ liệu player
    public void LoadPlayerData(PlayerSaveData saveData)
    {
        SpawnPlayer(saveData.position); // Tạo nếu chưa có
        var stateMachine = playerObj.GetComponent<PlayerStateMachine>();
        stateMachine?.LoadFromData(saveData);
    }

}
