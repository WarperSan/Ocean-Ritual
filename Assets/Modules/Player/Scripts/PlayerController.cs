using ControllerModule.Interfaces.Player;
using InteractModule;
using UnityEngine;
using UnityEngine.UI;

namespace ControllerModule.Controllers
{
    /// <summary>
    /// Controller that manages how the player behaves
    /// </summary>

    public class PlayerController : Controller, IMovable, IFirable, IJumpable
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

        [SerializeField]
        private InteractionAsset defaultInteraction;

        /// <summary>
        /// Updates the cursor depending on the possible interactions
        /// </summary>
        private void UpdateCursor()
        {
            // If cursor invalid, skip
            if (this.cursor == null)
                return;

            // If eyes invalid, skip
            if (this.Eyes == null)
                return;

            if (IInteractable.CanInteract(this.Eyes.position, this.Eyes.forward, out IInteractable interactable, this.interactRange))
            {
                InteractionAsset asset = interactable.InteractionAsset != null ? interactable.InteractionAsset : this.defaultInteraction;
                this.cursor.sprite = asset != null ? asset.icon : null;
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

        private void Interact()
        {
            // If eyes invalid, skip
            if (this.Eyes == null)
                return;

            IInteractable.TryInteract(this.Eyes.position, this.Eyes.forward, this.interactRange);
        }

        #endregion

        #region Move

        [Header("Move")]
        [SerializeField, Tooltip("Determines how fast the player can move")]
        private float movementSpeed = 20;

        //Used to move the character while on the boat
        [SerializeField]
        BoatController boatController;

        private Rigidbody _rigidbody;
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
            if (this.Eyes == null || this._rigidbody == null)
                return;

            if (facing.x == 0 && facing.y == 0 && this.CheckGrounded())
            {
                this._rigidbody.velocity = Vector3.zero;
            }

            Vector3 moveDir = (this.Eyes.forward * facing.y) + (this.Eyes.right * facing.x);

            // Modify the direction
            moveDir.y = 0;

            // Move the character controller
            moveDir *= speed * elapsed;
            moveDir += this.transform.position;

            if (boatController != null)
                moveDir += boatController.MovementBoat;

            this._rigidbody.MovePosition(moveDir);
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

        private bool isGrounded;

        private bool CheckGrounded()
        {
            if (this.Feet == null || this._rigidbody == null)
                return false;

            this.isGrounded = Physics.CheckSphere(
                this.Feet.position,
                this.GroundCheckRadius,
                this.GroundLayers,
                QueryTriggerInteraction.Ignore
            );

            return this.isGrounded;
        }

        #endregion

        #region Controller

        /// <inheritdoc/>
        protected override void OnStart()
        {
            // Get components
            this._rigidbody = this.GetComponent<Rigidbody>();

            // Start with this controller
            ControllerManager.SwitchTo(this);
        }

        /// <inheritdoc/>
        protected override void OnUpdate(float elapsed)
        {
            this.UpdateCursor();
        }

        protected override void OnFixedUpdate(float elapsed)
        {
            this.UpdateMove(this.direction, this.movementSpeed, elapsed);
        }

        /// <inheritdoc/>
        protected override void OnSwitchIn()
        {
            this.gameObject.SetActive(true);

            // Update cursor
            this.SetCursor(true);
            SetCursorLock(true);

            // Reset direction
            this.direction = Vector2.zero;
        }

        /// <inheritdoc/>
        protected override void OnSwitchOut()
        {
            this.gameObject.SetActive(false);

            // Update cursor
            this.SetCursor(false);
            SetCursorLock(false);
        }

        #endregion

        #region IMovable

        /// <inheritdoc/>
        public void OnMove(Vector2 dir) => this.direction = dir;

        #endregion

        #region IFirable

        /// <inheritdoc/>
        public void OnFireStart() => this.Interact();

        /// <inheritdoc/>
        public void OnFireEnd() { }

        #endregion 

        #region IJumpable
        [Header("Jump")]
        [SerializeField, Tooltip("Determines height of Jump")]
        private float jumpHeight = 1.0f;
        public void OnJump()
        {
            //Debug.Log("Jump");
            if (CheckGrounded())
            {
                //Debug.Log("grounded");
                _rigidbody.AddForce(new Vector3(0, jumpHeight, 0), ForceMode.Impulse);

            }
        }
        #endregion

        #region MonoBehaviour

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (this.Eyes != null)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(this.Eyes.position, this.Eyes.position + (this.Eyes.forward * this.interactRange));
            }

            if (this.Feet != null)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(this.Feet.position, this.GroundCheckRadius);
            }
        }
#endif

        #endregion
    }
}