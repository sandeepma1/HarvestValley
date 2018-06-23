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
            PlayerMovement.Instance.PlayerPickaxeAction(false);
        }
    }

    private void Update()
    {
        if (PlayerController.Instance.actionButtonType == ActionButtonType.Enterence)
        {
            if (isClicked)
            {
                OnHitComplete.Invoke();
                isClicked = false;
            }
            return;
        }
        if (isClicked || isHold)
        {
            PlayerMovement.Instance.PlayerPickaxeAction(true);
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

public enum ActionButtonType
{
    Untagged,
    Enterence,
    Pickaxe,
    Sword,
    Field,
    MainCanvas,
    Pickable,
    FlappyFish,
    DragableUiItem,
    Grass,
    Item
}