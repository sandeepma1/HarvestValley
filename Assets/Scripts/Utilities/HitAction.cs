using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class HitAction : UIBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public event Action OnHitComplete;

    private float hitDuration = 0.75f;
    public float HitDuration { set { hitDuration = value; } }
    private float timer;
    private bool isClicked = false;
    private bool isHold = false;

    protected override void OnDisable()
    {
        isClicked = false;
        isHold = false;
        if (PlayerMovement.Instance != null)
        {
            PlayerMovement.Instance.PlayerToolAction(false);
        }
    }

    private void Update()
    {
        if (isClicked || isHold)
        {
            PlayerController.Instance.isPlayerInAction = true;
            switch (PlayerController.Instance.actionButtonType)
            {
                case ActionButtonType.Untagged:
                    break;
                case ActionButtonType.Field:
                case ActionButtonType.MainCanvas:
                case ActionButtonType.Pickable:
                case ActionButtonType.FlappyFish:
                case ActionButtonType.DragableUiItem:
                case ActionButtonType.Grass:
                case ActionButtonType.Item:
                case ActionButtonType.Enterence:
                case ActionButtonType.Ladder:
                    OneClickEvent();
                    break;
                case ActionButtonType.Pickaxe:
                case ActionButtonType.Sword:
                    HoldEvent();
                    break;
                default:
                    break;
            }
        }
        else
        {
            if (PlayerController.Instance.isPlayerInAction == true)
            {
                PlayerController.Instance.isPlayerInAction = false;
            }
        }
    }

    private void OneClickEvent()
    {
        OnHitComplete.Invoke();
        isClicked = false;
        isHold = false;
    }

    private void HoldEvent()
    {
        PlayerMovement.Instance.PlayerToolAction(true);
        timer += Time.deltaTime;
        if (timer > hitDuration)
        {
            timer = 0;
            isClicked = false;
            OnHitComplete.Invoke();
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
        PlayerMovement.Instance.PlayerToolAction(false);
    }
}

public enum ActionButtonType
{
    Untagged,
    Enterence,
    DroppedItem,
    Pickaxe,
    Sword,
    Field,
    MainCanvas,
    Pickable,
    FlappyFish,
    DragableUiItem,
    Grass,
    Item,
    Ladder
}