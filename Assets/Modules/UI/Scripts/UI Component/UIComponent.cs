using UnityEngine;

namespace UIModule
{
    /// <summary>
    /// Class that defines all the UI components
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public abstract class UIComponent : MonoBehaviour, IElementable
    {
        /// <inheritdoc/>
        public RectTransform Rect { get; private set; }

        private void Awake()
        {
            this.Rect = this.GetComponent<RectTransform>();
        }
    }
}