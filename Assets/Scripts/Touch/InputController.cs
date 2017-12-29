using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SwipeManager))]
public class InputController : MonoBehaviour
{
    [SerializeField]
    private bool enableSwipe;
    [SerializeField]
    private bool enableDrag;

    [SerializeField]
    private float gapBetweenScreens;

    [SerializeField]
    private int numberOfScreens;

    [Range(0.001f, 0.05f), SerializeField]
    private float dragSpeed;

    //To avoid minor drags and check click
    [Range(1f, 10f), SerializeField]
    private float minDragDelta;

    [SerializeField]
    private Ease swipeEase = Ease.OutQuad;

    [SerializeField]
    private float easeDuration;

    private int currPos;
    private float[] cameraPositions;
    private float touchDeltaPosition;
    private float dragOffset;

    private bool isTouchingUI;
    private bool isDragging;
    private bool isAlreadySnapped;

    private void Start()
    {
        CreateCameraPositions();
        SwipeManager swipeManager = GetComponent<SwipeManager>();
        swipeManager.onSwipe += SwipeEvent;
        dragOffset = (cameraPositions[1] - cameraPositions[0]) / 3;
    }

    private void Update()
    {
        //if (GEM.isObjectDragging)
        //{
        //    enableSwipe = false;
        //} else
        //{
        if (enableDrag)
        {
            DragCamera();
        }
        //enableSwipe = true;
        // }
    }

    private void CreateCameraPositions()
    {
        cameraPositions = new float[numberOfScreens];
        for (int i = 0; i < numberOfScreens; i++)
        {
            cameraPositions[i] = gapBetweenScreens * i;
        }
        currPos = numberOfScreens / 2;
        transform.position = new Vector3(cameraPositions[currPos], transform.position.y, transform.position.z);
    }

    private void DetectColliderTouch()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
        if (hit.collider != null)
        {
            GameObject hitObject = hit.collider.gameObject;
            switch (hitObject.tag)
            {
                case "Toucher":
                    hitObject.GetComponent<Toucher>().TouchUp();
                    break;
                default:
                    break;
            }
        }
    }

    private void DragCamera()
    {
        var touch = InputHelper.GetTouches();
        if (touch.Count > 0)
        {
            Touch t = touch[0];
            if (t.phase == TouchPhase.Began)
            {
                isAlreadySnapped = false;
                if (EventSystem.current.IsPointerOverGameObject(t.fingerId))
                {
                    isTouchingUI = true;
                    return;
                }
            }
            if (t.phase == TouchPhase.Moved && !isTouchingUI)
            {
                touchDeltaPosition = t.deltaPosition.x;
                if (touchDeltaPosition > minDragDelta || touchDeltaPosition < -minDragDelta)
                {
                    isDragging = true;
                    transform.Translate(-touchDeltaPosition * dragSpeed, 0, 0);
                }
            }
            if (t.phase == TouchPhase.Ended)
            {
                if (!isTouchingUI)
                {
                    SnapCamera();
                    if (!isDragging)
                    {
                        DetectColliderTouch();
                    }
                }
                isTouchingUI = false;
                isDragging = false;
            }
        }
    }

    private void SnapCamera()
    {
        float posX = transform.position.x;

        if (posX > cameraPositions[currPos] + dragOffset)
        {
            SnapRight();
        } else if (posX < cameraPositions[currPos] - dragOffset)
        {
            SnapLeft();
        } else
        {
            SnapCenter();
        }
        if (posX > cameraPositions[cameraPositions.Length - 1] || posX < cameraPositions[0])
        {
            SnapCenter();
        }
    }

    private void SnapRight()
    {
        if (currPos < cameraPositions.Length - 1)
        {
            isAlreadySnapped = true;
            transform.DOMoveX(cameraPositions[currPos + 1], easeDuration).SetEase(swipeEase);
            currPos++;
        }
    }

    private void SnapLeft()
    {
        if (currPos > 0)
        {
            isAlreadySnapped = true;
            transform.DOMoveX(cameraPositions[currPos - 1], easeDuration).SetEase(swipeEase);
            currPos--;
        }
    }

    private void SnapCenter()
    {
        transform.DOMoveX(cameraPositions[currPos], easeDuration).SetEase(swipeEase);
    }

    public void SnapCameraOnButton(int pos)
    {
        currPos = pos;
        transform.DOMoveX(cameraPositions[currPos], easeDuration).SetEase(swipeEase);
    }

    #region Swipe
    private void SwipeCamera(int pos)
    {
        int temp = currPos;
        temp += pos;
        if (temp < 0 || temp >= cameraPositions.Length || isAlreadySnapped)
        {
            return;
        }
        currPos += pos;
        transform.DOMoveX(cameraPositions[currPos], 0.5f).SetEase(swipeEase);
    }

    private void SwipeEvent(SwipeAction swipeAction)
    {
        if (!enableSwipe)
        {
            return;
        }
        if (swipeAction.direction == SwipeDirection.Right)
        {
            SwipeCamera(-1);
        } else if (swipeAction.direction == SwipeDirection.Left)
        {
            SwipeCamera(1);
        }
    }
    #endregion
}