using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerPlanting : MonoBehaviour
{
    public Camera mainCamera;
    public LayerMask groundLayer;
    public TreeManager treeManager;

    public GameObject seedPrefab;

    public ShopItemData treeSeed;

    private GameObject draggedSeed;
    private bool isDragging = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button pressed
        {
            OnDragStart();
        }
        else if (Input.GetMouseButton(0) && isDragging) // While dragging
        {
            OnDragMove();
        }
        else if (Input.GetMouseButtonUp(0)) // Left mouse button released
        {
            OnDragEnd();
        }
    }

    private void OnDragStart()
    {
        if (IsPointerOverUIElement(out GameObject uiElement))
        {
            if (uiElement.CompareTag("Seed"))
            {
                draggedSeed = Instantiate(seedPrefab); // Clone the seed
                draggedSeed.transform.SetParent(null); // Detach from UI
                draggedSeed.transform.localScale = Vector3.one; // Adjust scale if needed
                isDragging = true;
            }
        }
    }

    private void OnDragMove()
    {
        if (draggedSeed != null)
        {
            Vector3 screenPosition = Input.mousePosition;
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, 10f));
            draggedSeed.transform.position = new Vector3(worldPosition.x, worldPosition.y, 0f);
        }
    }

    private void OnDragEnd()
    {
        if (isDragging && draggedSeed != null)
        {
            Vector3 screenPosition = Input.mousePosition;
            Ray ray = mainCamera.ScreenPointToRay(screenPosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
            {
                Vector3 plantPosition = hit.point;
                plantPosition.y = 0f; // Ensure the tree is planted at a fixed Y position
                treeManager.PlantTree(plantPosition, treeSeed);
                Destroy(draggedSeed);
            }
            else
            {
                Destroy(draggedSeed); // Destroy the dragged seed if not planted
            }

            draggedSeed = null;
            isDragging = false;
        }
    }

    private bool IsPointerOverUIElement(out GameObject uiElement)
    {
        var pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        if (results.Count > 0)
        {
            uiElement = results[0].gameObject;
            return true;
        }

        uiElement = null;
        return false;
    }
}
