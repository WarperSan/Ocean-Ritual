using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace FishingModule
{
    public class FishNewsEntry : MonoBehaviour
    {
        #region UI Components

        [Header("UI Components")]
        [SerializeField, Tooltip("Icon to show the new fish")]
        private Image newFishIcon;

        [SerializeField, Tooltip("Text to show the new fish")]
        private TextMeshProUGUI newFishTitle;

        [SerializeField, Tooltip("RectTransfrom to animate")]
        private RectTransform animatedRect;

        #endregion

        #region Parameters

        [Header("Parameters")]
        [SerializeField, Tooltip("Color used to show the amount. The maximum of the gradient is for 100.")]
        private Gradient amountColor;

        [SerializeField, Tooltip("Curve to follow when showing this element")]
        private AnimationCurve easingCurveShow = AnimationCurve.EaseInOut(0, 0, 1, 1);

        [SerializeField, Tooltip("How many seconds the show animation lasts")]
        private float lengthShow = 3f;

        [SerializeField, Tooltip("Curve to follow when hiding this element")]
        private AnimationCurve easingCurveHide = AnimationCurve.EaseInOut(0, 0, 1, 1);

        [SerializeField, Tooltip("How many seconds the hide animation lasts")]
        private float lengthHide = 3f;

        [SerializeField, Tooltip("How many seconds the item stays on screen")]
        private float lengthStay = 5f;

        #endregion

        #region Methods

        /// <summary>
        /// Fetches the text to show
        /// </summary>
        private static string GetText() => "You just got\nx{0}";

        public void Set(FishSO fish, uint amount)
        {
            // TITLE
            string colorTag = this.amountColor.Evaluate(amount / 100f).ToHexString();

            string text = string.Format(
                GetText(),
                $"<color=#{colorTag}>{amount}</color>"
            );
            this.newFishTitle.text = text;

            // ICON
            this.newFishIcon.sprite = fish != null ? fish.Icon : null;
        }

        #endregion

        #region Coroutines

        /// <summary>Plays the animation to show on screen</summary>
        private IEnumerator ShowAnimation(float length)
        {
            // Put off screen
            var currentPosition = new Vector3
            {
                x = this.animatedRect.sizeDelta.x,
                y = this.animatedRect.anchoredPosition.y
            };
            this.animatedRect.anchoredPosition = currentPosition;

            // Put on screen
            var targetPosition = new Vector3
            {
                x = 0,
                y = currentPosition.y
            };

            // Lerp to target
            float elapsedTime = 0;

            while (elapsedTime < length)
            {
                float evaluationAtTime = this.easingCurveShow.Evaluate(elapsedTime / length);

                this.animatedRect.anchoredPosition = Vector3.Lerp(
                    currentPosition,
                    targetPosition,
                    evaluationAtTime
                );

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Snap to target
            this.animatedRect.anchoredPosition = targetPosition;
        }

        /// <summary>Plays the animaton to hide on screen</summary>
        private IEnumerator HideAnimation(float length)
        {
            // Put on screen
            var currentPosition = new Vector3
            {
                x = 0,
                y = this.animatedRect.anchoredPosition.y
            };
            this.animatedRect.anchoredPosition = currentPosition;

            // Put off screen
            var targetPosition = new Vector3
            {
                x = this.animatedRect.sizeDelta.x,
                y = currentPosition.y
            };

            // Lerp to target
            float elapsedTime = 0;

            while (elapsedTime < length)
            {
                float evaluationAtTime = this.easingCurveHide.Evaluate(elapsedTime / length);

                this.animatedRect.anchoredPosition = Vector3.Lerp(
                    currentPosition,
                    targetPosition,
                    evaluationAtTime
                );

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Snap to target
            this.animatedRect.anchoredPosition = targetPosition;
        }

        /// <summary>Plays the animations for the life cycle of an entry</summary>
        public IEnumerator LifeCycle()
        {
            yield return this.ShowAnimation(this.lengthShow);
            yield return new WaitForSeconds(this.lengthStay);
            yield return this.HideAnimation(this.lengthHide);

            Destroy(this.gameObject);
        }

        #endregion
    }
}
