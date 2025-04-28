using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "Item Data/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public int itemPrice;
}
