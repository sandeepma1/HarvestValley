using UnityEngine;
using Lean.Touch;

namespace HarvestValley.Controls
{
    public class LeanTouchController : Singleton<LeanTouchController>
    {
        [SerializeField]
        LeanTouch leanTouch;

        public void EnableLeanTouch()
        {
            leanTouch.enabled = true;
        }

        public void DisableLeanTouch()
        {
            leanTouch.enabled = false;
        }
    }
}