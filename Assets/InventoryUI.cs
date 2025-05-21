using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    private List<SlotUI> slotUIList;

    private void Start()
    {
        slotUIList = GetComponentsInChildren<SlotUI>(true).ToList();

        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryUpdated -= UpdateUI; 
            InventoryManager.Instance.OnInventoryUpdated += UpdateUI;
        }
        else
        {
            StartCoroutine(WaitForInventoryManager());
        }
    }

    private IEnumerator WaitForInventoryManager()
    {
        while (InventoryManager.Instance == null)
            yield return null;

        InventoryManager.Instance.OnInventoryUpdated -= UpdateUI;
        InventoryManager.Instance.OnInventoryUpdated += UpdateUI;
        UpdateUI();
    }

    private void OnEnable()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryUpdated -= UpdateUI;
            InventoryManager.Instance.OnInventoryUpdated += UpdateUI;
        }
    }

    private void OnDisable()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryUpdated -= UpdateUI;
        }
    }


    public static void ForceUpdate()
    {
        var ui = FindObjectOfType<InventoryUI>();
        if (ui != null)
            ui.UpdateUI();
    }

    public SlotUI GetSlotUI(int index)
    {
        if (index >= 0 && index < slotUIList.Count)
            return slotUIList[index];
        return null;
    }



    public void UpdateUI()
    {
        Debug.Log($"[UpdateUI] Gọi cập nhật - inventorySlots.Length = {InventoryManager.Instance.inventorySlots.Length}, slotUIList.Count = {slotUIList.Count}");

        int length = Mathf.Min(InventoryManager.Instance.inventorySlots.Length, slotUIList.Count);

        for (int i = 0; i < length; i++)
        {
            var slot = InventoryManager.Instance.inventorySlots[i];
            slotUIList[i].UpdateSlot(slot);
            Debug.Log($"[UpdateUI] Slot {i}: {slot.item?.itemName} - {slot.amount}");
        }

        for (int i = length; i < slotUIList.Count; i++)
        {
            slotUIList[i].icon.enabled = false;
            slotUIList[i].amountText.text = "";
        }
    }

}



