using UnityEngine;

public class DraggableItemShopUIItem : MonoBehaviour
{
    public int shopItemID = 0;
    Vector3 intialPosition;

    public void OnDown()
    {
        intialPosition = transform.localPosition;
    }

    public void OnDrag()
    {
        ShopMenuManager.Instance.ChildCallingOnMouseDrag(shopItemID);
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) - BuildingsManager.Instance.transform.position;
    }

    public void OnUp()
    {
        transform.localPosition = intialPosition;
        print(Camera.main.ScreenToWorldPoint(new Vector3(Mathf.RoundToInt(Input.mousePosition.x), Mathf.RoundToInt(Input.mousePosition.y)))
        - BuildingsManager.Instance.transform.position);
        ShopMenuManager.Instance.ChildCallingOnMouseUp(shopItemID, Camera.main.ScreenToWorldPoint(new Vector3(Mathf.RoundToInt(Input.mousePosition.x), Mathf.RoundToInt(Input.mousePosition.y)))
        - BuildingsManager.Instance.transform.position);
    }
}
