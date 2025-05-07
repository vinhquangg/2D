using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "Item Data/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public string itemDesc;
    public Sprite itemIcon;
    public int itemPrice;
}
