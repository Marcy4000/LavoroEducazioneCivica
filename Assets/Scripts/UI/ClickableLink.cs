using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ClickableLink : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private TextMeshProUGUI textComponent;
    private Color originalColor;
    [SerializeField] private Color hoverColor = Color.blue;

    public event System.Action OnClick;

    void Start()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        originalColor = textComponent.color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        textComponent.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        textComponent.color = originalColor;
    }
}