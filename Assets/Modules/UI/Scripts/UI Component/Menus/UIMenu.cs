using System.Collections;
using UnityEngine;

namespace UIModule.Menus
{
    /// <summary>
    /// Class that represents a menu
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public abstract class UIMenu : UIComponent
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
    }
}

