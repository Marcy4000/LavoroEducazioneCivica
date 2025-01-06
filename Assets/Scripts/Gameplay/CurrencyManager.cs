using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

    [SerializeField] private Sprite currencySprite;
    public Sprite CurrencySprite => currencySprite;

    private int shopTickets = 20;

    public int ShopTickets => shopTickets;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddCurrency(int amount)
    {
        shopTickets += amount;
        Debug.Log($"Currency added: {amount}. Current balance: {shopTickets}");
    }

    public bool SpendCurrency(int amount)
    {
        if (shopTickets >= amount)
        {
            shopTickets -= amount;
            Debug.Log($"Currency spent: {amount}. Remaining balance: {shopTickets}");
            return true;
        }
        else
        {
            Debug.Log("Not enough currency!");
            return false;
        }
    }
}
