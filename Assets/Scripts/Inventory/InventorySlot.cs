using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using HarvestValley.IO;
using System;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public event Action<int> OnSlotClicked;
    public int slotId;
    public SlotType slotType;
    private int amount;
    private int itemId;
    [SerializeField]
    private TextMeshProUGUI amountText;
    [SerializeField]
    private Image itemImage;

    public int Amount
    {
        get
        {
            return amount;
        }

        set
        {
            amount = value;
            if (amount <= 0)
            {
                EmptySlot();
            }
            else if (amount == 1)
            {
                amountText.text = "";
            }
            else
            {
                amountText.text = amount.ToString();
            }
        }
    }

    public int ItemId
    {
        get
        {
            return itemId;
        }
        set
        {
            itemId = value;
            if (itemId == -1)
            {
                EmptySlot();
            }
            else
            {
                SetItemImage();
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SelectSlot();
    }

    public void SelectSlot()
    {
        OnSlotClicked.Invoke(slotId);
    }

    public void SetItemImage()
    {
        Amount = 1;
        itemImage.sprite = AtlasBank.Instance.GetSprite(ItemDatabase.GetItemSlugById(itemId), AtlasType.GUI);
    }

    public void EmptySlot()
    {
        amount = 0;
        itemImage.sprite = null;
    }
}