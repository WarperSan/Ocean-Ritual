using UnityEngine;

namespace UIModule.Components
{
    /// <summary>
    /// Component that defines and manages a nav bar
    /// </summary>
    public class NavBar : UIComponent
    {
        private NavTab currentTab = null;
        private int currentIndex = 0;
        private NavTab[] tabs = null;

        /// <summary>
        /// Goes to the next tab in the list
        /// </summary>
        /// <param name="loop">Defines if the navbar loops from the start when reaching the end</param>
        public void Next(bool loop = true)
        {
            this.currentIndex++;

            if (loop && this.currentIndex >= this.tabs.Length)
                this.currentIndex = 0;

            this.Select(this.currentIndex);
        }

        /// <summary>
        /// Goes to the previous tab in the list
        /// </summary>
        /// <param name="loop">Defiens if the navbar loops from the end when reaching the start</param>
        public void Previous(bool loop = true)
        {
            this.currentIndex--;

            if (loop && this.currentIndex < 0)
                this.currentIndex = this.tabs.Length - 1;

            this.Select(this.currentIndex);
        }

        /// <summary>
        /// Selects the given tab
        /// </summary>
        public void SelectTab(NavTab tab)
        {
            // If opened tab is selected tab, skip
            if (this.currentTab == tab)
                return;

            int tabIndex = -1;
            for (int i = 0; i < this.tabs.Length; i++)
            {
                if (this.tabs[i] == tab)
                {
                    tabIndex = i;
                    break;
                }
            }

            // If not a child, skip
            if (tabIndex == -1)
            {
                Debug.LogWarning($"Tried to open the tab '{tab.name}' with the navbar '{this.name}'.");
                return;
            }

            this.Select(tabIndex);
        }

        public void Select(int index)
        {
            // Pre set
            if (this.tabs == null)
            {
                this.currentIndex = index;
                return;
            }

            if (index < 0 || index >= this.tabs.Length)
            {
                Debug.LogWarning($"Tried to select the tab at the index '{index}', but it is outside the valid range.");
                return;
            }

            // Close the opened tab
            if (this.currentTab != null)
                this.currentTab.Close();

            // Open selected tab
            this.currentTab = this.tabs[index];
            this.currentIndex = index;
            this.currentTab.Open();
        }

        #region MonoBehaviour

        /// <inheritdoc/>
        private void Start()
        {
            this.tabs = this.GetComponentsInChildren<NavTab>();

            if (this.tabs.Length == 0)
            {
                this.enabled = false;
                Debug.LogWarning($"Tried to make a '{nameof(NavBar)}' that contains no '{nameof(NavTab)}'.");
                return;
            }

            // Close all the tabs
            for (int i = 1; i < this.tabs.Length; i++)
                this.tabs[i].Close();

            // Open the first tab
            this.Select(this.currentIndex);
        }

        #endregion
    }
}