using ControllerModule.Controllers.Interfaces;
using UIModule;
using UIModule.Menus;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ControllerModule.Controllers
{
    /// <summary>
    /// Class that manages the inputs of the player
    /// </summary>
    public class InputMaster : UtilsModule.Singleton<InputMaster>
    {
        #region Delegates

        public delegate void LookEvent(Vector2 direction);
        public delegate void MoveEvent(Vector2 direction);
        public delegate void FireEvent();

        #endregion

        #region Events

        public event LookEvent OnLook;
        public event MoveEvent OnMove;
        public event FireEvent OnFireStart;
        public event FireEvent OnFireEnd;

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

        public void Inventory(InputAction.CallbackContext context)
        {
            if (context.started)
                UIManager.Toggle<InventoryMenu>();
        }
        #endregion

        #region Operations

        public static InputMaster operator +(InputMaster input, Controller controller)
        {
            // Subscribe all events
            input.OnLook += controller.OnLook;

            if (controller is IMovable movable)
                input.OnMove += movable.OnMove;

            if (controller is IFirable firable)
            {
                input.OnFireStart += firable.OnFireStart;
                input.OnFireEnd += firable.OnFireEnd;
            }

            return input;
        }

        public static InputMaster operator -(InputMaster input, Controller controller)
        {
            // Unsubscribe all events
            input.OnLook -= controller.OnLook;

            if (controller is IMovable movable)
                input.OnMove -= movable.OnMove;

            if (controller is IFirable firable)
            {
                input.OnFireStart -= firable.OnFireStart;
                input.OnFireEnd -= firable.OnFireEnd;
            }

            return input;
        }

        #endregion
    }
}