using ControllerModule.Interfaces;
using ControllerModule.Interfaces.Player;
using ControllerModule.Interfaces.UI;
using UIModule;
using UIModule.Menus;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ControllerModule.Controllers
{
    /// <summary>
    /// Class that manages the inputs of the player
    /// </summary>
    [RequireComponent(typeof(PlayerInput))]
    public class InputMaster : UtilsModule.Singleton<InputMaster>
    {
        #region Delegates

        public delegate void LookEvent(Vector2 direction);
        public delegate void MoveEvent(Vector2 direction);
        public delegate void JumpEvent();
        public delegate void FireEvent();
        public delegate void TabEvent();

        #endregion

        #region Events

        public event LookEvent OnLook;
        public event MoveEvent OnMove;
        public event FireEvent OnFireStart;
        public event FireEvent OnFireEnd;
        public event JumpEvent OnJump;
        public event TabEvent OnTabNext;
        public event TabEvent OnTabPrevious;

        #endregion

        #region States

        public bool IsShift { get; private set; }

        #endregion

        #region Callbacks

        public void Look(InputAction.CallbackContext context)
        {
            Vector2 direction = context.ReadValue<Vector2>();

            this.OnLook?.Invoke(direction);
        }

        public void Move(InputAction.CallbackContext context)
        {
            Vector2 direction = context.ReadValue<Vector2>();

            this.OnMove?.Invoke(direction);
        }

        public void Fire(InputAction.CallbackContext context)
        {
            if (context.started)
                this.OnFireStart?.Invoke();
            else if (context.canceled)
                this.OnFireEnd?.Invoke();
        }

        public void Unmount(InputAction.CallbackContext context)
        {
            if (context.started)
                ControllerManager.BackTo();
        }
        public void Jump(InputAction.CallbackContext context)
        {
            this.OnJump?.Invoke();
        }

        public void Inventory(InputAction.CallbackContext context)
        {
            if (context.started)
                UIManager.Toggle<InventoryMenu>();
        }

        public void Tab(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                if (this.IsShift)
                    this.OnTabPrevious?.Invoke();
                else
                    this.OnTabNext?.Invoke();
            }
        }

        public void Shift(InputAction.CallbackContext context)
        {
            if (context.started)
                this.IsShift = true;
            else if (context.canceled)
                this.IsShift = false;
        }

        #endregion

        #region Maps

        private InputActionMap PlayerMap;
        private InputActionMap UIMap;

        public static void ResumePlay()
        {
            Instance.UIMap.Disable();
            Instance.PlayerMap.Enable();
        }

        public static void StartMenu()
        {
            Instance.PlayerMap.Disable();
            Instance.UIMap.Enable();
        }

        #endregion

        #region Operations

        public static InputMaster operator +(InputMaster input, IActionable actionable)
        {
            if (input == null || actionable == null)
                return input;

            // Subscribe all events
            input = actionable switch
            {
                IPlayerActionable player => player + input,
                IUIActionable ui => ui + input,
                _ => input
            };

            return input;
        }

        public static InputMaster operator -(InputMaster input, IActionable actionable)
        {
            if (input == null || actionable == null)
                return input;

            // Unsubscribe all events
            input = actionable switch
            {
                IPlayerActionable player => player - input,
                IUIActionable ui => ui - input,
                _ => input
            };

            return input;
        }

        #endregion

        #region Singleton

        /// <inheritdoc/>
        protected override void OnAwake()
        {
            PlayerInput input = this.GetComponent<PlayerInput>();
            this.PlayerMap = input.actions.FindActionMap("Player");
            this.UIMap = input.actions.FindActionMap("UI");
        }

        #endregion
    }
}