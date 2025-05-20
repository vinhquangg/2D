using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<ItemData> allItems;
    public static InventoryManager Instance;
    private Dictionary<ItemType, ItemData> itemMap = new Dictionary<ItemType, ItemData>();
    public SlotClass[] inventorySlots = new SlotClass[2];

    public Action OnInventoryUpdated;

    private void Awake()
    {
        Instance = this;
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            inventorySlots[i] = new SlotClass();
        }

        // Tạo map enum → ItemData
        foreach (var item in allItems)
        {
            if (!itemMap.ContainsKey(item.itemType))
                itemMap.Add(item.itemType, item);
        }
    }

    public void AddItem(ItemData item, int amount)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].item == item && item.canStack)
            {
                inventorySlots[i].amount += amount;
                OnInventoryUpdated?.Invoke();
                return;
            }
        }

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].item == null)
            {
                inventorySlots[i].item = item;
                inventorySlots[i].amount = amount;
                OnInventoryUpdated?.Invoke();
                return;
            }
        }

        Debug.Log("Inventory full!");
    }

    public InventoryData GetInventoryData()
    {
        InventoryData data = new InventoryData();

        foreach (var slot in inventorySlots)
        {
            if (slot.item != null)
            {
                data.slots.Add(new InventorySaveData(slot.item.itemType, slot.amount));
            }
        }

        return data;
    }



    public void LoadInventoryData(InventoryData data)
    {
        foreach (var savedSlot in data.slots)
        {
            if (itemMap.TryGetValue(savedSlot.itemType, out ItemData itemData))
            {
                AddItem(itemData, savedSlot.amount);
            }
            else
            {
                Debug.LogWarning($"ItemType {savedSlot.itemType} not found in itemMap!");
            }
        }
    }

}
