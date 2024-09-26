using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIModule
{

    [RequireComponent(typeof(Canvas))]
    public class UIManager : UtilsModule.Singleton<UIManager>
    {
        #region Singleton

        /// <inheritdoc/>
        protected override bool DestroyOnLoad => true;

        /// <inheritdoc/>
        protected override void OnAwake()
        {
            UIMenu[] menus = FindObjectsByType<UIMenu>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            foreach (UIMenu item in menus)
                Register(item);
        }

        #endregion

        #region Toggle

        private static Stack<UIMenu> openedMenus = new();
        private static readonly Queue<System.Guid> toggleHistory = new();

        public static void Toggle<T>() where T : UIMenu
        {
            foreach (UIMenu menu in registeredMenus)
            {
                if (menu is not T)
                    continue;

                Instance.StartCoroutine(Instance.ToggleMenu(menu, System.Guid.NewGuid()));
            }
        }

        private IEnumerator ToggleMenu(UIMenu menu, System.Guid token)
        {
            // Add to the queue
            toggleHistory.Enqueue(token);

            // Wait for your turn
            while (toggleHistory.TryPeek(out System.Guid nextToken) && nextToken != token)
                yield return null;

            openedMenus.TryPeek(out UIMenu openedMenu);

            // If a menu is opened
            if (openedMenu != null)
            {
                yield return openedMenu.Close();
                openedMenus.Pop();
            }

            // If different menu
            if (openedMenu != menu)
            {
                yield return menu.Open();
                openedMenus.Push(menu);
            }

            // Consume your token
            toggleHistory.Dequeue();
        }

        #endregion

        #region Register

        private static List<UIMenu> registeredMenus = new();

        public static void Register(UIMenu menu)
        {
            registeredMenus.Add(menu);
        }

        public static void Unregister(UIMenu menu)
        {
            registeredMenus.Remove(menu);
        }

        #endregion
    }
}

