using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    public float swipeSpeed = 0.01f;
    public float minX = -10f;
    public float maxX = 10f;

    private bool isSwiping = false; // Flag to indicate if the swipe is active
    private Vector2 lastMousePosition;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Detect mouse button down
        {
            if (IsPointerOverUIElementWithTag("CameraSwipe"))
            {
                isSwiping = true;
                lastMousePosition = Input.mousePosition;
            }
        }

        if (Input.GetMouseButton(0) && isSwiping) // Detect mouse drag
        {
            Vector2 currentMousePosition = Input.mousePosition;
            Vector2 delta = currentMousePosition - lastMousePosition;

            float deltaX = -delta.x * swipeSpeed;
            Vector3 newPosition = transform.position + new Vector3(deltaX, 0, 0);
            transform.position = new Vector3(
                Mathf.Clamp(newPosition.x, minX, maxX),
                transform.position.y,
                transform.position.z
            );

            lastMousePosition = currentMousePosition; // Update last mouse position
        }

        if (Input.GetMouseButtonUp(0)) // Detect mouse button release
        {
            isSwiping = false;
        }
    }

    private bool IsPointerOverUIElementWithTag(string tag)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var result in results)
        {
            if (result.gameObject.CompareTag(tag))
            {
                return true;
            }
        }

        return false;
    }
}
