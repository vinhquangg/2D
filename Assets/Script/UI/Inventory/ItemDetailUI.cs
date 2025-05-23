using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetailUI : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;
    public GameObject panel;
    public Button buyButton;
    private ItemData currentItem;

    private void Start()
    {
        buyButton.onClick.AddListener(OnBuyButtonClicked);
    }

    public void ShowItem(ItemData item)
    {
        currentItem = item; 
        iconImage.sprite = item.itemIcon; 
        itemNameText.text = item.itemName; 
        itemDescriptionText.text = item.itemDesc; 
        panel.SetActive(true); 
    }

    public void Hide()
    {
        panel.SetActive(false);
    }

    private void OnBuyButtonClicked()
    {
        if (currentItem != null)
        {
            ShopUIController.instance.BuySelectedItem(currentItem);
        }
    }
}
