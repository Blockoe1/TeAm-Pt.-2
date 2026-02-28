/*************************************************
Brandon Koederitz
1/31/2025
2/5/2025
Takes screenshots of the current camera view with transparency and 
NaughtyAttributes, FishNet
***************************************************/
using UnityEngine;

namespace GraffitiGala
{
    [RequireComponent(typeof(Camera))]
    public class CameraScreenshot : MonoBehaviour
    {
        [SerializeField] private RenderTexture targetTexture;

        private Camera captureCamera;

        /// <summary>
        /// When this script awakes, get a reference to the camera object attached to this GameObject.
        /// </summary>
        private void Awake()
        {
            captureCamera = GetComponent<Camera>();
        }

        /// <summary>
        /// Has the current GraffitiSaver take a screenshot of the current graffiti on the screen within given 
        /// dimensions and save it to StreamingAssets with a given file name.
        /// </summary>
        /// <remarks>
        /// Credit to Erik_Harg on the Unity Forums for this solution.
        /// </remarks>
        /// <param name="width"> The width in pixels of the screenshot to capture.</param>
        /// <param name="height">The height in pixels of the screenshot to capture.</param>
        public void ScreenshotDrawing()
        { 
            Camera cam = captureCamera != null ? captureCamera : Camera.main;

            // Renders camera data to the screenshot render texture.
            RenderTexture camRenderTexture = cam.targetTexture;
            cam.targetTexture = targetTexture;
            cam.Render();
            cam.targetTexture = camRenderTexture;            
        }
    }

}