using UnityEngine;

namespace Lean.Touch
{
    // This modifies LeanCameraZoom to be smooth
    public class LeanCameraZoomSmooth : LeanCameraZoom
    {
        [Tooltip("How quickly the zoom reaches the target value")]
        public float Dampening = 10.0f;

        private float currentZoom;

        [SerializeField]
        private bool cameraClamp = false;
        [SerializeField]
        private float minX, maxX = 0;
        [SerializeField]
        private float minY, maxY = 0;
        private float zoomCameraClamp;

        protected virtual void OnEnable()
        {
            currentZoom = Zoom;
        }

        protected override void LateUpdate()
        {
            // Use the LateUpdate code from LeanCameraZoom
            base.LateUpdate();

            // Get t value
            var factor = LeanTouch.GetDampenFactor(Dampening, Time.deltaTime);

            // Lerp the current value to the target one
            currentZoom = Mathf.Lerp(currentZoom, Zoom, factor);
            // Set the new zoom
            SetZoom(currentZoom);
            if (cameraClamp)
            {
                zoomCameraClamp = (Zoom - ZoomMin) / 2;
                transform.position = new Vector3(
                    Mathf.Clamp(transform.position.x, minX, maxX - zoomCameraClamp),
                    Mathf.Clamp(transform.position.y, minY, maxY - zoomCameraClamp),
                    transform.position.z);
            }
        }
    }
}