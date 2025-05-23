using UnityEngine;

public class BossTele : MonoBehaviour
{

    public Transform spawnPoint;

    public SceneName targetScene = SceneName.BossFight;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (spawnPoint == null)
        {
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

        }

       
        SceneLoader.instance.LoadScene(targetScene);

        // Tuỳ bạn có cần save ngay không:
        //SaveLoadManager.instance.SaveGame();
    }
}
