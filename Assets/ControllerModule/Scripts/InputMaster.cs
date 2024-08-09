using UnityEngine;
using UnityEngine.InputSystem;

namespace ControllerModule
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
        public delegate void InteractEvent();
        public delegate void PauseEvent();

        #endregion

        #region Events

        public event LookEvent OnLook;
        public event MoveEvent OnMove;
        public event FireEvent OnFireStart;
        public event FireEvent OnFireEnd;
        public event InteractEvent OnInteract;
        public event PauseEvent OnPause;

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

        public void Interact(InputAction.CallbackContext context) => this.OnInteract?.Invoke();

        public void Pause(InputAction.CallbackContext context)
        {
            if (context.started)
                this.OnPause?.Invoke();
        }

        #endregion
    }
}