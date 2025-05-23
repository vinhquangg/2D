using System.Collections.Generic;

[System.Serializable]
public class InventorySaveData
{
    public ItemType itemType;
    public int amount;

    public InventorySaveData(ItemType itemType, int amount)
    {
        this.itemType = itemType;
        this.amount = amount;
    }
}

[System.Serializable]
public class InventoryData
{
    public List<InventorySaveData> slots = new List<InventorySaveData>();

    public InventoryData() { }

    public static InventoryData GetDefaultInventory()
    {
        InventoryData defaultInventory = new InventoryData();

        defaultInventory.slots.Add(new InventorySaveData(ItemType.HealthPotion, 3));
        defaultInventory.slots.Add(new InventorySaveData(ItemType.EnergyPotion, 2));

        return defaultInventory;
    }
}

