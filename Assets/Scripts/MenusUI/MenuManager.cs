using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public enum MenuNames
{
    LevelUp,
    BuldingUpgrade,
    Inventory,
    UIItemScrollList,
    Harvest
}

public class MenuManager : Singleton<MenuManager>
{
    [SerializeField]
    private GameObject mainCanvas;
    [SerializeField]
    private GameObject MINEBUTTON;

    //Add all types of menus here
    [SerializeField]
    private GameObject levelUpMenu;
    [SerializeField]
    private GameObject buildingUpgradeMenu;
    [SerializeField]
    private GameObject inventoryMenu;
    [SerializeField]
    private GameObject uiItemScrollList;
    [SerializeField]
    private GameObject harvestMenu;

    private Stack<GameObject> openMenusStack = new Stack<GameObject>();

    private void Awake()
    {
        if (mainCanvas != null)
        {
            mainCanvas.SetActive(true);
        }
        if (Application.isEditor)
        {
            MINEBUTTON.SetActive(false);
        }
        else
        {
            MINEBUTTON.SetActive(true);
        }
    }

    private void Start()
    {

    }

    private void Update()
    {
        /*if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WindowsEditor) {
			if (Input.GetKey (KeyCode.Escape)) {
				print ("escape");
				if (SceneManager.GetActiveScene ().name == "Menu") {
					Application.Quit ();
				} else {
					//ShowIGMMenu ();
				}
				return;
			}
		}*/
    }

    #region Stack Stuff

    public void DisplayMenu(MenuNames menuName)
    {
        switch (menuName)
        {
            case MenuNames.LevelUp:
                AddInStack(levelUpMenu);
                break;
            case MenuNames.BuldingUpgrade:
                AddInStack(buildingUpgradeMenu);
                break;
            case MenuNames.Inventory:
                AddInStack(inventoryMenu);
                break;
            case MenuNames.UIItemScrollList:
                AddInStack(uiItemScrollList);
                break;
            case MenuNames.Harvest:
                AddInStack(harvestMenu);
                break;
            default:
                break;
        }
    }

    private void AddInStack(GameObject menu)
    {
        if (openMenusStack.Contains(menu))
        {
            return;
        }
        menu.SetActive(true);
        openMenusStack.Push(menu);
        ShowAllStackItems();
    }

    private void RemoveFromStack()
    {
        if (openMenusStack.Count >= 1)
        {
            openMenusStack.Peek().SetActive(false);
            openMenusStack.Pop();
            ShowAllStackItems();
        }
        else
        {
            Debug.LogWarning("No menus are open, stack is empty");
        }
    }

    private void ClearStack()
    {
        openMenusStack.Clear();
    }

    private void ShowAllStackItems()
    {
        foreach (var item in openMenusStack)
        {
            print(item.name);
        }
    }

    #endregion

    public void LevelUpMenuSetActive(bool flag)
    {
        levelUpMenu.SetActive(flag);
    }

    public void BuildingUpgradeMenuSetActive(bool flag)
    {
        buildingUpgradeMenu.SetActive(flag);
    }

    public void InventoryMenuSetActive(bool flag)
    {
        inventoryMenu.SetActive(flag);
    }

    public void ShopMenuSetActive(bool flag)
    {
        //shopMenu.SetActive(flag);
    }

    #region Touch stuff
    private void OnMouseUp() // TODO: somehow inherate from MouseUpBase
    {
        var touch = InputHelper.GetTouches();
        if (touch.Count > 0)
        {
            Touch t = touch[0];

            if (Application.isEditor && EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            else if (EventSystem.current.IsPointerOverGameObject(t.fingerId))
            {
                return;
            }

            if (!InputController.Instance.IsDragging)
            {
                RemoveFromStack();
            }
        }
    }
    #endregion

    #region UI Button calls

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void CloseGameObject(GameObject go)
    {
        go.SetActive(false);
    }

    public void OpenGameObject(GameObject go)
    {
        go.SetActive(true);
    }
    #endregion
}