using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    private List<SlotUI> slotUIList;

    private void Awake()
    {
        slotUIList = GetComponentsInChildren<SlotUI>().ToList();
    }

    private void OnEnable()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryUpdated += UpdateUI;
            UpdateUI();
        }
    }

    private void OnDisable()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryUpdated -= UpdateUI;
        }
    }

    public void UpdateUI()
    {
        int length = Mathf.Min(InventoryManager.Instance.inventorySlots.Length, slotUIList.Count);
        for (int i = 0; i < length; i++)
        {
            slotUIList[i].UpdateSlot(InventoryManager.Instance.inventorySlots[i]);
        }
        for (int i = length; i < slotUIList.Count; i++)
        {
            slotUIList[i].icon.enabled = false;
            slotUIList[i].amountText.text = "";
        }
    }
}
