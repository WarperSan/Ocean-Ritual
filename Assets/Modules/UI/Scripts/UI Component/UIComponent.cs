using UnityEngine;
using UIModule.Interfaces;
using ControllerModule.Controllers;
using ControllerModule.Interfaces.UI;

namespace UIModule
{
    /// <summary>
    /// Class that defines all the UI components
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public abstract class UIComponent : MonoBehaviour, IElementable, IUIActionable
    {
        private RectTransform _rect;

        /// <inheritdoc/>
        public RectTransform Rect
        {
            get
            {
                if (_rect != null)
                    return _rect;

                _rect = GetComponent<RectTransform>();
                return _rect;
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