using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    public GameObject playerPrefab;  

    private GameObject playerObj;  
    private PlayerHealth playerHealth;  
    private PlayerEnergy playerEnergy;  

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
            InitializePlayer(Vector3.zero);  // Spawn player tại vị trí mặc định nếu chưa có
        }

        // Đảm bảo PlayerStateMachine đã được gán cho player
        PlayerStateMachine stateMachine = playerObj.GetComponent<PlayerStateMachine>();
        if (stateMachine == null)
        {
            Debug.LogError("PlayerStateMachine is missing on the player object.");
            return null;
        }

        return stateMachine.GetDefaultPlayerData();  // Trả về dữ liệu mặc định của player
    }


    // Hàm này được gọi từ SaveLoadManager để load dữ liệu player
    public void LoadPlayerData(PlayerSaveData saveData)
    {
        SpawnPlayer(saveData.position); // Tạo nếu chưa có
        var stateMachine = playerObj.GetComponent<PlayerStateMachine>();
        stateMachine?.LoadFromData(saveData);
    }

    public void ResetPlayer()
    {
        if (playerObj != null)
        {
            Destroy(playerObj);
            playerObj = null;
            playerHealth = null;
            playerEnergy = null;
        }

        // Xoá dữ liệu tạm nếu có
        PlayerSaveTemp.tempData = null;
    }

}
