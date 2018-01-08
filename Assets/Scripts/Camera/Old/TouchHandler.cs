using System;
using UnityEngine;
using UnityEngine.UI;

namespace HarvestValley.TouchClickInput
{
    public class TouchHandler : MonoBehaviour
    {
        public Text dText;

        private void Start()
        {
            TouchManager.OnTouchUpDetected += OnTouchUp;
            TouchManager.OnTouchDownDetected += OnTouchDown;
        }

        private void OnTouchDown(Vector3 position)
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(position), Vector3.zero);
            if (hitInfo)
            {
                GameObject hitTransform = hitInfo.transform.gameObject;
                // CheckObject(hitTransform);
            }
        }

        private void OnTouchUp(Vector3 position)
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(position), Vector3.zero);
            if (hitInfo)
            {
                GameObject hitTransform = hitInfo.transform.gameObject;
                CheckObject(hitTransform);
            }
        }

        private void CheckObject(GameObject obj)
        {
            switch (obj.tag)
            {
                case "Field":
                    //obj.GetComponent<DraggableBuildings>().TouchedUp();
                    break;
                case "Items":
                    break;
                case "Buildings":
                    break;
                case "Grass":
                    break;
                default:
                    break;
            }
        }
    }
}