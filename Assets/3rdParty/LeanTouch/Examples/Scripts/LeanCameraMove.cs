using System.Collections.Generic;
using UnityEngine;

namespace Lean.Touch
{
    // This script allows you to track & pedestral this GameObject (e.g. Camera) based on finger drags
    public class LeanCameraMove : MonoBehaviour
    {
        private Camera mainCamera;
        [SerializeField]
        public float screenWidth = 60;
        [SerializeField]
        public float screenHight = 60;

        [Tooltip("Ignore fingers with StartedOverGui?")]
        public bool IgnoreGuiFingers = true;

        [Tooltip("Ignore fingers if the finger count doesn't match? (0 = any)")]
        public int RequiredFingerCount;

        [Tooltip("The distance from the camera the world drag delta will be calculated from (this only matters for perspective cameras)")]
        public float Distance = 1.0f;

        [Tooltip("The sensitivity of the movement, use -1 to invert")]
        public float Sensitivity = 1.0f;

        public float minX, maxX, minY, maxY;

        private LeanCameraZoomSmooth leanZoom;
        private float deltaZoom;
        private float tempDeltaZoom;
        private List<LeanFinger> leanFinger;
        private Vector3 worldDelta;
        private Vector3 tempWorldDelta;

        private void Start()
        {
            leanZoom = GetComponent<LeanCameraZoomSmooth>();
            mainCamera = LeanTouch.GetCamera(mainCamera, gameObject);
            tempWorldDelta = worldDelta;
        }

        protected virtual void LateUpdate()
        {
            if (mainCamera != null)
            {
                ChangeBounds();
                // Get the fingers we want to use
                leanFinger = LeanTouch.GetFingers(IgnoreGuiFingers, RequiredFingerCount);

                // Get the world delta of all the fingers
                worldDelta = LeanGesture.GetWorldDelta(leanFinger, Distance, mainCamera);

                //Uncomment tis to upade move only when it is moved
                //if (tempWorldDelta == worldDelta)
                //{
                //    return;
                //}
                //tempWorldDelta = worldDelta;

                ClampCamera(worldDelta * Sensitivity);
            }
        }

        private void ClampCamera(Vector3 position)
        {
            // Pan the camera based on the world delta
            transform.position -= position;

            transform.position = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX),
                                    Mathf.Clamp(transform.position.y, minY, maxY),
                                    transform.position.z);
        }

        private void ChangeBounds()
        {
            deltaZoom = leanZoom.Zoom - leanZoom.ZoomMin;

            if (tempDeltaZoom == deltaZoom)
            {
                return;
            }
            tempDeltaZoom = deltaZoom;

            float horzExtent = leanZoom.Zoom * Screen.width / Screen.height;
            minX = (float)(horzExtent);
            maxX = (float)(screenWidth - horzExtent);
            minY = (float)(leanZoom.Zoom);
            maxY = (float)(screenHight - leanZoom.Zoom);
        }
    }
}