using ControllerModule.Interfaces.Player;
using ExtensionsModule;
using System.Collections.Generic;
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

        private readonly List<Rigidbody> aboardRbs = new();

        /// <summary>
        /// Updates the position of all the items aboard
        /// </summary>
        private void UpdateAboardPosition(Vector3 movement)
        {
            foreach (Rigidbody item in this.aboardRbs)
            {
                if (item == null)
                    continue;

                item.velocity = Vector3.zero;
                item.MovePosition(item.position + movement);
            }

        }

        /// <summary>
        /// Updates the rotation of all the items aboard
        /// </summary>
        private void UpdateAboardRotation(Quaternion rotationItem, Vector3 rotationCC)
        {
            foreach (Rigidbody item in this.aboardRbs)
            {
                if (item == null)
                    continue;
                item.velocity = Vector3.zero;

                item.MoveRotation(item.rotation * rotationItem);
            }
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
            if (this.direction.x == 0)
                return;

            var eulerAngleVelocity = new Vector3();

            //D�signe le sense de la rotation et la vitesse de rotation 
            if (this.direction.x > 0)
            {
                eulerAngleVelocity = new Vector3(0, _stats.GetHandling(), 0);
            }
            else if (this.direction.x < 0)
            {
                eulerAngleVelocity = new Vector3(0, -_stats.GetHandling(), 0);
            }

            // Rotation RB
            var deltaRotation = Quaternion.Euler(eulerAngleVelocity * elapsed);
            _rb.MoveRotation(_rb.rotation * deltaRotation);
        }

        #endregion

        #region Move

        [Header("Move")]
        [SerializeField, Min(0), Tooltip("Determines how fast the boat speeds up")]
        private float movementAcceleration = 0.01f;

        [SerializeField, Min(0), Tooltip("Determines how fast the boat slows down")]
        private float movementDeceleration = 0.01f;

        [SerializeField, Tooltip("Determines the offset of the boat from the wave height")]
        private float waveOffset = 0;

        private Vector3 targetPosition;
        private float currentSpeed;

        //For player movememnt correction
        private Vector3 movement;
        public Vector3 MovementBoat => movement;

        /// <summary>
        /// Updates the movement of the boat
        /// </summary>
        /// <param name="elapsed">Time passed since the last frame</param>
        private void UpdateMove(float elapsed)
        {

            float speed = GetSpeedMultiplier(this.direction) * _stats.GetSpeed();

            // Lerp the current speed to the wanted speed
            this.currentSpeed = this.currentSpeed < speed
                ? Mathf.Clamp(this.currentSpeed + this.movementAcceleration, float.MinValue, speed)
                : Mathf.Clamp(this.currentSpeed - this.movementDeceleration, speed, float.MaxValue);


            // Updates the wanted position
            this.targetPosition = this.transform.position + (this.transform.forward * this.currentSpeed);
            this.targetPosition.y = this.waveOffset; //Singletons.OceanManager.GetHeight(this.targetPosition, this.waveOffset);

            // Lerps to the position
            Vector3 newPosition = this.transform.position.LerpAll(this.targetPosition, elapsed);
            newPosition.y = this.transform.position.y;

            _rb.MovePosition(newPosition);

            // Update positions
            Vector3 diff = newPosition - this.transform.position;
            movement = diff;

            // Update Aboard
            //this.UpdateAboardPosition(diff);
        }

        public void ShutdownBoatAcceleration() => this.direction = Vector2.zero;

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

        private void LateUpdate()
        {
            movement = Vector3.zero;
        }

        /// <inheritdoc/>
        protected override void OnFixedUpdate(float elapsed)
        {
            this.UpdateMove(elapsed);
            this.UpdateTurn(elapsed);
        }

        /// <inheritdoc/>
        protected override void OnSwitchIn()
        {
            // Update cursor
            SetCursorLock(true);
            movementDeceleration *= 2;
        }

        /// <inheritdoc/>
        protected override void OnSwitchOut()
        {
            // Update cursor
            SetCursorLock(false);
            movementDeceleration /= 2;
            this.ShutdownBoatAcceleration();
        }

        #endregion

        #region MonoBehaviour

        /// <inheritdoc/>
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;

            if (other.gameObject.TryGetComponent(out Rigidbody rb))
            {
                other.transform.SetParent(this.aboardParent != null ? this.aboardParent : this.transform);
                this.aboardRbs.Add(rb);
                return;
            }
        }

        /// <inheritdoc/>
        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;

            if (other.gameObject.TryGetComponent(out Rigidbody rb))
            {
                other.transform.SetParent(null);
                this.aboardRbs.Remove(rb);
                return;
            }
        }

        #endregion

        #region IMovable

        /// <inheritdoc/>
        public void OnMove(Vector2 direction) => this.direction = direction;

        #endregion

        
    }
}