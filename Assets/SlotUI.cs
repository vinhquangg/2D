using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI amountText;

    public void UpdateSlot(SlotClass slot)
    {
        if (slot.item != null)
        {
            icon.sprite = slot.item.itemIcon;
            icon.enabled = true;
            amountText.text = slot.amount.ToString();
        }
        else
        {
            icon.enabled = false;
            amountText.text = "";
        }
    }
}
