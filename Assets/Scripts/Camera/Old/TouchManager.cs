//link: https://forum.unity.com/threads/swipe-in-all-directions-touch-and-mouse.165416/page-2#post-2741253
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine.UI;

namespace HarvestValley.TouchClickInput
{
    class CardinalDirection
    {
        public static readonly Vector2 Up = new Vector2(0, 1);
        public static readonly Vector2 Down = new Vector2(0, -1);
        public static readonly Vector2 Right = new Vector2(1, 0);
        public static readonly Vector2 Left = new Vector2(-1, 0);
        public static readonly Vector2 UpRight = new Vector2(1, 1);
        public static readonly Vector2 UpLeft = new Vector2(-1, 1);
        public static readonly Vector2 DownRight = new Vector2(1, -1);
        public static readonly Vector2 DownLeft = new Vector2(-1, -1);
    }

    public enum Swipe
    {
        None,
        Up,
        Down,
        Left,
        Right,
        UpLeft,
        UpRight,
        DownLeft,
        DownRight
    };

    public enum _Touch
    {
        Down,
        Up,
        Enter,
        Exit
    };

    public class TouchManager : MonoBehaviour
    {
        #region Inspector Variables
        [Tooltip("Min swipe distance (inches) to register as swipe")]
        [SerializeField]
        float minSwipeLength = 0.5f;

        [Tooltip("If true, a swipe is counted when the min swipe length is reached. If false, a swipe is counted when the touch/click ends.")]
        [SerializeField]
        bool triggerSwipeAtMinLength = false;

        [Tooltip("Whether to detect eight or four cardinal directions")]
        [SerializeField]
        bool useEightDirections = false;

        [SerializeField]
        float dragSpeed = 2;
        [SerializeField]
        float maxSwipeVelocity = 30;
        [SerializeField]
        float minDragDistance = 0.1f;
        #endregion

        #region variables
        const float eightDirAngle = 0.906f;
        const float fourDirAngle = 0.5f;
        const float defaultDPI = 72f;
        const float dpcmFactor = 2.54f;

        public static Vector2 swipeVelocity;
        static float dpcm;
        static float swipeStartTime;
        static float swipeEndTime;
        static bool autoDetectSwipes;
        static bool swipeEnded;
        static Swipe swipeDirection;
        static Vector2 firstPressPos;
        static Vector2 secondPressPos;

        static TouchManager instance;
        static private Vector2 position;
        static private Vector2 movePosition = Vector2.zero;
        static private Vector2 tempMovePosition = Vector2.zero;
        static private Hashtable ease = new Hashtable();
        static private int currentCameraPostion = 3;
        static private int snapThreshold = 6;
        static Transform thisTransform;

        static Dictionary<Swipe, Vector2> cardinalDirections = new Dictionary<Swipe, Vector2>()
    {
        { Swipe.Up,         CardinalDirection.Up                 },
        { Swipe.Down,         CardinalDirection.Down             },
        { Swipe.Right,         CardinalDirection.Right             },
        { Swipe.Left,         CardinalDirection.Left             },
        { Swipe.UpRight,     CardinalDirection.UpRight             },
        { Swipe.UpLeft,     CardinalDirection.UpLeft             },
        { Swipe.DownRight,     CardinalDirection.DownRight         },
        { Swipe.DownLeft,     CardinalDirection.DownLeft         }
    };

        #endregion

        public delegate void OnSwipeDetectedHandler(Swipe swipeDirection, Vector2 swipeVelocity);
        static OnSwipeDetectedHandler _OnSwipeDetected;
        public static event OnSwipeDetectedHandler OnSwipeDetected
        {
            add
            {
                _OnSwipeDetected += value;
                autoDetectSwipes = true;
            }
            remove
            {
                _OnSwipeDetected -= value;
            }
        }

        public delegate void OnTouchUp(Vector3 position);
        static OnTouchUp _OnTouchUp;
        public static event OnTouchUp OnTouchUpDetected
        {
            add
            {
                _OnTouchUp += value;
            }
            remove
            {
                _OnTouchUp -= value;
            }
        }

