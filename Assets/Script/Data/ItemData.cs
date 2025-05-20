using UnityEngine;
public enum ItemType
{
    HealthPotion,
    EnergyPotion,
}

[CreateAssetMenu(fileName = "NewItemData", menuName = "Item Data/Item")]

public class ItemData : ScriptableObject
{
    public string itemName;
    public string itemDesc;
    public Sprite itemIcon;
    public int itemPrice;
    public ItemType itemType;
    public int amountRestored;
    public int timeToUse;
    public bool canStack;
}
