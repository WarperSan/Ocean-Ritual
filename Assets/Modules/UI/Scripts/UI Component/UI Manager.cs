using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIModule
{
    /// <summary>
    /// 
    /// </summary>
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

        public static void Toggle<T>() where T : UIMenu
        {
            foreach (UIMenu menu in registeredMenus)
            {
                if (menu is not T)
                    continue;

                Instance.StartCoroutine(Instance.ToggleMenu(menu));
            }
        }

        private IEnumerator ToggleMenu(UIMenu menu)
        {
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

