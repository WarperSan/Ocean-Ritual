using ControllerModule.Controllers;
using System.Collections;
using System.Collections.Generic;
using UIModule.Menus;
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
            // Register all menus
            UIMenu[] menus = FindObjectsByType<UIMenu>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            foreach (UIMenu item in menus)
                Register(item);
        }

        #endregion

        #region Toggle

        private static readonly Stack<UIMenu> openedMenus = new();
        private static readonly Queue<System.Guid> toggleHistory = new();

        public static void Toggle<T>() where T : UIMenu
        {
            T menu = GetMenu<T>();

            // If not found, skip
            if (menu == null)
            {
                Debug.LogError($"Tried to toggle a menu of type '{nameof(T)}', but no instance of this menu is registered.");
                return;
            }

            var token = System.Guid.NewGuid();

            Instance.StartCoroutine(Instance.ToggleMenu(menu, token));
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
                ControllerManager.UnFreeze();
            }

            // If different menu
            if (openedMenu != menu)
            {
                yield return menu.Open();
                openedMenus.Push(menu);
                ControllerManager.Freeze();
            }

            // Consume your token
            toggleHistory.Dequeue();
        }

        #endregion

        #region Register

        private static readonly HashSet<UIMenu> registeredMenus = new();

        /// <summary>
        /// Finds the first instance of the given menu
        /// </summary>
        private static T GetMenu<T>() where T : UIMenu
        {
            foreach (UIMenu menu in registeredMenus)
            {
                if (menu is T typedMenu)
                    return typedMenu;
            }

            return null;
        }

        /// <summary>
        /// Registers the given menu, making it available to be interacted with
        /// </summary>
        public static void Register(UIMenu menu) => registeredMenus.Add(menu);

        /// <summary>
        /// Unergisters the given menu, making it unavailable to be interacted with
        /// </summary>
        public static void Unregister(UIMenu menu) => registeredMenus.Remove(menu);

        #endregion
    }
}
