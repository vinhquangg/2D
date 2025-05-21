using UnityEngine;

public class InventoryUITest : MonoBehaviour
{
    public InventoryUI inventoryUI;
    public ItemData testItem; // Kéo một ItemData vào trong Inspector

    private void Start()
    {
        // Ép thêm item test vào slot 0
        InventoryManager.Instance.inventorySlots[0].item = testItem;
        InventoryManager.Instance.inventorySlots[0].amount = 1;

        // Gọi update UI
        inventoryUI.UpdateUI();

        Debug.Log("[TEST] Injected item: " + testItem.itemName);
    }
}
