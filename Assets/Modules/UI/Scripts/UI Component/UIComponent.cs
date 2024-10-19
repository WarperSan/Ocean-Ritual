using UnityEngine;
using UIModule.Interfaces;
using ControllerModule;
using ControllerModule.Controllers;

namespace UIModule
{
    /// <summary>
    /// Class that defines all the UI components
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public abstract class UIComponent : MonoBehaviour, IElementable, IActionable
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

        #region MonoBehaviour

        /// <inheritdoc/>
        private void OnEnable() => InputMaster.Instance += this;

        /// <inheritdoc/>
        private void OnDisable() => InputMaster.Instance -= this;

        #endregion
    }
}