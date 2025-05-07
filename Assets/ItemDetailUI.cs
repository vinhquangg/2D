using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetailUI : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;
    public GameObject panel;


    public void ShowItem(ItemData item)
    {
        iconImage.sprite = item.itemIcon; 
        itemNameText.text = item.itemName; 
        itemDescriptionText.text = item.itemDesc; 
        panel.SetActive(true); 
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}
