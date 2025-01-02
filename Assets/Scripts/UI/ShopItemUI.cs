using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    public TMP_Text itemNameText;
    public TMP_Text costText;
    public Image iconImage;
    public Button buyButton;

    private ShopItemData itemData;
    private System.Action updateCurrencyDisplayCallback;

    public void Initialize(ShopItemData data, System.Action updateCallback)
    {
        itemData = data; 
        updateCurrencyDisplayCallback = updateCallback;

        itemNameText.text = data.itemName;
        costText.text = $"Cost: {data.cost}";
        iconImage.sprite = data.icon;

        buyButton.onClick.AddListener(() => PurchaseItem());
    }

    private void PurchaseItem()
    {
        if (CurrencyManager.Instance.SpendCurrency(itemData.cost))
        {
            Debug.Log($"Purchased: {itemData.itemName}");
            InventoryManager.Instance.AddItem(itemData);
            updateCurrencyDisplayCallback.Invoke();
        }
        else
        {
            Debug.Log("Not enough currency!");
        }
    }
}
