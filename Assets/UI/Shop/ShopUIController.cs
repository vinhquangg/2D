using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopUIController : MonoBehaviour
{
    public GameObject shopPanel;
    public GameObject slotPrefab;
    public Transform gridContainer;
    private int maxSlots = 10;
    public static ShopUIController instance { get; private set; }
    //[System.Serializable]
    //public class ShopItem
    //{
    //    public Sprite itemIcon;
    //    public int itemPrice;
    //}
    [Tooltip("List of item will apear in shop")]
    public List<ItemData> shopItems;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        InitializedSlot();
    }
    public void InitializedSlot()
    {
        foreach (Transform child in gridContainer)
        {
            Destroy(child.gameObject);
        }

        int slotCount = Mathf.Min(maxSlots, shopItems.Count);

        for (int i = 0; i < slotCount; i++)
        {
            var item = shopItems[i];
            GameObject slot = Instantiate(slotPrefab, gridContainer);

            slot.transform.Find("ItemIcon").GetComponent<UnityEngine.UI.Image>().sprite = item.itemIcon;
            slot.transform.Find("ItemPrice").GetComponent<TextMeshProUGUI>().text = item.itemPrice.ToString() + " Coins";
        }   
    }

    public void OpenShopUI()
    {
        GameManager.instance.OpenShopUI();
    }

    public void CloseShopUI()
    {
        GameManager.instance.CloseShopUI();
        Time.timeScale = 1;
    }
}
