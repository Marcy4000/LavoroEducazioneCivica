using UnityEngine;

[CreateAssetMenu(fileName = "NewShopItem", menuName = "Shop/Shop Item")]
public class ShopItemData : ScriptableObject
{
    public string itemName;
    public int cost;
    public ShopItemType type;
    public int treeTypeId;
    public Sprite icon;
    public string description;
}