using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuManager : Singleton<MenuManager>
{
    //[SerializeField]
    // private GameObject loadingScreen;
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
    //[SerializeField]
    //private GameObject shopMenu;

    [SerializeField]
    private GameObject[] disableAllMenus;

    public Hashtable ease = new Hashtable();

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
        DisableAllMenus();
        ease.Add("ease", LeanTweenType.easeOutSine);
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

    public void DisableAllMenus()
    {
        for (int i = 0; i < disableAllMenus.Length; i++)
        {
            disableAllMenus[i].transform.position = new Vector3(-500, -500, 0);
            disableAllMenus[i].SetActive(true);
            //GEM.isDragging = false;
        }
        BuildingsManager.Instance.DisableAnyOpenMenus();
        //BuildingsManager.Instance.isFarmTimerEnabled = false;
        //UIMasterMenuManager.Instance.ToggleDisplayMenuUI(false);
        // InputController.instance.ResetCameraAfterSnap();
    }

    #region UI Button calls
    public void ChangeCameraPosition(int number)
    {
        LeanTween.moveX(Camera.main.gameObject, GEM.screensPositions[number], 0.5f, ease);
    }

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