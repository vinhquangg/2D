using UnityEngine;

public class BossFlameTele : MonoBehaviour
{
    [Header("Spawn Point trong scene tiếp theo")]
    public Transform spawnPoint;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (spawnPoint == null)
            {
                Debug.LogWarning("SpawnPoint chưa được gán!");
                return;
            }

            var playerData = PlayerManager.Instance.GetPlayerSaveData();
            var inventoryData = InventoryManager.Instance.GetInventoryData();
            playerData.position = spawnPoint.position;
            PlayerSaveTemp.tempData = playerData;
            PlayerSaveTemp.tempInventory = inventoryData;
            if (PlayerSaveTemp.tempInventory != null)
            {
                InventoryManager.Instance.LoadInventoryData(PlayerSaveTemp.tempInventory);
                Debug.Log("[SceneLoader] Inventory đã load từ PlayerSaveTemp.");
            }
            Debug.Log($"[Tele] Lưu dữ liệu Player và chuyển scene: {spawnPoint.position}");
            SceneLoader.instance.LoadScene(SceneName.BossFight2);
            SaveLoadManager.instance.SaveGame();
        }
    }
}
