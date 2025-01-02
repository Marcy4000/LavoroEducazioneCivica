using UnityEngine;

public class ParallaxScrolling : MonoBehaviour
{
    public Transform cameraTransform; // Reference to the camera
    public float parallaxFactor = 0.5f; // Determines the strength of the parallax effect
    private Vector3 lastCameraPosition;

    private void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform; // Default to the main camera
        }
        lastCameraPosition = cameraTransform.position;
    }

    private void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * parallaxFactor, 0, 0);
        lastCameraPosition = cameraTransform.position;
    }
}