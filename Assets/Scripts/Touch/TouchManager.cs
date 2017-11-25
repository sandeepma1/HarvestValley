using UnityEngine;
using System.Collections;

public class TouchManager : MonoBehaviour
{
    Vector2 firstPressPos, secondPressPos;
    float swipeStartTime;
    bool swipeEnded;
    bool triggerSwipeAtMinLength;

    void Update()
    {
        DetectSwipe();
    }

    void DetectSwipe()
    {
        if (GetTouchInput() || GetMouseInput())
        {
            if (swipeEnded)
            {
                return;
            }
        }
    }

    bool GetTouchInput()
    {
        if (Input.touches.Length > 0)
        {
            Touch t = Input.GetTouch(0);
            // Swipe/Touch started
            if (t.phase == TouchPhase.Began)
            {
                firstPressPos = t.position;
                swipeStartTime = Time.time;
                swipeEnded = false;
                // Swipe/Touch ended
            } else if (t.phase == TouchPhase.Ended)
            {
                secondPressPos = t.position;
                return true;
                // Still swiping/touching
            } else
            {
                // Could count as a swipe if length is long enough
                if (triggerSwipeAtMinLength)
                {
                    return true;
                }
            }
        }
        return false;
    }

    bool GetMouseInput()
    {
        // Swipe/Click started
        if (Input.GetMouseButtonDown(0))
        {
            firstPressPos = (Vector2)Input.mousePosition;
            swipeStartTime = Time.time;
            swipeEnded = false;
            // Swipe/Click ended
        } else if (Input.GetMouseButtonUp(0))
        {
            secondPressPos = (Vector2)Input.mousePosition;
            return true;
            // Still swiping/clicking
        } else
        {
            // Could count as a swipe if length is long enough
            if (triggerSwipeAtMinLength)
            {
                return true;
            }
        }
        return false;
    }
}
