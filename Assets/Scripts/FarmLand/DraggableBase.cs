using UnityEngine;

public class DraggableBase : MonoBehaviour
{
    private Vector2 touchPos;
    private Camera mainCamera;
    private Vector3 iniPosition;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void OnMouseDown()
    {
        iniPosition = transform.position;
    }

    private void OnMouseUp()
    {
        transform.position = iniPosition;
    }

    private void OnMouseDrag()
    {
        if (Application.isEditor)
        {
            touchPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        }
        else
        {
            touchPos = mainCamera.ScreenToWorldPoint(Input.touches[0].position);
        }

        transform.position = new Vector3(touchPos.x, touchPos.y, 0);
    }
}
