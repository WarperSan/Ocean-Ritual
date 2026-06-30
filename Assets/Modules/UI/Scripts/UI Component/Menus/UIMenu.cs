using ControllerModule.Controllers;
using System.Collections;
using UnityEngine;
using UIModule.Interfaces;
using ControllerModule.Interfaces.UI;

namespace UIModule.Menus
{
    /// <summary>
    /// Class that represents a menu
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public abstract class UIMenu : Controller, IElementable, IEscapable
    {
        /// <summary>
        /// Called to open this menu
        /// </summary>
        public virtual IEnumerator Open()
        {
            gameObject.SetActive(true);
            yield return null;
        }

        /// <summary>
        /// Called to open this menu
        /// </summary>
        public virtual IEnumerator Close()
        {
            gameObject.SetActive(false);
            yield return null;
        }

        /// <inheritdoc/>
        private void OnDestroy() => UIManager.Unregister(this);

        #region IElementable

        /// <inheritdoc/>
        public RectTransform Rect { get; private set; }

        #endregion

        #region Controller

        /// <inheritdoc/>
        protected override void OnStart() => Rect = GetComponent<RectTransform>();

        #endregion

        #region IEscapable

        /// <inheritdoc/>
        public void OnEscape() => UIManager.Close(this);

        #endregion
    }
}