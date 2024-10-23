using ControllerModule.Controllers;
using System.Collections;
using UnityEngine;
using UIModule.Interfaces;

namespace UIModule.Menus
{
    /// <summary>
    /// Class that represents a menu
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public abstract class UIMenu : Controller, IElementable
    {
        /// <summary>
        /// Called to open this menu
        /// </summary>
        public virtual IEnumerator Open()
        {
            this.gameObject.SetActive(true);
            yield return null;
        }

        /// <summary>
        /// Called to open this menu
        /// </summary>
        public virtual IEnumerator Close()
        {
            this.gameObject.SetActive(false);
            yield return null;
        }

        /// <inheritdoc/>
        private void OnDestroy()
        {
            UIManager.Unregister(this);
        }

        #region IElementable

        /// <inheritdoc/>
        public RectTransform Rect { get; private set; }

        #endregion

        #region Controller

        /// <inheritdoc/>
        protected override void OnStart()
        {
            this.Rect = this.GetComponent<RectTransform>();
        }

        #endregion
    }
}

