using UnityEngine;
using UnityEngine.UI;

namespace ControllerModule.Controllers
{
    /// <summary>
    /// Controller that manages how the player behaves
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : Controller
    {
        #region Cursor 

        [Header("Cursor")]
        [SerializeField, Tooltip("Determines the sprite to use when an interaction is possible")]
        private Sprite interactCursor;

        [SerializeField, Tooltip("Determines the sprite to use when no interaction is available")]
        private Sprite normalCursor;

        [SerializeField, Tooltip("Image that represents the cursor")]
        private Image cursor;

        [SerializeField, Min(0), Tooltip("Determines how far the player can interact with things")]
        private float interactRange;

        /// <summary>
        /// Updates the cursor depending on the possible interactions
        /// </summary>
        private void UpdateCursor() 
        {
            // If cursor invalid, skip
            if (this.cursor is null)
                return;

            // If eyes invalid, skip
            if (this.Eyes is null)
                return;

            if (Interfaces.IInteractable.CanInteract(this.Eyes.position, this.Eyes.forward, this.interactRange))
            {
                this.cursor.sprite = this.interactCursor;
                this.cursor.rectTransform.sizeDelta = new Vector2(50, 50);
            }
            else
            {
                this.cursor.sprite = this.normalCursor;
                this.cursor.rectTransform.sizeDelta = new Vector2(10, 10);
            }
        }

        /// <summary>
        /// Sets the cursor's visibility to the given visibility
        /// </summary>
        /// <param name="visible">Will the cursor be visible or not?</param>
        private void SetCursor(bool visible)
        {
            if (this.cursor == null)
                return;

            this.cursor.enabled = visible;
        }

        #endregion

        #region Move

        [Header("Move")]
        [SerializeField, Tooltip("Determines how fast the player can move")]
        private float movementSpeed = 20;

        private CharacterController _characterController;

        private Vector3 direction;

        /// <summary>
        /// Updates the movement of the player
        /// </summary>
        /// <param name="facing">Direction of the movement</param>
        /// <param name="speed">Speed of the movement</param>
        /// <param name="elapsed">Time passed since the last frame</param>
        private void UpdateMove(Vector3 facing, float speed, float elapsed)
        {
            // Skip if invalid movement
            if (this.Eyes is null || this._characterController is null)
                return;

            Vector3 moveDir = (this.Eyes.forward * facing.y) + (this.Eyes.right * facing.x);
            
            // Modify the direction
            moveDir.y = 0;
            moveDir = moveDir.normalized * speed;
            
            // Move the character controller
            this._characterController.Move(moveDir * elapsed);
        }

        #endregion

        #region Gravity

        [Header("Gravity")]
        [SerializeField, Tooltip("Position where the player's feet are")]
        private Transform Feet;

        [SerializeField, Tooltip("Layers considered to be ground")]
        private LayerMask GroundLayers;

        [SerializeField, Tooltip("Radius of the check for the ground")]
        private float GroundCheckRadius = 0.2f;

        private Vector3 velocity;
        private bool isGrounded;

        /// <summary>
        /// Updates the gravity of the player
        /// </summary>
        /// <param name="elapsed">Time passed since the last frame</param>
        private void UpdateGravity(float elapsed)
        {
            if (this.Feet is null || this._characterController is null)
                return;

            this.isGrounded = Physics.CheckSphere(
                this.Feet.position, 
                this.GroundCheckRadius, 
                this.GroundLayers
            );
            
            if (this.isGrounded && this.velocity.y < 0)
                this.velocity.y = 0;
                
            this.velocity += Physics.gravity * elapsed;
            
            this._characterController.Move(this.velocity * elapsed);
        }

        #endregion

        #region Controller

        /// <inheritdoc/>
        protected override void OnStart()
        {
            // Get components
            this._characterController = this.GetComponent<CharacterController>();

            // Start with this controller
            ControllerManager.SwitchTo(this);
        }

        /// <inheritdoc/>
        protected override void OnUpdate(float elapsed)
        {
            this.UpdateCursor();
            this.UpdateMove(this.direction, this.movementSpeed, elapsed);
            this.UpdateGravity(elapsed);
        }

        /// <inheritdoc/>
        public override void OnMove(Vector2 dir) => this.direction = dir;

        /// <inheritdoc/>
        public override void OnFireStart() 
        {
            // If eyes invalid, skip
            if (this.Eyes == null)
                return;

            Interfaces.IInteractable.TryInteract(this.Eyes.position, this.Eyes.forward, this.interactRange);
        }

        /// <inheritdoc/>
        protected override void OnSwitchIn()
        {
            // Update cursor
            this.SetCursor(true);
            SetCursorLock(true);

            // Reset direction
            this.direction = Vector2.zero;
        }

        /// <inheritdoc/>
        protected override void OnSwitchOut()
        {
            // Update cursor
            this.SetCursor(false);
            SetCursorLock(false);
        }

        #endregion

        #region MonoBehaviour

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (this.Eyes is not null)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(this.Eyes.position, this.Eyes.position + (this.Eyes.forward * this.interactRange));
            }

            if (this.Feet is not null)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(this.Feet.position, this.GroundCheckRadius);
            }
        }
#endif

        #endregion
    }
}