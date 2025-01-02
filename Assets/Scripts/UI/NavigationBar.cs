using UnityEngine;
using UnityEngine.UI;

public class NavigationBar : MonoBehaviour
{
    public GameObject[] menus;

    [Header("Navigation Buttons")]
    public Button[] navigationButtons;

    [Header("Button Active/Inactive Colors")]
    public Color activeColor = Color.green;
    public Color inactiveColor = Color.gray;

    private Button currentActiveButton;

    private void Start()
    {
        // Assign click listeners to buttons
        for (int i = 0; i < navigationButtons.Length; i++)
        {
            int menuID = i;
            navigationButtons[i].onClick.AddListener(() => NavigateTo(menuID, navigationButtons[menuID]));
        }

        // Set the default active button (e.g., Dashboard)
        NavigateTo(0, navigationButtons[0]);
    }

    private void NavigateTo(int menuID, Button clickedButton)
    {
        // Hide all menus
        for (int i = 0; i < menus.Length; i++)
        {
            menus[i].SetActive(i == menuID);
        }

        // Update button visuals
        if (currentActiveButton != null)
            SetButtonColor(currentActiveButton, inactiveColor);

        SetButtonColor(clickedButton, activeColor);
        currentActiveButton = clickedButton;
    }

    private void SetButtonColor(Button button, Color color)
    {
        var colors = button.colors;
        colors.normalColor = color;
        button.colors = colors;
    }
}
