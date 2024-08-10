using UnityEngine;

namespace ControllerModule.Controllers
{
    /// <summary>
    /// Class that provides methods to use other controller independently
    /// </summary>
    public abstract class Controller : MonoBehaviour
    {
        #region Events

        private void Subscribe()
        {
            InputMaster input = InputMaster.Instance;

            if (input is null)
                return;

            input.OnLook += this.OnLook;
            input.OnMove += this.OnMove;
            input.OnFireStart += this.OnFireStart;
            input.OnFireEnd += this.OnFireEnd;
            input.OnInteract += this.OnInteract;
            input.OnPause += this.Pause;
        }

        private void UnSubscribe()
        {
            InputMaster input = InputMaster.Instance;

            if (input is null)
                return;

            input.OnLook -= this.OnLook;
            input.OnMove -= this.OnMove;
            input.OnFireStart -= this.OnFireStart;
            input.OnFireEnd -= this.OnFireEnd;
            input.OnInteract -= this.OnInteract;
            input.OnPause -= this.Pause;
        }

        #endregion

        #region Look

        [Header("Look")]
        [SerializeField]
        private LookController lookController;

        /// <summary>
        /// Point from which the player sees the world
        /// </summary>
        protected Transform Eyes => this.lookController?.cameraAnchor;
        
        private Vector3 camDirection;

        /// <summary>
        /// Called when the player requests a rotation
        /// </summary>
        /// <param name="direction">Direction of the rotation</param>
        protected virtual void OnLook(Vector2 direction) => this.camDirection = direction;

        #endregion

        /// <summary>
        /// Called when the player moves
        /// </summary>
        protected virtual void OnMove(Vector2 dir) { }

        /// <summary>
        /// Called when the player presses the 'Interact' button
        /// </summary>
        protected virtual void OnInteract() => ControllerManager.BackTo();

        #region Switch

        [Header("Switch")]
        [SerializeField, Tooltip("Determines if, when this controller switches out, it disables itself")]
        private bool disableIfOut = true;

        /// <summary>
        /// Is this controller enabled or not?
        /// </summary>
        /// <remarks>
        /// This allows a controller to receive certain updates while being "disabled"
        /// </remarks>
        protected bool IsEnabled { get; private set; } = true;

        /// <summary>
        /// Starts using this controller
        /// </summary>
        public void SwitchIn()
        {
            // Subscribe all
            this.Subscribe();

            // Update enable states
            this.IsEnabled = true;
            this.enabled = true;

            // Call callback
            this.OnSwitchIn();
        }
        
        /// <summary>
        /// Called when this controller is started to being used
        /// </summary>
        protected virtual void OnSwitchIn() { }

        /// <summary>
        /// Stops using this controller
        /// </summary>
        public void SwitchOut()
        {
            // Unsubscribe all
            this.UnSubscribe();

            // Update enable states
            this.IsEnabled = false;
            if (this.disableIfOut)
                this.enabled = false;

            // Resets the direction
            this.camDirection = Vector3.zero;

            // Call callback
            this.OnSwitchOut();
        }

        /// <summary>
        /// Called when this controller is no longer being used
        /// </summary>
        protected virtual void OnSwitchOut() { }

        #endregion

        #region Fire

        /// <summary>
        /// Called when the player presses the 'Fire' button
        /// </summary>
        protected virtual void OnFireStart() { }

        /// <summary>
        /// Called when the player releases the 'Fire' button
        /// </summary>
        protected virtual void OnFireEnd() {}

        #endregion

        #region Pause

        private void Pause()
        {
            // PauseMenu.Pause();

            // if (PauseMenu.IsPaused())
            //     this.OnPause();
            // else
            //     this.OnResumed();
        }

        /// <summary>
        /// Called when the player paused the game
        /// </summary>
        protected virtual void OnPause() { }

        /// <summary>
        /// Called when the player resumed the game
        /// </summary>
        protected virtual void OnResumed() { }

        #endregion

        #region Icons

        [Header("Icons")]
        [SerializeField, Tooltip("All the icons that will be affected by SetIconsVisibility")]
        private GameObject[] icons;

        /// <summary>
        /// Sets the visibility of the icons to the given state
        /// </summary>
        /// <param name="isVisible">Are the icons visible or not?</param>
        protected void SetIconsVisibility(bool isVisible)
        {
            foreach (GameObject item in this.icons)
            {
                if (item == null)
                    continue;

                item.SetActive(isVisible);
            }
        }

        #endregion

        #region MonoBehaviour

        /// <inheritdoc cref="Start" />
        public void Start() => this.OnStart();

        /// <summary>
        /// Called when this controller is being started
        /// </summary>
        protected virtual void OnStart() { }

        /// <inheritdoc cref="Update" />
        private void Update()
        {
            this.OnUpdate(Time.deltaTime);
            this.lookController?.UpdateRotation(this.camDirection, Time.deltaTime);
        }

        /// <summary>
        /// Called when this controller is being updated
        /// </summary>
        /// <param name="elapsed">Time passed since the last frame</param>
        protected virtual void OnUpdate(float elapsed) { }

        /// <inheritdoc cref="FixedUpdate" />
        private void FixedUpdate() => this.OnFixedUpdate(Time.fixedDeltaTime);

        /// <summary>
        /// Called when this controller is being updated
        /// </summary>
        /// <param name="elapsed">Time passed since the last call</param>
        protected virtual void OnFixedUpdate(float elapsed) { }

        /// <inheritdoc cref="OnDestroy" />
        private void OnDestroy() => this.UnSubscribe();

        #endregion

        #region Utilities

        /// <summary>
        /// Sets the cursor lock state 
        /// </summary>
        /// <param name="isLocked">Is the cursor locked or not?</param>
        protected static void SetCursorLock(bool isLocked)
        {
            if (isLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        #endregion
    }
}