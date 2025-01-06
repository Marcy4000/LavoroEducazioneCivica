using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    public TMP_Text itemNameText;
    public TMP_Text costText;
    public Image iconImage;
    public Button buyButton;
    public GameObject insufficientFundsMessage; // Add this UI element

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
        
        if (insufficientFundsMessage != null)
            insufficientFundsMessage.SetActive(false);
    }

    private void PurchaseItem()
    {
        if (CurrencyManager.Instance.SpendCurrency(itemData.cost))
        {
            Debug.Log($"Purchased: {itemData.itemName}");
            InventoryManager.Instance.AddItem(itemData);
            updateCurrencyDisplayCallback.Invoke();
            
            if (insufficientFundsMessage != null)
                insufficientFundsMessage.SetActive(false);
        }
        else
        {
            Debug.Log("Not enough currency!");
            if (insufficientFundsMessage != null)
            {
                insufficientFundsMessage.SetActive(true);
                Invoke("HideInsufficientMessage", 2f); // Hide message after 2 seconds
            }
        }
    }

    private void HideInsufficientMessage()
    {
        if (insufficientFundsMessage != null)
            insufficientFundsMessage.SetActive(false);
    }
}
