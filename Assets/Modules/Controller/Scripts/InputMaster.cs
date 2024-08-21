using UnityEngine;
using UnityEngine.InputSystem;

namespace ControllerModule.Controllers
{
    /// <summary>
    /// Class that manages the inputs of the player
    /// </summary>
    public class InputMaster : UtilsModule.Singleton<InputMaster>
    {
        #region Delegate

        public delegate void LookEvent(Vector2 direction);
        public delegate void MoveEvent(Vector2 direction);
        public delegate void FireEvent();
        public delegate void PauseEvent();
        public delegate void JumpEvent();

        public delegate void UnmountEvent();

        #endregion

        #region Events

        public event LookEvent OnLook;
        public event MoveEvent OnMove;
        public event FireEvent OnFireStart;
        public event FireEvent OnFireEnd;
        public event PauseEvent OnPause;
        public event JumpEvent OnJump;
        public event UnmountEvent OnUnmount;

        #endregion

        #region Callback

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

        public void Pause(InputAction.CallbackContext context)
        {
            if (context.started)
                this.OnPause?.Invoke();
        }

        public void Jump(InputAction.CallbackContext context)
        {
            if (context.started)
                this.OnJump?.Invoke();
        }

        public void Unmount(InputAction.CallbackContext context)
        {
            if (context.started)
                ControllerManager.BackTo();
        }

        #endregion

        #region Operation

        public static InputMaster operator +(InputMaster input, Controller controller)
        {
            // Subscribe all events
            input.OnLook += controller.OnLook;
            input.OnMove += controller.OnMove;
            input.OnFireStart += controller.OnFireStart;
            input.OnFireEnd += controller.OnFireEnd;
            input.OnPause += controller.Pause;
            input.OnJump += controller.OnJump;
            
            return input;
        }

        public static InputMaster operator -(InputMaster input, Controller controller)
        {
            // Unsubscribe all events
            input.OnLook -= controller.OnLook;
            input.OnMove -= controller.OnMove;
            input.OnFireStart -= controller.OnFireStart;
            input.OnFireEnd -= controller.OnFireEnd;
            input.OnPause -= controller.Pause;
            input.OnJump -= controller.OnJump;
            
            return input;
        }

        #endregion
    }
}