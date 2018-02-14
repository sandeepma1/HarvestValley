using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class MoreInventoryButton : MonoBehaviour
{
    public static MoreInventoryButton Instance = null;
    public bool useLeftAnalogStick = false;
    public RectTransform mainUIWindow;
    public GameObject mainCanvas, inventoryMenu;
    public GameObject sortButton, inventoryUpButton, closeInventoryButton;
    //public rightStick;
    public GameObject craftingMenu;

    private bool toggleCrafting = false;

    float heightAdjuster;
    int tabIndex = 0;
    Hashtable linearEase = new Hashtable();

    // = FindObjectsOfType (typeof(Devdog.InventorySystem.InventoryUIItemWrapper)) as Devdog.InventorySystem.InventoryUIItemWrapper[];
    void Awake()
    {
        Instance = this;
        linearEase.Add("ease", LeanTweenType.easeOutQuad);
        //Camera.main.transform.position =
    }

    void Start()
    {
        tabIndex = inventoryMenu.GetComponent<RectTransform>().GetSiblingIndex();
        //		print (Screen.height);
        heightAdjuster = Screen.height * 2;
        ToggleInventorySize(true);
    }

    public void MoveCamera(float posX)
    {
        LeanTween.moveX(Camera.main.transform.gameObject, posX, 0.5f, linearEase);
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("Main");
    }

    public void ToggleInventorySize(bool isInventoryDown)
    {
        ToggleInventory(isInventoryDown);

        if (isInventoryDown)
        {
            inventoryMenu.GetComponent<RectTransform>().SetSiblingIndex(tabIndex);
            GEM.SetState(GEM.E_STATES.e_game);
            mainUIWindow.anchoredPosition = new Vector3(mainUIWindow.anchoredPosition.x, heightAdjuster);
            //mainUIWindow.anchoredPosition = Vector3.zero;
            GEM.SetMenuState(GEM.E_MenuState.e_menuDown);
        } else
        {
            inventoryMenu.GetComponent<RectTransform>().SetSiblingIndex(tabIndex);
            GEM.SetState(GEM.E_STATES.e_pause);
            //mainUIWindow.anchoredPosition = new Vector3 (mainUIWindow.anchoredPosition.x, heightAdjuster);
            mainUIWindow.anchoredPosition = Vector3.zero;
            GEM.SetMenuState(GEM.E_MenuState.e_menuUp);
        }
    }

    public void ToggleCameraTownMine(bool isTown)
    {
        if (isTown)
        {
            Camera.main.transform.position = new Vector3(12f, 0, -200);
            Camera.main.orthographicSize = 9.5f;
        } else
        {
            Camera.main.transform.position = new Vector3(0, 0, -200);
            Camera.main.orthographicSize = 8f;
        }
    }

    public void ShowCraftingMenu(bool isCraftingDown)
    {
        ToggleInventory(isCraftingDown);
        if (isCraftingDown)
        {
            craftingMenu.GetComponent<RectTransform>().SetSiblingIndex(tabIndex);
            GEM.SetState(GEM.E_STATES.e_game);
            mainUIWindow.anchoredPosition = new Vector3(mainUIWindow.anchoredPosition.x, heightAdjuster);
            GEM.SetMenuState(GEM.E_MenuState.e_menuDown);
        } else
        {
            craftingMenu.GetComponent<RectTransform>().SetSiblingIndex(tabIndex);
            GEM.SetState(GEM.E_STATES.e_pause);
            mainUIWindow.anchoredPosition = Vector3.zero;
            GEM.SetMenuState(GEM.E_MenuState.e_menuUp);
        }
    }

    void ToggleInventory(bool flag)
    {
        //if (Bronz.LocalStore.Instance.GetBool ("TouchControls")) {
        //		leftStick.SetActive (flag);
        //}
        closeInventoryButton.SetActive(!flag);
        //inventoryTab.SetActive (!flag);
        sortButton.SetActive(!flag);
        //craftingTab.SetActive (!flag);
        //settingsTab.SetActive (!flag);
        //infoTab.SetActive (!flag);
        //runWalkButton.SetActive (flag);
        //actionButton.SetActive (flag);
        inventoryUpButton.SetActive(flag);
        //craftingUpButton.SetActive (flag);
        //miniMapButton.SetActive (flag);
    }

    public void ToggleCrafting()
    {
        toggleCrafting = !toggleCrafting;
    }

    IEnumerator SaveToPNG()
    {
        yield return new WaitForEndOfFrame();
        Texture2D screenTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);
        screenTexture.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height), 0, 0);
        screenTexture.Apply();
        byte[] dataToSave = screenTexture.EncodeToPNG();
        string destination = Path.Combine(Application.persistentDataPath, System.DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".png");
        File.WriteAllBytes(destination, dataToSave);
    }
}
