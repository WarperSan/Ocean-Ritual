using ControllerModule.Interfaces.Player;
using InteractModule;
using System.Collections;
using TMPro;
using UIModule.Components;
using UnityEngine;
using UnityEngine.UI;

namespace ControllerModule.Controllers
{
    /// <summary>
    /// Controller that manages how the player behaves
    /// </summary>

    public class PlayerController : Controller, IMovable, IInteractionable, IJumpable
    {
        #region Cursor

        private const string INTERACT_TIP_TAG = "PLAYER_INTERACT";

        [Header("Cursor")]
        [SerializeField, Tooltip("Determines the sprite to use when an interaction is possible")]
        private Sprite interactCursor;

        [SerializeField, Tooltip("Determines the sprite to use when no interaction is available")]
        private Sprite normalCursor;

        [SerializeField, Tooltip("Image that represents the cursor")]
        private Image cursor;

        [SerializeField, Tooltip("Text to show when the player can interact with something")]
        private TextMeshProUGUI cursorText;

        [SerializeField, Min(0), Tooltip("Determines how far the player can interact with things")]
        private float interactRange;

        [SerializeField]
        private InteractionAsset defaultInteraction;

        private bool isHoveringInteractable;

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

                this.isHoveringInteractable = true;
                this.showKeyCoroutine ??= this.StartCoroutine(this.ShowTip());

                this.cursorText.text = asset.tip;
            }
            else
            {
                this.cursor.sprite = this.normalCursor;
                this.cursor.rectTransform.sizeDelta = new Vector2(10, 10);

                this.isHoveringInteractable = false;
                this.discardTipCoroutine ??= this.StartCoroutine(this.DiscardTip());

                this.cursorText.text = "";
            }

            Vector2 textPos = this.cursorText.rectTransform.anchoredPosition;
            textPos.y = -this.cursor.rectTransform.sizeDelta.y / 2;
            this.cursorText.rectTransform.anchoredPosition = textPos;
        }

        private Coroutine discardTipCoroutine;
        private IEnumerator DiscardTip()
        {
            yield return new WaitForSeconds(0.5f);

            if (!this.isHoveringInteractable)
                KeybindTip.DiscardTip(INTERACT_TIP_TAG);

            this.discardTipCoroutine = null;
        }

        private Coroutine showKeyCoroutine;
        private IEnumerator ShowTip()
        {
            yield return new WaitForSeconds(5);

            if (this.isHoveringInteractable)
                KeybindTip.ShowKey(KeyCode.E, INTERACT_TIP_TAG);

            this.showKeyCoroutine = null;
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

            KeybindTip.UseTip(INTERACT_TIP_TAG);
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
            if (this.Eyes == null || this._characterController == null)
                return;

            Vector3 moveDir = (this.Eyes.forward * facing.y) + (this.Eyes.right * facing.x);

            // Modify the direction
            moveDir.y = 0;

            moveDir = moveDir.normalized * speed;

            // Add boat movement 
            if (boatController != null)
                moveDir += boatController.MovementBoat.normalized * boatController.MovementBoat.magnitude;

            // Move the character controller
            this._characterController.Move(moveDir * elapsed);

            //this._rigidbody.MovePosition(moveDir);
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
        private Vector3 velocity;

        /// <summary>
        /// Updates the gravity of the player
        /// </summary>
        /// <param name="elapsed">Time passed since the last frame</param>
        private void UpdateGravity(float elapsed)
        {
            if (this.Feet == null || this._characterController == null)
                return;

            this.isGrounded = Physics.CheckSphere(
                this.Feet.position,
                this.GroundCheckRadius,
                this.GroundLayers,
                QueryTriggerInteraction.Ignore
            );

            if (this.isGrounded && this.velocity.y < 0)
                this.velocity.y = 0;
            this.velocity += Physics.gravity * elapsed;

            this._characterController.Move(this.velocity * elapsed);
        }


        private bool CheckGrounded()
        {
            if (this.Feet == null || this._characterController == null)
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
            this._characterController = this.GetComponent<CharacterController>();

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
            this.UpdateGravity(elapsed);
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

        #region IInteractionable

        /// <inheritdoc/>
        public void OnInteract() => this.Interact();

        #endregion

        #region IJumpable

        [Header("Jump")]
        [SerializeField, Tooltip("Determines height of Jump")]
        private float jumpHeight = 1.0f;

        public void OnJump()
        {
            if (!this.CheckGrounded())
                return;

            //_rigidbody.AddForce(new Vector3(0, jumpHeight, 0), ForceMode.Impulse);
            velocity += new Vector3(0, jumpHeight, 0);
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