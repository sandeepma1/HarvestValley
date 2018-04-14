using UnityEngine;
using System.Collections.Generic;
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
    BuildingMenu,
    BuyResourcesMenu,
    GrassListMenu,
    FishingMenu
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
        [SerializeField]
        private GameObject buyResourcesMenu;
        [SerializeField]
        private GameObject grassListMenu;
        [SerializeField]
        private GameObject fishingMenu;

        private Stack<GameObject> openMenusStack = new Stack<GameObject>();

        protected override void Awake()
        {
            base.Awake();
            levelUpMenu.SetActive(true);
            buildingUpgradeMenu.SetActive(true);
            inventoryMenu.SetActive(true);
            seedListMenu.SetActive(true);
            fieldProgressPopup.SetActive(true);
            fieldUpgradePopup.SetActive(true);
            buildingMenu.SetActive(true);
            buyResourcesMenu.SetActive(true);
            grassListMenu.SetActive(true);
            fishingMenu.SetActive(true);
        }

        private void Start()
        {
            levelUpMenu.transform.GetChild(0).gameObject.SetActive(false);
            buildingUpgradeMenu.transform.GetChild(0).gameObject.SetActive(false);
            inventoryMenu.transform.GetChild(0).gameObject.SetActive(false);
            seedListMenu.transform.GetChild(0).gameObject.SetActive(false);
            fieldProgressPopup.transform.GetChild(0).gameObject.SetActive(false);
            fieldUpgradePopup.transform.GetChild(0).gameObject.SetActive(false);
            buildingMenu.transform.GetChild(0).gameObject.SetActive(false);
            buyResourcesMenu.transform.GetChild(0).gameObject.SetActive(false);
            grassListMenu.transform.GetChild(0).gameObject.SetActive(false);
            fishingMenu.transform.GetChild(0).gameObject.SetActive(false);
        }

        #region Stack Stuff

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
                case MenuNames.BuyResourcesMenu:
                    AddMenuInStack(buyResourcesMenu);
                    break;
                case MenuNames.GrassListMenu:
                    AddMenuInStack(grassListMenu);
                    break;
                case MenuNames.FishingMenu:
                    AddMenuInStack(fishingMenu);
                    break;
            }
        }

        public void OnEmptyClicked() // This is called when empty land is clicked
        {
            DisableAllItemsInStack();
            FieldManager.Instance.DeselectField();
            FieldManager.Instance.StopPlantingMode();
            GrassLandManager.Instance.StopPlantingMode();
        }

        private void AddMenuInStack(GameObject menu)
        {
            GameObject child = menu.transform.GetChild(0).gameObject;
            if (openMenusStack.Contains(child))
            {
                return;
            }
            child.SetActive(true);
            openMenusStack.Push(child);
            ShowAllStackItems();
        }

        public void RemoveTopMenuFromStack()
        {
            if (openMenusStack.Count > 0)
            {
                //print("popped " + openMenusStack.Peek().gameObject.name);
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
            if (openMenusStack.Count <= 0)
            {
                return;
            }
            while (openMenusStack.Count > 0)
            {
                openMenusStack.Peek().SetActive(false);
                openMenusStack.Pop();
            }
            FieldManager.Instance.StopPlantingMode();

            ShowAllStackItems();
            // print("DisableAllItemsInStack");
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
            SceneChanger.Instance.LoadScene(sceneName);
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

        public void CloseThisMenu(GameObject go)
        {
            go.SetActive(false);
        }

        public void OpenMenu(GameObject go)
        {
            go.SetActive(true);
        }
        #endregion
    }
}