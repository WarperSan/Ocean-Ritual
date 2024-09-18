using System.Collections;
using UnityEngine;

namespace UIModule
{
    /// <summary>
    /// Class that represents a menu
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public abstract class UIMenu : MonoBehaviour
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
        private void OnDestroy()
        {
            UIManager.Unregister(this);
        }
    }
}

