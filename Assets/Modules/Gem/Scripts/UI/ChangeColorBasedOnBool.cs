using BlacksmithModule;
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
        private Image image;    // L'�l�ment UI Image � changer de couleur

        #endregion

        #region Properties

        [Header("Properties")]
        [SerializeField]
        private Color activeColor = Color.green;

        [SerializeField]
        private Color inactiveColor = Color.gray;

        #endregion

        public void UpdateColor()
        {
            if (isActive)
            {
                image.color = activeColor; // Si le bool�en est true, couleur verte
            }
            else
            {
                image.color = inactiveColor; // Sinon, couleur grise
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
    }
}