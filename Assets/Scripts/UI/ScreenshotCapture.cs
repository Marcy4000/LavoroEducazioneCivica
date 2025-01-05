using UnityEngine;
using System.IO;

public class WorldScreenshotCapture : MonoBehaviour
{
    [SerializeField] private float screenshotWidth = 10f; // Width in world units
    [SerializeField] private float screenshotHeight = 10f; // Height in world units
    [SerializeField] private Vector3 screenshotCenter = Vector3.zero; // Center of the screenshot in world space

    public void CaptureScreenshot()
    {
        // Create a temporary camera
        Camera screenshotCamera = new GameObject("ScreenshotCamera").AddComponent<Camera>();
        screenshotCamera.transform.position = screenshotCenter;
        screenshotCamera.transform.rotation = Quaternion.identity;

        // Set the camera's orthographic size and aspect ratio
        screenshotCamera.orthographic = true;
        screenshotCamera.orthographicSize = screenshotHeight / 2f; // Half of the world height
        float aspectRatio = screenshotWidth / screenshotHeight;

        // Determine render texture size
        int textureWidth = Mathf.CeilToInt(1024 * aspectRatio);
        int textureHeight = 1024;

        // Setup render texture and capture
        RenderTexture renderTexture = new RenderTexture(textureWidth, textureHeight, 24);
        screenshotCamera.targetTexture = renderTexture;

        Texture2D screenshot = new Texture2D(textureWidth, textureHeight, TextureFormat.RGB24, false);
        screenshotCamera.Render();
        RenderTexture.active = renderTexture;
        screenshot.ReadPixels(new Rect(0, 0, textureWidth, textureHeight), 0, 0);
        screenshot.Apply();

        // Clean up
        RenderTexture.active = null;
        screenshotCamera.targetTexture = null;
        Destroy(renderTexture);
        Destroy(screenshotCamera.gameObject);

        // Encode to PNG
        byte[] bytes = screenshot.EncodeToPNG();

        // Save or download the screenshot
#if UNITY_WEBGL
        string fileName = "Screenshot_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
        DownloadImageWebGL(bytes, fileName);
#else
        string path = PromptForSaveLocation();
        if (!string.IsNullOrEmpty(path))
        {
            File.WriteAllBytes(path, bytes);
            Debug.Log("Screenshot saved to " + path);
        }
#endif
    }

#if UNITY_WEBGL
    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void DownloadImageWebGL(byte[] data, string fileName);

    private void DownloadImageWebGL(byte[] data, string fileName)
    {
        string base64Data = System.Convert.ToBase64String(data);
        string downloadScript = $"(function(){{ var link = document.createElement('a'); link.href = 'data:image/png;base64,{base64Data}'; link.download = '{fileName}'; document.body.appendChild(link); link.click(); document.body.removeChild(link); }})();";
        Application.ExternalEval(downloadScript);
    }
#endif

#if UNITY_EDITOR || UNITY_STANDALONE
    private string PromptForSaveLocation()
    {
#if UNITY_EDITOR
        return UnityEditor.EditorUtility.SaveFilePanel("Save Screenshot", "", "Screenshot.png", "png");
#else
        // Implement a file save dialog for standalone builds (platform-specific)
        Debug.LogWarning("Save file dialog not implemented for standalone builds.");
        return "";
#endif
    }
#endif
}
