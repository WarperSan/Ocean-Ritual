using UnityEngine;
using UIModule.Interfaces;

namespace UIModule
{
    /// <summary>
    /// Class that defines all the UI components
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public abstract class UIComponent : MonoBehaviour, IElementable
    {
        private RectTransform _rect;

        /// <inheritdoc/>
        public RectTransform Rect
        {
            get
            {
                if (this._rect != null)
                    return this._rect;

                this._rect = this.GetComponent<RectTransform>();
                return this._rect;
            }
        }
    }
}