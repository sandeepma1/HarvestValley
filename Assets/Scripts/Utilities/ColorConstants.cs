using UnityEngine;
using HarvestValley.Utilities;

namespace HarvestValley.Ui
{
    [CreateAssetMenu(fileName = "ColorConstants", menuName = "HarvestValley/Singletons/ColorConstants")]
    public class ColorConstants : ScriptableSingleton<ColorConstants>
    {
#if UNITY_EDITOR
        [UnityEditor.MenuItem("HarvestValley/ColorConstants")]
        public static void ShowInEditor()
        {
            UnityEditor.Selection.activeObject = Instance;
        }
#endif
        #region World space
        // basic Colors
        [SerializeField]
        private Color white = new Color(1, 1, 1);
        public static Color White { get { return Instance.white; } }
        #endregion

        //Feild
        [SerializeField]
        private Color fieldGlow = new Color(0.75f, 1, 0);
        public static Color FieldGlow { get { return Instance.fieldGlow; } }

        [SerializeField]
        private Color fieldNormal = new Color(1, 1, 1);
        public static Color FieldNormal { get { return Instance.fieldNormal; } }

        #region Menu UI
        //Menu UI
        [SerializeField]
        private Color dehighlightedUiItem = new Color(0.25f, 0.25f, 0.25f);
        public static Color DehighlightedUiItem { get { return Instance.dehighlightedUiItem; } }

        [SerializeField]
        private Color normalUiItem = new Color(1, 1, 1);
        public static Color NormalUiItem { get { return Instance.normalUiItem; } }
        #endregion
    }
}