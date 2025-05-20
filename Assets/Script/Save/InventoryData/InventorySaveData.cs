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
}
