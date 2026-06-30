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
        [SerializeField]
        [Tooltip("Determines the sprite to use when an interaction is possible")]
        private Sprite interactCursor;

        [SerializeField]
        [Tooltip("Determines the sprite to use when no interaction is available")]
        private Sprite normalCursor;

        [SerializeField]
        [Tooltip("Image that represents the cursor")]
        private Image cursor;

        [SerializeField]
        [Tooltip("Text to show when the player can interact with something")]
        private TextMeshProUGUI cursorText;

        [SerializeField]
        [Min(0)]
        [Tooltip("Determines how far the player can interact with things")]
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
            if (cursor == null)
                return;

            // If eyes invalid, skip
            if (Eyes == null)
                return;

            if (IInteractable.CanInteract(Eyes.position,
                    Eyes.forward,
                    out IInteractable interactable,
                    interactRange))
            {
                InteractionAsset asset = interactable.InteractionAsset ?? defaultInteraction;
                cursor.sprite = asset?.icon;
                cursor.rectTransform.sizeDelta = new Vector2(50, 50);

                isHoveringInteractable = true;
                showKeyCoroutine ??= StartCoroutine(ShowTip());

                cursorText.text = asset.tip;
            }
            else
            {
                cursor.sprite = normalCursor;
                cursor.rectTransform.sizeDelta = new Vector2(10, 10);

                isHoveringInteractable = false;
                discardTipCoroutine ??= StartCoroutine(DiscardTip());

                cursorText.text = "";
            }

            Vector2 textPos = cursorText.rectTransform.anchoredPosition;
            textPos.y = -cursor.rectTransform.sizeDelta.y / 2;
            cursorText.rectTransform.anchoredPosition = textPos;
        }

        private Coroutine discardTipCoroutine;

        private IEnumerator DiscardTip()
        {
            yield return new WaitForSeconds(0.5f);

            if (!isHoveringInteractable)
                KeybindTip.DiscardTip(INTERACT_TIP_TAG);

            discardTipCoroutine = null;
        }

        private Coroutine showKeyCoroutine;

        private IEnumerator ShowTip()
        {
            yield return new WaitForSeconds(5);

            if (isHoveringInteractable)
                KeybindTip.ShowKey(KeyCode.E, INTERACT_TIP_TAG);

            showKeyCoroutine = null;
        }

        /// <summary>
        /// Sets the cursor's visibility to the given visibility
        /// </summary>
        /// <param name="visible">Will the cursor be visible or not?</param>
        private void SetCursor(bool visible)
        {
            if (cursor == null)
                return;

            cursor.enabled = visible;
        }

        private void Interact()
        {
            // If eyes invalid, skip
            if (Eyes == null)
                return;

            KeybindTip.UseTip(INTERACT_TIP_TAG);
            IInteractable.TryInteract(Eyes.position, Eyes.forward, interactRange);
        }

        #endregion

        #region Move

        [Header("Move")]
        [SerializeField]
        [Tooltip("Determines how fast the player can move")]
        private float movementSpeed = 20;

        //Used to move the character while on the boat

        public BoatController boatController;

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
            if (Eyes == null || _characterController == null)
                return;

            Vector3 moveDir = Eyes.forward * facing.y + Eyes.right * facing.x;

            // Modify the direction
            moveDir.y = 0;

            moveDir = moveDir.normalized * speed;

            // Add boat movement 
            if (boatController != null)
                moveDir += boatController.MovementBoat.normalized * boatController.MovementBoat.magnitude;

            // Move the character controller
            _characterController.Move(moveDir * elapsed);

            //this._rigidbody.MovePosition(moveDir);
        }

        #endregion

        #region Gravity

        [Header("Gravity")]
        [SerializeField]
        [Tooltip("Position where the player's feet are")]
        private Transform Feet;

        [SerializeField]
        [Tooltip("Layers considered to be ground")]
        private LayerMask GroundLayers;

        [SerializeField]
        [Tooltip("Radius of the check for the ground")]
        private float GroundCheckRadius = 0.2f;

        private bool isGrounded;
        private Vector3 velocity;

        /// <summary>
        /// Updates the gravity of the player
        /// </summary>
        /// <param name="elapsed">Time passed since the last frame</param>
        private void UpdateGravity(float elapsed)
        {
            if (Feet == null || _characterController == null)
                return;

            isGrounded = Physics.CheckSphere(
                Feet.position,
                GroundCheckRadius,
                GroundLayers,
                QueryTriggerInteraction.Ignore
            );

            if (isGrounded && velocity.y < 0)
                velocity.y = 0;
            velocity += Physics.gravity * elapsed;

            _characterController.Move(velocity * elapsed);
        }

        private bool CheckGrounded()
        {
            if (Feet == null || _characterController == null)
                return false;

            isGrounded = Physics.CheckSphere(
                Feet.position,
                GroundCheckRadius,
                GroundLayers,
                QueryTriggerInteraction.Ignore
            );

            return isGrounded;
        }

        #endregion

        #region Controller

        /// <inheritdoc/>
        protected override void OnStart()
        {
            // Get components
            _characterController = GetComponent<CharacterController>();

            // Start with this controller
            ControllerManager.SwitchTo(this);
        }

        /// <inheritdoc/>
        protected override void OnUpdate(float elapsed) => UpdateCursor();

        protected override void OnFixedUpdate(float elapsed)
        {
            UpdateMove(direction, movementSpeed, elapsed);
            UpdateGravity(elapsed);
        }

        /// <inheritdoc/>
        protected override void OnSwitchIn()
        {
            gameObject.SetActive(true);

            // Update cursor
            SetCursor(true);
            SetCursorLock(true);

            // Reset direction
            direction = Vector2.zero;
        }

        /// <inheritdoc/>
        protected override void OnSwitchOut()
        {
            gameObject.SetActive(false);

            // Update cursor
            SetCursor(false);
            SetCursorLock(false);
        }

        #endregion

        #region IMovable

        /// <inheritdoc/>
        public void OnMove(Vector2 dir) => direction = dir;

        #endregion

        #region IInteractionable

        /// <inheritdoc/>
        public void OnInteract() => Interact();

        #endregion

        #region IJumpable

        [Header("Jump")]
        [SerializeField]
        [Tooltip("Determines height of Jump")]
        private float jumpHeight = 1.0f;

        public void OnJump()
        {
            if (!CheckGrounded())
                return;

            //_rigidbody.AddForce(new Vector3(0, jumpHeight, 0), ForceMode.Impulse);
            velocity += new Vector3(0, jumpHeight, 0);
        }

        #endregion

        #region MonoBehaviour

        #if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (Eyes != null)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(Eyes.position, Eyes.position + Eyes.forward * interactRange);
            }

            if (Feet != null)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(Feet.position, GroundCheckRadius);
            }
        }
        #endif

        #endregion
    }
}