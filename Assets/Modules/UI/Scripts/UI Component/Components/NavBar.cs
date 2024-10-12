using UnityEngine;

namespace UIModule.Components
{
    /// <summary>
    /// Component that defines and manages a nav bar
    /// </summary>
    public class NavBar : UIComponent
    {
        private NavTab currentTab = null;

        /// <summary>
        /// Selects the given tab
        /// </summary>
        public void Select(NavTab tab)
        {
            // If opened tab is selected tab, skip
            if (this.currentTab == tab)
                return;

            // Close the opened tab
            if (this.currentTab != null)
                this.currentTab.Close();

            // Open selected tab
            this.currentTab = tab;
            this.currentTab.Open();
        }

        #region MonoBehaviour

        /// <inheritdoc/>
        private void Start()
        {
            NavTab[] tabs = this.GetComponentsInChildren<NavTab>();

            if (tabs.Length == 0)
            {
                this.enabled = false;
                Debug.LogWarning($"Tried to make a '{nameof(NavBar)}' that contains no '{nameof(NavTab)}'.");
                return;
            }

            // Close all the tabs
            for (int i = 1; i < tabs.Length; i++)
                tabs[i].Close();

            // Open the first tab
            this.Select(tabs[0]);
        }

        #endregion
    }
}