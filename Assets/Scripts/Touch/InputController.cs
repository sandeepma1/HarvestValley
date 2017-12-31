using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent(typeof(SwipeManager))]
public class InputController : MonoBehaviour
{
    public static InputController instance = null;
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
    private float minDragDelta = 3;

    [SerializeField]
    private Ease snapEase = Ease.OutQuad;

    [SerializeField]
    private float easeDuration = 0.5f;

    private int currPos;
    private int[] cameraPositions;
    private float touchDeltaPosition;
    private float dragOffset;

    private bool isTouchingUI;
    private bool isDragging;
    private bool isAlreadySnapped;

    private Transform mainCameraTransform;
    Hashtable ease = new Hashtable();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        CreateCameraPositions();
        SwipeManager swipeManager = GetComponent<SwipeManager>();
        swipeManager.onSwipe += SwipeEvent;
        dragOffset = (cameraPositions[1] - cameraPositions[0]) / 3;
        ease.Add("ease", LeanTweenType.easeOutSine);
    }

    private void Update()
    {
        if (enableDrag)
        {
            DragCamera();
        }
    }

    private void CreateCameraPositions()
    {
        if (!Camera.main.CompareTag("MainCamera"))
        {
            Debug.LogError("@@-NO camera in scene or MainCamera tag not assigned");
        }
        mainCameraTransform = Camera.main.transform;
        mainCameraTransform.position = new Vector3(0, 0, -1);
        Camera camera = mainCameraTransform.GetComponent<Camera>();
        //camera.project
        cameraPositions = new int[numberOfScreens];
        for (int i = 0; i < numberOfScreens; i++)
        {
            cameraPositions[i] = gapBetweenScreens * i;
        }
        currPos = numberOfScreens / 2;
        mainCameraTransform.position = new Vector3(cameraPositions[currPos], mainCameraTransform.position.y, mainCameraTransform.position.z);
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
                case "DisableAllMenus":
                    MenuManager.Instance.DisableAllMenus();
                    break;
                case "Toucher":
                    hitObject.GetComponent<Toucher>().TouchUp();
                    break;
                case "Field":
                    hitObject.GetComponent<DraggableBuildings>().TouchedUp();
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
                    mainCameraTransform.Translate(-touchDeltaPosition * dragSpeed, 0, 0);
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
        float posX = mainCameraTransform.position.x;

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
            mainCameraTransform.DOMoveX(cameraPositions[currPos + 1], easeDuration).SetEase(snapEase);
            currPos++;
        }
    }

    private void SnapLeft()
    {
        if (currPos > 0)
        {
            isAlreadySnapped = true;
            mainCameraTransform.DOMoveX(cameraPositions[currPos - 1], easeDuration).SetEase(snapEase);
            currPos--;
        }
    }

    private void SnapCenter()
    {
        mainCameraTransform.DOMoveX(cameraPositions[currPos], easeDuration).SetEase(snapEase);
    }

    public void SnapCameraOnButton(int pos)
    {
        if (currPos != pos)
        {
            currPos = pos;
            DOTween.KillAll();

            //mainCameraTransform.DOMoveX(cameraPositions[currPos], easeDuration).SetEase(snapEase);
            Vector3 newLocation = new Vector3(cameraPositions[currPos], mainCameraTransform.position.y, mainCameraTransform.position.z);
            print(newLocation);
            // mainCameraTransform.DOMove(newLocation, easeDuration, true).SetEase(snapEase);

            //print(cameraPositions[currPos]);
            LeanTween.move(mainCameraTransform.gameObject, newLocation, 0f, ease);
        }
        //print(currPos + " nov " + cameraPositions[currPos]);
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
        mainCameraTransform.DOMoveX(cameraPositions[currPos], 0.5f).SetEase(snapEase);
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