using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopMenuManager : MonoBehaviour
{
    public static ShopMenuManager Instance = null;
    public GameObject shopUIMenu, shopToggleButton, shopitemPrefab, UIList;
    public GameObject PlaceHolderSprite;

    public GameObject[] shopItemsGO;
    bool showShop = false;

    void Awake()
    {
        Instance = this;
        PopulateScrollListAtStart();
    }

    public void PopulateScrollListAtStart()
    {
        // Image, background, name, desc
        shopItemsGO = new GameObject[SourceDatabase.Instance.sources.Length];
        for (int i = 0; i < SourceDatabase.Instance.sources.Length; i++)
        {
            shopItemsGO[i] = Instantiate(shopitemPrefab, UIList.transform);
            shopItemsGO[i].transform.GetChild(0).GetComponent<DraggableItemShopUIItem>().shopItemID = i;
            shopItemsGO[i].transform.GetChild(0).GetComponent<Image>().overrideSprite = Resources.Load<Sprite>("Textures/Buildings/" + SourceDatabase.Instance.sources[i].sourceID);
            shopItemsGO[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = SourceDatabase.Instance.sources[i].sourceID.ToString();
            shopItemsGO[i].transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = SourceDatabase.Instance.sources[i].desc.ToString();
            shopItemsGO[i].transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = SourceDatabase.Instance.sources[i].cost.ToString();
        }
    }

    public void ChildCallingOnMouseDrag(int shopItemID)
    {
        PlaceHolderSprite.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Textures/Buildings/" + SourceDatabase.Instance.sources[shopItemID].sourceID);
        PlaceHolderSprite.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
        PlaceHolderSprite.transform.position = new Vector3(Mathf.RoundToInt(PlaceHolderSprite.transform.position.x), Mathf.RoundToInt(PlaceHolderSprite.transform.position.y));
    }

    public void ChildCallingOnMouseUp(int shopItemID, Vector2 pos)
    {
        pos = new Vector2(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
        BuildingsManager.Instance.AddNewBuilding(pos, shopItemID);
        PlaceHolderSprite.transform.position = new Vector3(500, 500, 0);

    }

    public void ToggleShopView()
    {
        showShop = !showShop;
        if (showShop)
        {  //show shop
            shopUIMenu.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 175);
            shopToggleButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-50, 50);
        } else
        {
            shopUIMenu.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -175);
            shopToggleButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-50, 120);
        }
    }
}

