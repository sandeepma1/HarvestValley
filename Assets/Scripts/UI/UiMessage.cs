using UnityEngine;
using TMPro;

namespace HarvestValley.Ui
{
    public class UiMessage : Singleton<UiMessage>
    {
        [SerializeField]
        private GameObject topMessageGO;
        [SerializeField]
        private TextMeshProUGUI topMessageText;

        public void SetTopMessage(string message = "")
        {
            topMessageGO.SetActive(true);
            topMessageText.text = message;
        }

        public void HideClearTopMessageText()
        {
            topMessageText.text = "";
            topMessageGO.SetActive(false);
        }
    }
}
