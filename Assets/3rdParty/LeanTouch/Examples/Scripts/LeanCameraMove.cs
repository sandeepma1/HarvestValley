using UnityEngine;

namespace Lean.Touch
{
    // This script allows you to track & pedestral this GameObject (e.g. Camera) based on finger drags
    public class LeanCameraMove : MonoBehaviour
    {
        private Camera mainCamera;

        [Tooltip("Ignore fingers with StartedOverGui?")]
        public bool IgnoreGuiFingers = true;

        [Tooltip("Ignore fingers if the finger count doesn't match? (0 = any)")]
        public int RequiredFingerCount;

        [Tooltip("The distance from the camera the world drag delta will be calculated from (this only matters for perspective cameras)")]
        public float Distance = 1.0f;

        [Tooltip("The sensitivity of the movement, use -1 to invert")]
        public float Sensitivity = 1.0f;

        public float minX, maxX, minY, maxY;

        private void Start()
        {
            mainCamera = Camera.main;
        }

        protected virtual void LateUpdate()
        {
            // Make sure the camera exists
            //var camera = LeanTouch.GetCamera(mainCamera, gameObject);

            if (mainCamera != null)
            {
                // Get the fingers we want to use
                var fingers = LeanTouch.GetFingers(IgnoreGuiFingers, RequiredFingerCount);

                // Get the world delta of all the fingers
                var worldDelta = LeanGesture.GetWorldDelta(fingers, Distance, mainCamera);

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
    }
}