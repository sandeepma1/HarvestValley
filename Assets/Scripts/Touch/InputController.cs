using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using System.Collections;
using HarvestValley.Ui;

[RequireComponent(typeof(SwipeManager))]
public class InputController : Singleton<InputController>
{
    [SerializeField]
    private bool enableSwipe = true;
    [SerializeField]
    private bool enableDrag = true;

    [SerializeField]
    private int gapBetweenScreens = 16;

    [SerializeField]
    private int numberOfScreens = 5;

    [Range(0.001f, 0.05f), SerializeField]
    private float dragSpeed = 0.01f;

    //To avoid minor drags and check click
    [Range(1f, 10f), SerializeField]
    private float minDragLength = 3;

    [SerializeField]
    private Ease snapEase = Ease.OutQuad;

    [SerializeField]
    private float easeDuration = 0.5f;

    public bool IsDragging { get; private set; }

    private int currPos;
    private float[] cameraPositions;
    private float touchDeltaPosition;
    private float dragOffset;
    private readonly float cameraOrthSizeNormal = 16;
    private readonly float cameraOrthSizeZoom = 12;

    private bool isTouchingUI;

    private bool isAlreadySnapped;
    private bool isCameraZoomed;

    private Vector3 tempCameraPosition;

    private Transform mainCameraTransform;
    private Camera mainCamera;
    Hashtable ease = new Hashtable();

    private void Start()
    {
        CreateCameraPositions();
        SwipeManager swipeManager = GetComponent<SwipeManager>();
        swipeManager.onSwipeEvent += SwipeEventHandler;
        dragOffset = (cameraPositions[1] - cameraPositions[0]) / 3;
        ease.Add("ease", LeanTweenType.easeOutSine);
    }

    private void Update()
    {
        DragCamera();
    }

    public void SetDragSwipe(bool flag)
    {
        enableDrag = enableSwipe = flag;
    }


    private void CreateCameraPositions()
    {
        if (!Camera.main.CompareTag("MainCamera"))
        {
            Debug.LogError("@@-NO camera in scene or MainCamera tag not assigned");
        }
        mainCameraTransform = Camera.main.transform;
        mainCamera = Camera.main;

        mainCameraTransform.position = new Vector3(0, 0, mainCameraTransform.position.z);
        cameraPositions = new float[numberOfScreens];
        for (int i = 0; i < numberOfScreens; i++)
        {
            cameraPositions[i] = gapBetweenScreens * i;
        }
        currPos = numberOfScreens / 2;
        mainCameraTransform.position = new Vector3(cameraPositions[currPos], mainCameraTransform.position.y, mainCameraTransform.position.z);
        tempCameraPosition = mainCameraTransform.position;
    }

    private void DetectColliderTouch() // not using
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
                case "Grass":
                    //hitObject.GetComponent<DraggableGrass>().TouchedUp();
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
            if (enableDrag)
            {
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
                    if (touchDeltaPosition > minDragLength || touchDeltaPosition < -minDragLength)
                    {
                        IsDragging = true;
                        mainCameraTransform.Translate(-touchDeltaPosition * dragSpeed, 0, 0);
                    }
                }
            }

            if (t.phase == TouchPhase.Ended)
            {
                if (!isTouchingUI)
                {
                    SnapCamera();
                    if (!IsDragging)
                    {
                        // print("not touching UI and touch on GO");
                        // DetectColliderTouch();
                    }
                }
                isTouchingUI = false;
                IsDragging = false;
                if (EventSystem.current.IsPointerOverGameObject(t.fingerId))
                {
                    // print("touched");
                }
            }
        }
    }

    private void SnapCamera()
    {
        float posX = mainCameraTransform.position.x;

        if (posX > cameraPositions[currPos] + dragOffset)
        {
            SnapRight();
        }
        else if (posX < cameraPositions[currPos] - dragOffset)
        {
            SnapLeft();
        }
        else
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
            mainCameraTransform.DOMoveX(cameraPositions[currPos + 1], easeDuration).SetEase(snapEase);
            currPos++;
            MenuManager.Instance.CloseAllMenu();
        }
    }

    private void SnapLeft()
    {
        if (currPos > 0)
        {
            isAlreadySnapped = true;
            mainCameraTransform.DOMoveX(cameraPositions[currPos - 1], easeDuration).SetEase(snapEase);
            currPos--;
            MenuManager.Instance.CloseAllMenu();
        }
    }

    private void SnapCenter()
    {
        mainCameraTransform.DOMoveX(cameraPositions[currPos], easeDuration).SetEase(snapEase);
    }

    public void ZoomCameraOnObject(Vector2 pos)
    {
        enableSwipe = false;
        enableDrag = false;
        tempCameraPosition = mainCameraTransform.position;
        Vector3 snapPosition = new Vector3(pos.x, pos.y, mainCameraTransform.position.z);
        mainCameraTransform.DOMove(snapPosition, easeDuration).SetEase(snapEase).OnComplete(() => isCameraZoomed = true);
        mainCamera.DOOrthoSize(cameraOrthSizeZoom, easeDuration).SetEase(snapEase);
    }

    public void ResetCameraAfterSnap()
    {
        if (isCameraZoomed)
        {
            mainCameraTransform.DOMove(tempCameraPosition, easeDuration).SetEase(snapEase);
            mainCamera.DOOrthoSize(cameraOrthSizeNormal, easeDuration).SetEase(snapEase);
            enableSwipe = true;
            enableDrag = true;
            isCameraZoomed = false;
        }
    }

    public void SnapCameraOnButton(int id)//, float position)
    {
        if (!isTouchingUI || currPos == id)
        {
            return;
        }
        mainCameraTransform.DOMoveX(cameraPositions[id], easeDuration).SetEase(snapEase);
        currPos = id;
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
        mainCameraTransform.DOMoveX(cameraPositions[currPos], easeDuration).SetEase(snapEase);
        MenuManager.Instance.CloseAllMenu();
    }

    private void SwipeEventHandler(SwipeAction swipeAction)
    {
        if (!enableSwipe)
        {
            return;
        }
        if (swipeAction.direction == SwipeDirection.Right)
        {
            SwipeCamera(-1);
        }
        else if (swipeAction.direction == SwipeDirection.Left)
        {
            SwipeCamera(1);
        }
    }
    #endregion
}