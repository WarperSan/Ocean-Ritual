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

        #region Open

        /// <summary>
        /// Opens the first instance of the menu of the given type
        /// </summary>
        public static void Open<T>() where T : UIMenu
        {
            T menu = GetMenu<T>();

            // If not found, skip
            if (menu == null)
            {
                Debug.LogError($"Tried to open a menu of type '{nameof(T)}', but no instance of this menu is registered.");
                return;
            }

            Open(menu);
        }

        /// <summary>
        /// Opens the given instance of the menu
        /// </summary>
        public static void Open(UIMenu menu)
        {
            // If already opened, skip
            if (openedMenus.Contains(menu))
            {
                Debug.LogWarning($"Tried to open a menu of type '{menu.GetType().Name}', but an instance of this menu is already opened.");
                return;
            }

            // Open
            AddOperation(OperationType.OPEN, menu);
        }

        /// <summary>
        /// Checks if the given menu is already opened
        /// </summary>
        public static bool IsOpened(UIMenu menu) => openedMenus.Contains(menu);

        #endregion

        #region Close

        /// <summary>
        /// Closes the first instance of the menu of the given type
        /// </summary>
        public static void Close<T>() where T : UIMenu
        {
            T menu = GetMenu<T>();

            // If not found, skip
            if (menu == null)
            {
                Debug.LogError($"Tried to close a menu of type '{nameof(T)}', but no instance of this menu is registered.");
                return;
            }

            Close(menu);
        }

        /// <summary>
        /// Closes the given instance of the menu
        /// </summary>
        public static void Close(UIMenu menu)
        {
            // If not opened, skip
            if (!IsOpened(menu))
            {
                Debug.LogWarning($"Tried to close a menu of type '{menu.GetType().Name}', but no instance of this menu is opened.");
                return;
            }

            // Close menu
            AddOperation(OperationType.CLOSE, menu);
        }

        #endregion

        #region Toggle

        /// <summary>
        /// Toggles the first instance of the menu of the given type
        /// </summary>
        /// <inheritdoc cref="Toggle(UIMenu, bool)"/>
        public static void Toggle<T>(bool alwaysToggle = false) where T : UIMenu
        {
            T menu = GetMenu<T>();

            // If not found, skip
            if (menu == null)
            {
                Debug.LogError($"Tried to toggle a menu of type '{nameof(T)}', but no instance of this menu is registered.");
                return;
            }

            Toggle(menu, alwaysToggle);
        }

        /// <summary>
        /// Toggles the given instance of the menu
        /// </summary>
        /// <param name="alwaysToggle">Determines if the menu can be toggled even when it's not the current one</param>
        public static void Toggle(UIMenu menu, bool alwaysToggle)
        {
            UIMenu current = CurrentMenu;

            // Allow only when the menu is the current one
            if (!alwaysToggle && current != null && current != menu)
                return;

            // Call the appropriate method
            if (IsOpened(menu))
                Close(menu);
            else
                Open(menu);
        }

        #endregion

        #region Operation

        private static readonly Stack<UIMenu> openedMenus = new();
        private static UIMenu CurrentMenu => openedMenus.TryPeek(out UIMenu menu) ? menu : null;

        private enum OperationType
        {
            OPEN = 0x01,
            CLOSE = 0x10,
        }

        private static readonly Queue<(OperationType operation, UIMenu menu)> operationsInProcess = new();
        private Coroutine currentProcess = null;

        private static void AddOperation(OperationType operation, UIMenu menu)
        {
            operationsInProcess.Enqueue((operation, menu));

            UIManager manager = Instance;

            // If already processing, skip
            if (manager.currentProcess != null)
                return;

            // Start processing
            manager.currentProcess = manager.StartCoroutine(manager.ProcessOperations());
        }

        private IEnumerator ProcessOperations()
        {
            // Continue until no more operations
            while (operationsInProcess.Count > 0)
            {
                (OperationType operation, UIMenu menu) = operationsInProcess.Dequeue();

                switch (operation)
                {
                    // Open menu
                    case OperationType.OPEN:
                        yield return this.OpenMenu(menu);
                        openedMenus.Push(menu);
                        break;
                    // Close menu
                    case OperationType.CLOSE:
                        UIMenu cur;
                        do
                        {
                            cur = CurrentMenu;

                            // If reached end, skip
                            if (cur == null)
                                break;

                            yield return this.CloseMenu(cur);
                            openedMenus.Pop();
                        } while (cur != menu);
                        break;
                    default:
                        Debug.LogWarning($"The operation '{operation}' called by '{menu.name}' is not supported.");
                        yield return null;
                        break;
                }

                if (CurrentMenu == null)
                    InputMaster.ResumePlay();
                else
                    InputMaster.StartMenu();
            }

            this.currentProcess = null;
        }

        private IEnumerator OpenMenu(UIMenu menu)
        {
            ControllerManager.SwitchTo(menu); // Desactivate previous inputs
            yield return menu.Open(); // Wait for animation
        }

        private IEnumerator CloseMenu(UIMenu menu)
        {
            yield return menu.Close(); // Wait for animation
            ControllerManager.BackTo(); // Activate previous inputs
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