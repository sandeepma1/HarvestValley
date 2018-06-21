using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class LongPressEventTrigger : UIBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public event Action OnHitComplete;

    private float hitDuration = 0.75f;
    public float HitDuration { set { hitDuration = value; } }
    private float timer;
    private bool isClicked = false;
    private bool isHold = false;

    private void Update()
    {
        if (isClicked || isHold)
        {
            PlayerController.Instance.isPlayerInAction = true;
            timer += Time.deltaTime;
            if (timer > hitDuration)
            {
                timer = 0;
                isClicked = false;
                OnHitComplete.Invoke();
                PlayerController.Instance.isPlayerInAction = false;
            }
        }
        else
        {
            PlayerController.Instance.isPlayerInAction = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isHold = true;
        isClicked = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHold = false;
    }
}