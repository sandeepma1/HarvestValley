using UnityEngine;
using TMPro;
using HarvestValley.IO;

namespace HarvestValley.Ui
{
    public class ItemPopupProduction : Singleton<ItemPopupProduction>
    {
        public TextMeshPro itemName;
        public TextMeshPro itemDuration;
        public GameObject item1Icon, item2Icon, item3Icon, item4Icon;
        public TextMeshPro item1Costing, item2Costing, item3Costing, item4Costing;
        //public TextMeshPro item1Required, item2Required, item3Required, item4Required;

        int popupOffset = 135;
        // Use this for initialization
        void Start()
        {
            HideItemPopupProduction();
        }

        public void DisplayItemPopupProduction_DOWN(int itemID, Vector2 pos)
        {
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y + popupOffset, 10));

            itemName.text = ItemDatabase.Instance.items[itemID].name;
            itemDuration.text = ItemDatabase.Instance.items[itemID].timeRequiredInSeconds.ToString();
            if (ItemDatabase.Instance.items[itemID].needID1 >= 0)
            {
                item1Icon.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Textures/Items/" + ItemDatabase.Instance.items[ItemDatabase.Instance.items[itemID].needID1].name);
                item1Costing.text = UiInventoryMenu.Instance.playerInventory[ItemDatabase.Instance.items[itemID].needID1].count.ToString() + "/" + ItemDatabase.Instance.items[itemID].needAmount1.ToString();
                if (QuickCheckItemAvaliable(UiInventoryMenu.Instance.playerInventory[ItemDatabase.Instance.items[itemID].needID1].count, ItemDatabase.Instance.items[itemID].needAmount1))
                {
                    item1Costing.GetComponent<TextMeshPro>().color = HexToColor("A7630DFF");
                }
                else
                {
                    item1Costing.GetComponent<TextMeshPro>().color = Color.red;
                }
            }
            if (ItemDatabase.Instance.items[itemID].needID2 >= 0)
            {
                item2Icon.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Textures/Items/" + ItemDatabase.Instance.items[ItemDatabase.Instance.items[itemID].needID2].name);
                item2Costing.text = UiInventoryMenu.Instance.playerInventory[ItemDatabase.Instance.items[itemID].needID2].count.ToString() + "/" + ItemDatabase.Instance.items[itemID].needAmount2.ToString();
                if (QuickCheckItemAvaliable(UiInventoryMenu.Instance.playerInventory[ItemDatabase.Instance.items[itemID].needID2].count, ItemDatabase.Instance.items[itemID].needAmount2))
                {
                    item2Costing.GetComponent<TextMeshPro>().color = HexToColor("A7630DFF");
                }
                else
                {
                    item2Costing.GetComponent<TextMeshPro>().color = Color.red;
                }
            }
            if (ItemDatabase.Instance.items[itemID].needID3 >= 0)
            {
                item3Icon.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Textures/Items/" + ItemDatabase.Instance.items[ItemDatabase.Instance.items[itemID].needID3].name);
                item3Costing.text = UiInventoryMenu.Instance.playerInventory[ItemDatabase.Instance.items[itemID].needID3].count.ToString() + "/" + ItemDatabase.Instance.items[itemID].needAmount3.ToString();
                if (QuickCheckItemAvaliable(UiInventoryMenu.Instance.playerInventory[ItemDatabase.Instance.items[itemID].needID3].count, ItemDatabase.Instance.items[itemID].needAmount3))
                {
                    item3Costing.GetComponent<TextMeshPro>().color = HexToColor("A7630DFF");
                }
                else
                {
                    item3Costing.GetComponent<TextMeshPro>().color = Color.red;
                }
            }
            if (ItemDatabase.Instance.items[itemID].needID4 >= 0)
            {
                item4Icon.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Textures/Items/" + ItemDatabase.Instance.items[ItemDatabase.Instance.items[itemID].needID4].name);
                item4Costing.text = UiInventoryMenu.Instance.playerInventory[ItemDatabase.Instance.items[itemID].needID4].count.ToString() + "/" + ItemDatabase.Instance.items[itemID].needAmount4.ToString();
                if (QuickCheckItemAvaliable(UiInventoryMenu.Instance.playerInventory[ItemDatabase.Instance.items[itemID].needID4].count, ItemDatabase.Instance.items[itemID].needAmount4))
                {
                    item4Costing.GetComponent<TextMeshPro>().color = HexToColor("A7630DFF");
                }
                else
                {
                    item4Costing.GetComponent<TextMeshPro>().color = Color.red;
                }
            }
        }

        bool QuickCheckItemAvaliable(int has, int required)
        {
            if (has >= required)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void DisplayItemPopupProduction_DRAG(Vector2 pos)
        {
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y + popupOffset, 10));
        }

        public void HideItemPopupProduction()
        {
            transform.position = new Vector3(-500, -500, 0);
            itemName.text = "";
            itemDuration.text = "";
            item1Icon.GetComponent<SpriteRenderer>().sprite = new Sprite();
            item1Costing.GetComponent<TextMeshPro>().color = HexToColor("A7630DFF");
            item1Costing.text = "";
            item2Icon.GetComponent<SpriteRenderer>().sprite = new Sprite();
            item2Costing.GetComponent<TextMeshPro>().color = HexToColor("A7630DFF");
            item2Costing.text = "";
            item3Icon.GetComponent<SpriteRenderer>().sprite = new Sprite();
            item3Costing.GetComponent<TextMeshPro>().color = HexToColor("A7630DFF");
            item3Costing.text = "";
            item4Icon.GetComponent<SpriteRenderer>().sprite = new Sprite();
            item4Costing.GetComponent<TextMeshPro>().color = HexToColor("A7630DFF");
            item4Costing.text = "";
        }

        string ColorToHex(Color32 color)
        {
            string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
            return hex;
        }

        Color HexToColor(string hex)
        {
            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            return new Color32(r, g, b, 255);
        }
    }
}