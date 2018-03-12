using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace HarvestValley.Ui
{
    public class UiBuildingMenu : BuildingMenuBase<UiBuildingMenu>
    {
        [SerializeField]
        private TextMeshProUGUI buildingName;
        [SerializeField]
        private Image buildingImage;
        [SerializeField]
        private Transform itemParent;
        [SerializeField]
        private Transform queueParent;

        internal Canvas mainCanvas;

        public override void Start()
        {
            base.Start();
            mainCanvas = GetComponent<Canvas>();
        }

        public override void OnEnable()
        {
            if (FieldManager.Instance == null)
            {
                return;
            }
            base.OnEnable();

            selectedBuildingID = BuildingManager.Instance.currentSelectedBuildingID;
            selectedSourceID = BuildingManager.Instance.currentlSelectedSourceID;
        }
    }
}