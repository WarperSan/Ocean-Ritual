using UnityEngine;
using UnityEngine.UI;

namespace UIModule.Components
{
    /// <summary>
    /// Component that determines a tab in a nav bar
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class NavTab : UIComponent
    {
        [SerializeField, Tooltip("Content that will be toggled by this tab")]
        private GameObject Content;

        [SerializeField, Tooltip("Bar that tells the user that this tab is selected")]
        private GameObject SelectedBar;

        /// <summary>
        /// Opens this tab
        /// </summary>
        public void Open()
        {
            this.Content.SetActive(true);

            if (this.SelectedBar != null)
                this.SelectedBar.SetActive(true);
        }

        /// <summary>
        /// Closes this tab
        /// </summary>
        public void Close()
        {
            this.Content.SetActive(false);

            if (this.SelectedBar != null)
                this.SelectedBar.SetActive(false);
        }

        #region MonoBehaviour

        /// <inheritdoc/>
        private void Start()
        {
            if (this.Content == null)
            {
                this.enabled = false;
                Debug.LogError($"Please add a content for the '{nameof(NavTab)}' named '{this.name}'.");
                return;
            }

            NavBar navBar = this.GetComponentInParent<NavBar>();
            if (navBar == null)
            {
                this.enabled = false;
                Debug.LogWarning($"Tried to add a '{nameof(NavTab)}' without parenting a '{nameof(NavBar)}'.");
                return;
            }

            Button button = this.GetComponent<Button>();
            button.onClick.AddListener(() => navBar.Select(this));
        }

        #endregion
    }
}