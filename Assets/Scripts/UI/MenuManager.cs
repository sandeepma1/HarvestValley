using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using HarvestValley.Managers;

public enum MenuNames
{
    LevelUp,
    BuldingUpgrade,
    Inventory,
    SeedList,
    Navigation,
    FieldProgress,
    FieldUpgrade,
    BuildingMenu
}

public enum MenuOpeningType
{
    CloseAll,
    OnTop
}

namespace HarvestValley.Ui
{
    public class MenuManager : Singleton<MenuManager>
    {
        [SerializeField]
        private GameObject mainCanvas;

        //Add all types of menus here
        [SerializeField]
        private GameObject levelUpMenu;
        [SerializeField]
        private GameObject buildingUpgradeMenu;
        [SerializeField]
        private GameObject inventoryMenu;
        [SerializeField]
        private GameObject seedListMenu;
        [SerializeField]
        private GameObject navigationBar;
        [SerializeField]
        private GameObject fieldProgressPopup;
        [SerializeField]
        private GameObject fieldUpgradePopup;
        [SerializeField]
        private GameObject buildingMenu;

        private Stack<GameObject> openMenusStack = new Stack<GameObject>();

        private void Awake()
        {
            if (mainCanvas != null)
            {
                mainCanvas.SetActive(true);
            }
        }

        private void Start()
        {
            levelUpMenu.SetActive(false);
            buildingUpgradeMenu.SetActive(false);
            inventoryMenu.SetActive(false);
            seedListMenu.SetActive(false);
            //navigationBar.SetActive(false);
            fieldProgressPopup.SetActive(false);
            fieldUpgradePopup.SetActive(false);
            buildingMenu.SetActive(false);
        }

        #region Stack Stuff

        public void DisplayMenu(MenuNames menuName, MenuOpeningType openingType, int fieldID, int sourceID)
        {
            DisplayMenu(menuName, openingType);
        }

        public void DisplayMenu(MenuNames menuName, MenuOpeningType openingType)
        {
            switch (openingType)
            {
                case MenuOpeningType.CloseAll:
                    DisableAllItemsInStack();
                    break;
                case MenuOpeningType.OnTop:
                    break;
                default:
                    break;
            }

            switch (menuName)
            {
                case MenuNames.LevelUp:
                    AddMenuInStack(levelUpMenu);
                    break;
                case MenuNames.BuldingUpgrade:
                    AddMenuInStack(buildingUpgradeMenu);
                    break;
                case MenuNames.Inventory:
                    AddMenuInStack(inventoryMenu);
                    break;
                case MenuNames.SeedList:
                    AddMenuInStack(seedListMenu);
                    break;
                case MenuNames.Navigation:
                    AddMenuInStack(navigationBar);
                    break;
                case MenuNames.FieldProgress:
                    AddMenuInStack(fieldProgressPopup);
                    break;
                case MenuNames.FieldUpgrade:
                    AddMenuInStack(fieldUpgradePopup);
                    break;
                case MenuNames.BuildingMenu:
                    AddMenuInStack(buildingMenu);
                    break;
                default:
                    break;
            }
        }

        public void OnEmptyClicked() // This is called when empty land is clicked
        {
            DisableAllItemsInStack();
            FieldManager.Instance.DeselectField();
        }

        private void AddMenuInStack(GameObject menu)
        {
            if (openMenusStack.Contains(menu))
            {
                return;
            }
            menu.SetActive(true);
            openMenusStack.Push(menu);
            ShowAllStackItems();
        }

        public void RemoveTopMenuFromStack()
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

        private void DisableAllItemsInStack()
        {
            while (openMenusStack.Count > 0)
            {
                openMenusStack.Peek().SetActive(false);
                openMenusStack.Pop();
            }
        }

        private void ShowAllStackItems()
        {
            foreach (var item in openMenusStack)
            {
                if (GEM.ShowDebugInfo) print(item.name + " count:" + openMenusStack.Count);
            }
        }

        #endregion

        #region Remove this not needed
        public void LevelUpMenuSetActive(bool flag)
        {
            levelUpMenu.SetActive(flag);
        }

        public void FieldUpgradeMenuSetActive(bool flag)
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
        #endregion

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
                    OnEmptyClicked();
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

        public void CloseMenu()
        {
            RemoveTopMenuFromStack();
        }

        public void CloseAllMenu()
        {
            DisableAllItemsInStack();
        }

        public void OpenMenu(GameObject go)
        {
            go.SetActive(true);
        }
        #endregion
    }
}