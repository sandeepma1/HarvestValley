using UnityEngine;
using DG.Tweening;

public class DroppedItem : MonoBehaviour
{
    public int itemId;
    [SerializeField]
    private SpriteRenderer itemSprite;

    private CircleCollider2D collider2D;
    //private Sequence sequence;
    private bool isPlayerFound;
    private readonly float itemToPlayerDuration = 0.25f;
    private readonly float itemBounceDuration = 0.35f;

    private void Start()
    {
        //itemSprite.transform.do
        //sequence = DOTween.Sequence();
        collider2D = GetComponent<CircleCollider2D>();
        SetCollider2d(false);
        itemSprite.sprite = AtlasBank.Instance.GetSprite(itemId, AtlasType.GUI);
        StartDropAnimation();
    }

    private void StartDropAnimation()
    {
        //sequence.Append(itemSprite.transform.DOLocalMoveY(1f, itemBounceDuration / 2))
        //   .Append(itemSprite.transform.DOLocalMoveY(0, itemBounceDuration).SetEase(Ease.OutBounce)).OnComplete(() => SetCollider2d(true));
        itemSprite.transform.DOPunchPosition(Vector3.up, itemBounceDuration, 10, 1).OnComplete(() => SetCollider2d(true));
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isPlayerFound)
        {
            return;
        }
        if (collision.tag == "Player")
        {
            isPlayerFound = true;
            transform.DOMove(collision.transform.position, itemToPlayerDuration).OnComplete(ItemAddedToInventory);
        }
    }

    private void ItemAddedToInventory()
    {
        Inventory.Instance.AddItem(itemId);
        Destroy(gameObject);
    }

    private void SetCollider2d(bool flag)
    {
        collider2D.enabled = flag;
    }
}