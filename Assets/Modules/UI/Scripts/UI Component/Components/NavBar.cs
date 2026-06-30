using UnityEngine;

namespace UIModule.Components
{
    /// <summary>
    /// Component that defines and manages a nav bar
    /// </summary>
    public class NavBar : UIComponent
    {
        private NavTab currentTab;
        private int currentIndex;
        private NavTab[] tabs;

        /// <summary>
        /// Goes to the next tab in the list
        /// </summary>
        /// <param name="loop">Defines if the navbar loops from the start when reaching the end</param>
        public void Next(bool loop = true)
        {
            currentIndex++;

            if (loop && currentIndex >= tabs.Length)
                currentIndex = 0;

            Select(currentIndex);
        }

        /// <summary>
        /// Goes to the previous tab in the list
        /// </summary>
        /// <param name="loop">Defiens if the navbar loops from the end when reaching the start</param>
        public void Previous(bool loop = true)
        {
            currentIndex--;

            if (loop && currentIndex < 0)
                currentIndex = tabs.Length - 1;

            Select(currentIndex);
        }

        /// <summary>
        /// Selects the given tab
        /// </summary>
        public void SelectTab(NavTab tab)
        {
            // If opened tab is selected tab, skip
            if (currentTab == tab)
                return;

            int tabIndex = -1;

            for (int i = 0; i < tabs.Length; i++)
            {
                if (tabs[i] == tab)
                {
                    tabIndex = i;
                    break;
                }
            }

            // If not a child, skip
            if (tabIndex == -1)
            {
                Debug.LogWarning($"Tried to open the tab '{tab.name}' with the navbar '{name}'.");
                return;
            }

            Select(tabIndex);
        }

        public void Select(int index)
        {
            // Pre set
            if (tabs == null)
            {
                currentIndex = index;
                return;
            }

            if (index < 0 || index >= tabs.Length)
            {
                Debug.LogWarning($"Tried to select the tab at the index '{index}', but it is outside the valid range.");
                return;
            }

            // Close the opened tab
            if (currentTab != null)
                currentTab.Close();

            // Open selected tab
            currentTab = tabs[index];
            currentIndex = index;
            currentTab.Open();
        }

        #region MonoBehaviour

        /// <inheritdoc/>
        private void Start()
        {
            tabs = GetComponentsInChildren<NavTab>();

            if (tabs.Length == 0)
            {
                enabled = false;
                Debug.LogWarning($"Tried to make a '{nameof(NavBar)}' that contains no '{nameof(NavTab)}'.");
                return;
            }

            // Close all the tabs
            for (int i = 1; i < tabs.Length; i++)
                tabs[i].Close();

            // Open the first tab
            Select(currentIndex);
        }

        #endregion
    }
}