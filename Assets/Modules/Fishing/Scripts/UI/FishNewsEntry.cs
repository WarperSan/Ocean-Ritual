using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace FishingModule.UI
{
    public class FishNewsEntry : MonoBehaviour
    {
        #region UI Components

        [Header("UI Components")]
        [SerializeField]
        [Tooltip("Icon to show the new fish")]
        private Image newFishIcon;

        [SerializeField]
        [Tooltip("Text to show the new fish")]
        private TextMeshProUGUI newFishTitle;

        [SerializeField]
        [Tooltip("RectTransfrom to animate")]
        private RectTransform animatedRect;

        #endregion

        #region Parameters

        [Header("Parameters")]
        [SerializeField]
        [Tooltip("Color used to show the amount. The maximum of the gradient is for 100.")]
        private Gradient amountColor;

        #endregion

        #region Methods

        /// <summary>
        /// Fetches the text to show
        /// </summary>
        private static string GetText() => "Vous avez \nre�u x{0}";

        public void Set(FishSO fish, uint amount)
        {
            // TITLE
            string colorTag = amountColor.Evaluate(amount / 100f).ToHexString();

            string text = string.Format(
                GetText(),
                $"<color=#{colorTag}>{amount}</color>"
            );
            newFishTitle.text = text;

            // ICON
            newFishIcon.sprite = fish != null ? fish.Icon : null;
        }

        #endregion
    }
}