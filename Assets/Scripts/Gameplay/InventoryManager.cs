using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    private List<InventoryItem> inventory = new List<InventoryItem>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist inventory across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(ShopItemData itemData, int quantity = 1)
    {
        var existingItem = inventory.Find(i => i.itemData == itemData);
        if (existingItem != null)
        {
            existingItem.quantity += quantity;
        }
        else
        {
            inventory.Add(new InventoryItem(itemData, quantity));
        }

        Debug.Log($"Added {quantity}x {itemData.itemName} to inventory.");
    }

    public bool RemoveItem(ShopItemData itemData, int quantity = 1)
    {
        var existingItem = inventory.Find(i => i.itemData == itemData);
        if (existingItem != null && existingItem.quantity >= quantity)
        {
            existingItem.quantity -= quantity;

            if (existingItem.quantity == 0)
                inventory.Remove(existingItem);

            Debug.Log($"Removed {quantity}x {itemData.itemName} from inventory.");
            return true;
        }
        else
        {
            Debug.Log("Not enough items to remove!");
            return false;
        }
    }

    public List<InventoryItem> GetInventory()
    {
        return new List<InventoryItem>(inventory); // Return a copy to avoid external modifications
    }
}

[System.Serializable]
public class InventoryItem
{
    public ShopItemData itemData;
    public int quantity;

    public InventoryItem(ShopItemData data, int qty)
    {
        itemData = data;
        quantity = qty;
    }
}