        public delegate void OnTouchDown(Vector3 position);
        static OnTouchDown _OnTouchDown;
        public static event OnTouchDown OnTouchDownDetected
        {
            add
            {
                _OnTouchDown += value;
            }
            remove
            {
                _OnTouchDown -= value;
            }
        }

        public delegate void OnScreenDrag(Vector3 position);
        static OnScreenDrag _OnScreenDrag;
        public static event OnScreenDrag OnScreenDragDetected
        {
            add
            {
                _OnScreenDrag += value;
            }
            remove
            {
                _OnScreenDrag -= value;
            }
        }

        void Awake()
        {
            instance = this;
            thisTransform = this.transform;
            float dpi = (Screen.dpi == 0) ? defaultDPI : Screen.dpi;
            dpcm = dpi / dpcmFactor;
        }

        void Start()
        {
            //ease.Add("ease", LeanTweenType.easeOutQuint);
            OnSwipeDetected += OnSwipeDetectedFun;
        }

        void OnSwipeDetectedFun(Swipe direction, Vector2 swipeVelcity)
        {
            switch (direction)
            {
                case Swipe.Left:
                    SnapCamera(1);
                    break;
                case Swipe.Right:
                    SnapCamera(-1);
                    break;
                default:
                    break;
            }
        }

        void Update()
        {
            if (autoDetectSwipes)
            {
                //#if UNITY_EDITOR
                if (GetMouseInput())
                {
                    DetectSwipe();
                } else
                {
                    swipeDirection = Swipe.None;
                }
            }
        }

        static void DetectSwipe()
        {
            if (swipeEnded) { return; }  // Swipe already ended, don't detect until a new swipe has begun
            Vector2 currentSwipe = secondPressPos - firstPressPos;
            float swipeCm = currentSwipe.magnitude / dpcm;
            if (swipeCm < instance.minSwipeLength) // Check the swipe is long enough to count as a swipe (not a touch, etc)
            {
                if (!instance.triggerSwipeAtMinLength) // Swipe was not long enough, its a Touch
                {
                    if (_OnTouchUp != null)
                    {
                        _OnTouchUp(secondPressPos);
                    }
                    swipeDirection = Swipe.None;
                }
                return;
            }
            swipeEndTime = Time.time;
            swipeVelocity = currentSwipe * (swipeEndTime - swipeStartTime);
            swipeDirection = GetSwipeDirByTouch(currentSwipe);
            swipeEnded = true;
            //print("Swipe");
            if (_OnSwipeDetected != null)
            {
                _OnSwipeDetected(swipeDirection, swipeVelocity);
            }

        }

        #region Helper Functions

