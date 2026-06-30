using ControllerModule.Interfaces.Player;
using ExtensionsModule;
using MapModule;
using UnityEngine;

namespace ControllerModule.Controllers
{
    public class BoatController : Controller, IMovable
    {
        [SerializeField]
        private Rigidbody _rb;

        #region Stats

        [Header("Stats")]
        [SerializeField]
        private BoatStats _stats;

        #endregion

        #region Aboard

        [Header("Aboard")]
        [SerializeField]
        private Transform aboardParent;

        [SerializeField]
        private Collider aboardCollider;

        private Transform player;

        private void UpdatePlayerAboard()
        {
            // If player not aboard, skip
            if (player == null)
                return;

            // If contains player, skip
            if (aboardCollider.bounds.Contains(player.transform.position))
                return;

            if (player.TryGetComponent(out CharacterController cc))
            {
                player.SetParent(null);
                cc.detectCollisions = true;
                cc.GetComponent<PlayerController>().boatController = null;
            }

            player = null;
        }

        #endregion

        #region Turn

        [Header("Turn")]
        private Vector2 direction;

        /// <summary>
        /// Updates the rotation of the boat
        /// </summary>
        /// <param name="elapsed">Time passed since the last frame</param>
        private void UpdateTurn(float elapsed)
        {
            // Skip if no turn
            if (direction.x == 0)
            {
                _rb.angularVelocity = Vector3.zero;
                return;
            }

            //D�signe le sense de la rotation et la vitesse de rotation 
            if (direction.x > 0 && !(_rb.angularVelocity.sqrMagnitude > _stats.GetHandling() / 50))
            {
                //eulerAngleVelocity = new Vector3(0, _stats.GetHandling(), 0);
                _rb.AddTorque(0,
                    _stats.GetHandling() / 50,
                    0,
                    ForceMode.Acceleration);
                //comparer transform.forward à la vélocité normalized
            }
            else if (direction.x < 0 && !(_rb.angularVelocity.sqrMagnitude > _stats.GetHandling() / 50))
            {
                //eulerAngleVelocity = new Vector3(0, -_stats.GetHandling(), 0);
                _rb.AddTorque(0,
                    -_stats.GetHandling() / 50,
                    0,
                    ForceMode.Acceleration);
            }

            _rb.linearVelocity = transform.forward * _rb.linearVelocity.magnitude;
        }

        #endregion

        #region Move

        [Header("Move")]
        [SerializeField]
        [Min(0)]
        [Tooltip("Determines how fast the boat speeds up")]
        private float movementAcceleration = 0.01f;

        [SerializeField]
        [Min(0)]
        [Tooltip("Determines how fast the boat slows down")]
        private float movementDeceleration = 0.01f;

        private Vector3 targetPosition;

        public float CurrentSpeed { get; private set; }

        //For player movememnt correction
        private Vector3 movement;
        public Vector3 MovementBoat => movement;

        /// <summary>
        /// Updates the movement of the boat
        /// </summary>
        /// <param name="elapsed">Time passed since the last frame</param>
        private void UpdateMove(float elapsed)
        {
            movement = Vector3.zero;
            float speed = GetSpeedMultiplier(direction) * _stats.GetSpeed();

            // Lerp the current speed to the wanted speed
            CurrentSpeed = CurrentSpeed < speed
                ? Mathf.Clamp(CurrentSpeed + movementAcceleration, float.MinValue, speed)
                : Mathf.Clamp(CurrentSpeed - movementDeceleration, speed, float.MaxValue);

            if (CurrentSpeed < 0 && _rb.linearVelocity.magnitude >= 0 && _rb.linearVelocity.magnitude < 1)
            {
                _rb.linearVelocity = Vector3.zero;
                return;
            }

            // Updates the wanted position
            targetPosition = transform.position + transform.forward * CurrentSpeed;
            targetPosition.y = OceanManager.WATER_HEIGHT;

            // Lerps to the position
            Vector3 newPosition = transform.position.LerpAll(targetPosition, elapsed);
            newPosition.y = transform.position.y;

            //_rb.MovePosition(newPosition);
            _rb.AddForce(transform.forward * CurrentSpeed);
            _rb.linearVelocity = Vector3.ClampMagnitude(_rb.linearVelocity, _stats.GetSpeed());

            // Update positions
            movement = _rb.linearVelocity;
        }

        public void ShutdownBoatAcceleration() => direction = Vector2.zero;

        /// <summary>
        /// Gets the speed multiplier depending of the direction of the movement
        /// </summary>
        /// <param name="direction">Direction of the movement</param>
        /// <returns>Multiplier of the speed</returns>
        private static float GetSpeedMultiplier(Vector2 direction)
        {
            // Double speed if going forwards
            if (direction.y > 0)
                return 2;

            // Half speed if going backwards
            if (direction.y < 0)
                return -0.5f;

            // Regular speed if turning
            if (direction.x != 0)
                return 0;

            // No speed if not moving
            return 0;
        }

        #endregion

        #region Controller

        /// <inheritdoc/>
        protected override void OnFixedUpdate(float elapsed)
        {
            UpdateMove(elapsed);
            UpdateTurn(elapsed);
            UpdatePlayerAboard();
        }

        /// <inheritdoc/>
        protected override void OnSwitchIn()
        {
            // Update cursor
            SetCursorLock(true);
            movementDeceleration *= 2;
            sliderLife.SetActive(true);
        }

        [SerializeField]
        private GameObject sliderLife;

        /// <inheritdoc/>
        protected override void OnSwitchOut()
        {
            // Update cursor
            SetCursorLock(false);
            movementDeceleration /= 2;
            ShutdownBoatAcceleration();
            sliderLife.SetActive(false);
        }

        #endregion

        #region MonoBehaviour

        /// <inheritdoc/>
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;

            if (other.gameObject.TryGetComponent(out CharacterController cc))
            {
                other.transform.SetParent(aboardParent ?? transform);
                cc.detectCollisions = false;
                cc.GetComponent<PlayerController>().boatController = this;

                player = other.transform;
            }
        }

        #endregion

        #region IMovable

        /// <inheritdoc/>
        public void OnMove(Vector2 direction) => this.direction = direction;

        #endregion
    }
}