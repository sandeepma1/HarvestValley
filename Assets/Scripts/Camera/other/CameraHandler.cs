using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField]
    private float dragSpeed = 2;
    private Vector3 dragOrigin;
    private Vector3 position;
    private Vector3 movePosition = Vector3.zero;
    private Vector3 tempMovePosition = Vector3.zero;

    private enum SnapStates { left, right, center };
    private int currentCameraPostion = 3;
    private int snapThreshold = 6;
    //Watch out for this is swipe/drag has issue as OnSwipeDetected is executed first
    private bool isSwipeDetected;

    Hashtable ease = new Hashtable();

    private void Start()
    {
        SwipeManager.OnSwipeDetected += OnSwipeDetected;
        ease.Add("ease", LeanTweenType.easeOutSine);
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            tempMovePosition = Vector3.zero;
            if (isSwipeDetected == false)
            {
                SnapStates snapState = new SnapStates();
                snapState = DetectDragMoveDirection();
                SnapCamera(snapState);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            isSwipeDetected = false;
            dragOrigin = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(0)) return;
        DetectSwipe();
    }

    private void DetectSwipe()
    {
        position = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        //use movePosition.y to control Y
        movePosition = new Vector3(-position.x * dragSpeed, 0, 0);
        DragCamera(movePosition - tempMovePosition);
        tempMovePosition = movePosition;
    }

    private void DragCamera(Vector3 position)
    {
        transform.position += position;
    }

    private void SnapCamera(SnapStates snapState)
    {
        if (currentCameraPostion == 1 && snapState == SnapStates.left)
        {
            snapState = SnapStates.center;
        }

        if (currentCameraPostion == 5 && snapState == SnapStates.right)
        {
            snapState = SnapStates.center;
        }

        switch (snapState)
        {
            case SnapStates.left:
                currentCameraPostion--;
                break;
            case SnapStates.right:
                currentCameraPostion++;
                break;
            case SnapStates.center:
                break;
            default:
                break;
        }

        LeanTween.moveX(Camera.main.gameObject, GameEventManager.screensPositions[currentCameraPostion], 0.5f, ease);
    }

    private SnapStates DetectDragMoveDirection()
    {
        //move right
        if (transform.position.x > GameEventManager.screensPositions[currentCameraPostion] + snapThreshold)
        {
            return SnapStates.right;
        }

        //Move Left
        if (transform.position.x < GameEventManager.screensPositions[currentCameraPostion] - snapThreshold)
        {
            return SnapStates.left;
        }

        return SnapStates.center;
    }

    void OnSwipeDetected(Swipe direction, Vector2 swipeVelocity)
    {
        print(swipeVelocity);
        if (swipeVelocity.x >= 30 || swipeVelocity.x <= -30)
        {
            return;
        }
        isSwipeDetected = true;

        switch (direction)
        {
            case Swipe.None:
                break;
            case Swipe.Up:
                break;
            case Swipe.Down:
                break;
            case Swipe.Left:
                SnapCamera(SnapStates.right);
                print("Swiped left");
                break;
            case Swipe.Right:
                SnapCamera(SnapStates.left);
                print("Swiped left");
                break;
            case Swipe.UpLeft:
                break;
            case Swipe.UpRight:
                break;
            case Swipe.DownLeft:
                break;
            case Swipe.DownRight:
                break;
            default:
                break;
        }
    }
}