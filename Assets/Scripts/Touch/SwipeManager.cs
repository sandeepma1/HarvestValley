using UnityEngine;
using UnityEngine.EventSystems;

public struct SwipeAction
{
    public SwipeDirection direction;
    public Vector2 rawDirection;
    public Vector2 startPosition;
    public Vector2 endPosition;
    public float startTime;
    public float endTime;
    public float duration;
    public float distance;
    public float longestDistance;

    public override string ToString()
    {
        return string.Format("[SwipeAction: {0}, From {1}, To {2}, Delta {3}, Time {4:0.00}s]", direction, rawDirection, startPosition, endPosition, duration);
    }
}

public enum SwipeDirection
{
    None, // Basically means an invalid swipe
    Up,
    UpRight,
    Right,
    DownRight,
    Down,
    DownLeft,
    Left,
    UpLeft
}

/// <summary>
/// Swipe manager.
/// BASED ON: http://forum.unity3d.com/threads/swipe-in-all-directions-touch-and-mouse.165416/#post-1516893
/// </summary>
public class SwipeManager : MonoBehaviour
{
    public System.Action<SwipeAction> onSwipeEvent;

    [Range(0f, 200f), SerializeField]
    private float minSwipeLength = 100;

    [Range(0.1f, 1f), SerializeField]
    private float minSwipeDuration = 0.4f;

    private Vector2 currentSwipe;
    private SwipeAction currentSwipeAction = new SwipeAction();

    private bool isTouchingUI;

    private void Update()
    {
        DetectSwipe();
    }

    public void DetectSwipe()
    {
        var touches = InputHelper.GetTouches();
        if (touches.Count > 0)
        {
            Touch t = touches[0];
            if (t.phase == TouchPhase.Began)
            {
                if (EventSystem.current.IsPointerOverGameObject(t.fingerId))
                {
                    isTouchingUI = true;
                }
                ResetCurrentSwipeAction(t);
            }
            if (t.phase == TouchPhase.Moved || t.phase == TouchPhase.Stationary)
            {
                UpdateCurrentSwipeAction(t);
                if (currentSwipeAction.longestDistance < minSwipeLength)
                {
                    currentSwipeAction.direction = SwipeDirection.None; // Invalidate current swipe action
                    return;
                }
            }
            if (t.phase == TouchPhase.Ended)
            {
                UpdateCurrentSwipeAction(t);
                if (currentSwipeAction.distance < minSwipeLength) // Didn't swipe enough or this is a long press
                {
                    currentSwipeAction.direction = SwipeDirection.None; // Invalidate current swipe action
                    return;
                }
                if (onSwipeEvent != null && currentSwipeAction.duration < minSwipeDuration && !isTouchingUI)
                {
                    onSwipeEvent(currentSwipeAction); // Fire event
                }
                isTouchingUI = false;
            }
        }
    }

    private void ResetCurrentSwipeAction(Touch t)
    {
        currentSwipeAction.duration = 0f;
        currentSwipeAction.distance = 0f;
        currentSwipeAction.longestDistance = 0f;
        currentSwipeAction.startPosition = new Vector2(t.position.x, t.position.y);
        currentSwipeAction.startTime = Time.time;
        currentSwipeAction.endPosition = currentSwipeAction.startPosition;
        currentSwipeAction.endTime = currentSwipeAction.startTime;
    }

    private void UpdateCurrentSwipeAction(Touch t)
    {
        currentSwipeAction.endPosition = new Vector2(t.position.x, t.position.y);
        currentSwipeAction.endTime = Time.time;
        currentSwipeAction.duration = currentSwipeAction.endTime - currentSwipeAction.startTime;
        currentSwipe = currentSwipeAction.endPosition - currentSwipeAction.startPosition;
        currentSwipeAction.rawDirection = currentSwipe;
        currentSwipeAction.direction = GetSwipeDirection(currentSwipe);
        currentSwipeAction.distance = Vector2.Distance(currentSwipeAction.startPosition, currentSwipeAction.endPosition);
        if (currentSwipeAction.distance > currentSwipeAction.longestDistance) // If new distance is longer than previously longest
        {
            currentSwipeAction.longestDistance = currentSwipeAction.distance; // Update longest distance
        }
    }

    SwipeDirection GetSwipeDirection(Vector2 direction)
    {
        var angle = Vector2.Angle(Vector2.up, direction.normalized); // Degrees
        var swipeDirection = SwipeDirection.None;

        if (direction.x > 0) // Right
        {
            if (angle < 22.5f) // 0.0 - 22.5
            {
                swipeDirection = SwipeDirection.Up;
            } else if (angle < 67.5f) // 22.5 - 67.5
            {
                swipeDirection = SwipeDirection.UpRight;
            } else if (angle < 112.5f) // 67.5 - 112.5
            {
                swipeDirection = SwipeDirection.Right;
            } else if (angle < 157.5f) // 112.5 - 157.5
            {
                swipeDirection = SwipeDirection.DownRight;
            } else if (angle < 180.0f) // 157.5 - 180.0
            {
                swipeDirection = SwipeDirection.Down;
            }
        } else // Left
        {
            if (angle < 22.5f) // 0.0 - 22.5
            {
                swipeDirection = SwipeDirection.Up;
            } else if (angle < 67.5f) // 22.5 - 67.5
            {
                swipeDirection = SwipeDirection.UpLeft;
            } else if (angle < 112.5f) // 67.5 - 112.5
            {
                swipeDirection = SwipeDirection.Left;
            } else if (angle < 157.5f) // 112.5 - 157.5
            {
                swipeDirection = SwipeDirection.DownLeft;
            } else if (angle < 180.0f) // 157.5 - 180.0
            {
                swipeDirection = SwipeDirection.Down;
            }
        }
        return swipeDirection;
    }
}