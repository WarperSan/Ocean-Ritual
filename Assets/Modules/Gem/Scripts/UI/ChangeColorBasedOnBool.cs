using BlacksmithModule;
using ExtensionsModule;
using UnityEngine;
using UnityEngine.UI;

namespace GemModule.UI
{
    public class ChangeColorBasedOnBool : MonoBehaviour
    {
        public bool isActive;  // Le bool�en qui va d�terminer la couleur

        public bool start = true;

        #region Fields

        [Header("Fields")]
        [SerializeField]
        private Graphic select;

        [SerializeField]
        private Graphic background;

        #endregion

        private void UpdateColor()
        {
            if (this.isActive)
            {
                select.SetAlpha(1);
            }
            else 
            {
                select.SetAlpha(0);
            }
        }

        public void SwapState()
        {
            if (TestBlackSmith.Instance.canAddNewCase || start || isActive)
            {
                start = false;
                isActive = !isActive;
                UpdateColor();
            }
        }

        public void SetState(bool isActive, Color activeColor, Color backgroundColor)
        {
            this.isActive = isActive;
            this.select.color = activeColor;
            this.background.color = backgroundColor;
            this.UpdateColor();
        }
    }
}