        static bool GetTouchInput()
        {
            if (Input.touches.Length > 0)
            {
                Touch t = Input.GetTouch(0);
                if (t.phase == TouchPhase.Began)  // Swipe/Touch started
                {
                    firstPressPos = t.position;
                    if (_OnTouchDown != null)
                    {
                        _OnTouchDown(firstPressPos);
                    }
                    swipeStartTime = Time.time;
                    swipeEnded = false;
                } else if (t.phase == TouchPhase.Ended)  // Swipe/Touch ended
                {
                    secondPressPos = t.position;
                    SnapCamera(DetectDragMoveDirection());
                    return true;
                } else     // Still swiping/touching
                {
                    float distance = t.position.x - firstPressPos.x;
                    position = Camera.main.ScreenToViewportPoint(t.position - firstPressPos);
                    DetectDrag(position, distance);
                    if (instance.triggerSwipeAtMinLength) // Could count as a swipe if length is long enough
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        static bool GetMouseInput()
        {
            if (GEM.isObjectDragging)
            {
                //swipeEnded = false;
                secondPressPos = firstPressPos = Vector2.zero;
                return false;
            }
            //print("GetMouseInput");
            if (Input.GetMouseButtonDown(0))  // Swipe/Click started
            {
                firstPressPos = Input.mousePosition;
                if (_OnTouchDown != null)
                {
                    _OnTouchDown(firstPressPos);
                }
                swipeStartTime = Time.time;
                swipeEnded = false;
            } else if (Input.GetMouseButtonUp(0))  // Swipe/Click ended
            {
                secondPressPos = Input.mousePosition;
                SnapCamera(DetectDragMoveDirection());
                return true;
            } else // Still swiping/clicking
            {
                if (instance.triggerSwipeAtMinLength)  // Could count as a swipe if length is long enough
                {
                    return true;
                }
            }
            if (Input.GetMouseButton(0))
            {
                float distance = Input.mousePosition.x - firstPressPos.x;
                Vector2 distanceDelta = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - firstPressPos;
                position = Camera.main.ScreenToViewportPoint(distanceDelta);
                DetectDrag(position, distance);
            }
            return false;
        }

        static void DetectDrag(Vector2 position, float distance)
        {
            movePosition = new Vector2(-position.x * 2, 0);
            if (distance > 0.1 || distance < -0.1)  //Drag
            {
                Vector2 dragDelta = movePosition - tempMovePosition;
                thisTransform.position += new Vector3(dragDelta.x, dragDelta.y);
                if (_OnScreenDrag != null)
                {
                    _OnScreenDrag(movePosition - tempMovePosition);
                }
            }
            tempMovePosition = movePosition;
        }

        static void SnapCamera(int snapState)
        {
            if (currentCameraPostion == 1 && snapState == -1)
            {
                snapState = 0;
            }

            if (currentCameraPostion == 5 && snapState == 1)
            {
                snapState = 0;
            }

            switch (snapState)
            {
                case -1:
                    currentCameraPostion--;
                    break;
                case 1:
                    currentCameraPostion++;
                    break;
                case 0:
                    break;
                default:
                    break;
            }
            LeanTween.moveX(Camera.main.gameObject, GEM.screensPositions[currentCameraPostion], 0.35f, ease);
        }

        static int DetectDragMoveDirection()
        {
            //move right
            if (thisTransform.position.x > GEM.screensPositions[currentCameraPostion] + snapThreshold)
            {
                return 1;
            }

            //Move Left
            if (thisTransform.position.x < GEM.screensPositions[currentCameraPostion] - snapThreshold)
            {
                return -1;
            }
            return 0;
        }

        static bool IsDirection(Vector2 direction, Vector2 cardinalDirection)
        {
            var angle = instance.useEightDirections ? eightDirAngle : fourDirAngle;
            return Vector2.Dot(direction, cardinalDirection) > angle;
        }

        static Swipe GetSwipeDirByTouch(Vector2 currentSwipe)
        {
            currentSwipe.Normalize();
            var swipeDir = cardinalDirections.FirstOrDefault(dir => IsDirection(currentSwipe, dir.Value));
            return swipeDir.Key;
        }

        static bool IsSwipingDirection(Swipe swipeDir)
        {
            DetectSwipe();
            return swipeDirection == swipeDir;
        }

        #endregion

        public static bool IsSwiping() { return swipeDirection != Swipe.None; }
        public static bool IsSwipingRight() { return IsSwipingDirection(Swipe.Right); }
        public static bool IsSwipingLeft() { return IsSwipingDirection(Swipe.Left); }
        public static bool IsSwipingUp() { return IsSwipingDirection(Swipe.Up); }
        public static bool IsSwipingDown() { return IsSwipingDirection(Swipe.Down); }
        public static bool IsSwipingDownLeft() { return IsSwipingDirection(Swipe.DownLeft); }
        public static bool IsSwipingDownRight() { return IsSwipingDirection(Swipe.DownRight); }
        public static bool IsSwipingUpLeft() { return IsSwipingDirection(Swipe.UpLeft); }
        public static bool IsSwipingUpRight() { return IsSwipingDirection(Swipe.UpRight); }

    }
}