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
        // Đảm bảo chỉ có một instance của PlayerManager
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
        if (playerObj != null)
        {
            Destroy(playerObj);  // Nếu player đã tồn tại, hủy nó trước khi tạo mới
        }

        // Instantiate player tại vị trí spawn
        playerObj = Instantiate(playerPrefab, spawnPos, Quaternion.identity);

        // Khôi phục các thuộc tính của player
        playerHealth = playerObj.GetComponent<PlayerHealth>();
        playerEnergy = playerObj.GetComponent<PlayerEnergy>();

        // Bạn có thể khôi phục các thông tin khác từ SaveData nếu cần
        // Ví dụ: playerHealth.SetHealth(savedHealth);
        // Ví dụ: playerEnergy.SetEnergy(savedEnergy);
    }

    // Hàm này được gọi từ SaveLoadManager để load dữ liệu player
    public void LoadPlayerData(PlayerSaveData saveData)
    {
        Vector3 spawnPos = saveData.position;  // Lấy vị trí từ dữ liệu lưu trữ
        SpawnPlayer(spawnPos);  // Tạo player tại vị trí đã lưu
    }
}
