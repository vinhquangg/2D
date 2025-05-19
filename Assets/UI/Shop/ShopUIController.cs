using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIController : MonoBehaviour
{
    public GameObject shopPanel;
    public GameObject slotPrefab;
    public Transform gridContainer;
    private int maxSlots = 10;
    [SerializeField] private ItemDetailUI itemDetailUI;
    public static ShopUIController instance { get; private set; }
    [Tooltip("List of item will apear in shop")]
    public List<ItemData> shopItems;
    private PlayerSoul playerSoul;
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
        itemDetailUI.Hide();
        InitializedSlot();
        playerSoul = FindObjectOfType<PlayerSoul>();
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
            slot.transform.Find("ItemPrice").GetComponent<TextMeshProUGUI>().text = item.itemPrice.ToString() + " Soul";

            Button button = slot.GetComponent<Button>();
            if (button != null)
            {
                ItemData currentItem = item;
                button.onClick.AddListener(() => itemDetailUI.ShowItem(currentItem));
            }
        }
    }


    public void OpenShopUI()
    {
        GameManager.instance.OpenShopUI();
        PlayerInputHandler.instance?.DisablePlayerInput();
        Time.timeScale= 0f;
    }

    public void CloseShopUI()
    {
        GameManager.instance.CloseShopUI();
        PlayerInputHandler.instance?.EnablePlayerInput();
        Time.timeScale = 1;
    }

    public void BuySelectedItem(ItemData item)
    {
        if (playerSoul == null)
        {
            Debug.LogError("PlayerSoul không tìm thấy!");
            return;
        }

        bool success = playerSoul.SpendSoul(item.itemPrice);
        if (success)
        {
            Debug.Log($"Mua thành công item {item.itemName} với giá {item.itemPrice} Soul");
            itemDetailUI.Hide();
            InitializedSlot(); 
        }
        else
        {
            Debug.Log("Không đủ Soul để mua item này!");
        }
    }

}
