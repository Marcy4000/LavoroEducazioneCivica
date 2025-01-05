using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text currencyText;
    public Transform shopItemsParent;

    [Header("Prefabs")]
    public GameObject shopItemPrefab;

    [Header("Shop Items")]
    public ShopItemData[] shopItems; // Array of items available in the shop

    private void Start()
    {
        UpdateCurrencyDisplay();
        PopulateShop();
    }

    private void Update()
    {
        UpdateCurrencyDisplay();
    }

    private void UpdateCurrencyDisplay()
    {
        currencyText.text = $"{CurrencyManager.Instance.ShopTickets}<size=70%>x";
    }

    private void PopulateShop()
    {
        foreach (var item in shopItems)
        {
            var shopItem = Instantiate(shopItemPrefab, shopItemsParent);
            var shopItemUI = shopItem.GetComponent<ShopItemUI>();
            shopItemUI.Initialize(item, UpdateCurrencyDisplay);
        }
    }
}

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

public enum ShopItemType
{
    TreeSeed,
    Decoration,
    Other
}