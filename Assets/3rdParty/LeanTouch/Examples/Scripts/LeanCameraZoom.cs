using UnityEngine;

namespace Lean.Touch
{
    // This script allows you to zoom a camera in and out based on the pinch gesture
    // This supports both perspective and orthographic cameras
    [ExecuteInEditMode]
    public class LeanCameraZoom : MonoBehaviour
    {
        private Camera mainCamera;

        [Tooltip("Ignore fingers with StartedOverGui?")]
        public bool IgnoreGuiFingers = true;

        [Tooltip("Allows you to force rotation with a specific amount of fingers (0 = any)")]
        public int RequiredFingerCount;

        [Tooltip("If you want the mouse wheel to simulate pinching then set the strength of it here")]
        [Range(-1.0f, 1.0f)]
        public float WheelSensitivity;

        [Tooltip("The current FOV/Size")]
        public float Zoom = 50.0f;
        private float tempZoom;

        [Tooltip("Limit the FOV/Size?")]
        public bool ZoomClamp;

        [Tooltip("The minimum FOV/Size we want to zoom to")]
        public float ZoomMin = 10.0f;

        [Tooltip("The maximum FOV/Size we want to zoom to")]
        public float ZoomMax = 60.0f;

        private void Start()
        {
            mainCamera = LeanTouch.GetCamera(mainCamera, gameObject);
            tempZoom = Zoom;
        }

        protected virtual void LateUpdate()
        {
            // Get the fingers we want to use
            var fingers = LeanTouch.GetFingers(IgnoreGuiFingers, RequiredFingerCount);

            // Get the pinch ratio of these fingers
            var pinchRatio = LeanGesture.GetPinchRatio(fingers, WheelSensitivity);

            // Modify the zoom value
            Zoom *= pinchRatio;

            if (tempZoom == Zoom)
            {
                return;
            }
            tempZoom = Zoom;
            SetZoom(Zoom);

            if (ZoomClamp == true)
            {
                Zoom = Mathf.Clamp(Zoom, ZoomMin, ZoomMax);
            }


        }

        protected void SetZoom(float current)
        {
            // Make sure the camera exists
            // var camera = LeanTouch.GetCamera(Camera, gameObject);

            if (mainCamera != null)
            {
                if (mainCamera.orthographic == true)
                {
                    mainCamera.orthographicSize = current;
                }
                else
                {
                    mainCamera.fieldOfView = current;
                }
            }
        }
    }
}