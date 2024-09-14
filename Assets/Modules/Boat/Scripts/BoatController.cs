using ControllerModule.Controllers.Interfaces;
using ExtensionsModule;
using System.Collections.Generic;
using UnityEngine;

namespace ControllerModule.Controllers
{
    public class BoatController : Controller, IMovable
    {

        [SerializeField]
        private Rigidbody _rb;
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

                item.MovePosition(item.position + movement);
            }
        }

        /// <summary>
        /// Updates the rotation of all the items aboard
        /// </summary>
        private void UpdateAboardRotation(Quaternion rotation)
        {
            foreach (Rigidbody item in this.aboardRbs)
            {
                if (item == null)
                    continue;

                
                item.MoveRotation(item.rotation * rotation);
            }
        }

        #endregion

        #region Turn

        [Header("Turn")]
        [SerializeField, Min(0), Tooltip("Determines how fast the boat can turn")]
        private float turningSpeed = 1;
        private Vector2 direction;

        [SerializeField, Min(0), Tooltip("Determines how fast the boat speeds up while turning")]
        private float turningAcceleration = 0.01f;

        [SerializeField, Min(0), Tooltip("Determines how fast the boat slows down while turning")]
        private float turningDeceleration = 0.01f;

        /// <summary>
        /// Updates the rotation of the boat
        /// </summary>
        /// <param name="elapsed">Time passed since the last frame</param>
        private void UpdateTurn(float elapsed)
        {
            // Skip if no turn
            if (this.direction.x == 0)
                return;

            var eulerAngleVelocity =new Vector3();

            
            if (this.direction.x > 0)
            {
                eulerAngleVelocity = new Vector3(0, turningSpeed, 0);
            }
            if (this.direction.x < 0)
            {
                eulerAngleVelocity = new Vector3(0, -turningSpeed, 0);
            }
            //float amount = this.direction.x * this.turningSpeed;
            //Debug.Log(amount * elapsed * Vector3.up);
            //this.transform.Rotate(amount * elapsed * Vector3.up);
            
            var deltaRotation = Quaternion.Euler(eulerAngleVelocity*  Time.fixedDeltaTime);
            
            _rb.MoveRotation(_rb.rotation * deltaRotation);
            //this.UpdateAboardRotation(deltaRotation);
        }

        #endregion

        #region Move

        [Header("Move")]
        [SerializeField, Min(0), Tooltip("Determines how fast the boat walks")]
        private float movementSpeed = 20;
        
        [SerializeField, Min(0), Tooltip("Determines how fast the boat speeds up")]
        private float movementAcceleration = 0.01f;

        [SerializeField, Min(0), Tooltip("Determines how fast the boat slows down")]
        private float movementDeceleration = 0.01f;

        [SerializeField, Tooltip("Determines the offset of the boat from the wave height")]
        private float waveOffset = 0;

        private Vector3 targetPosition;
        private float currentSpeed;

        /// <summary>
        /// Updates the movement of the boat
        /// </summary>
        /// <param name="elapsed">Time passed since the last frame</param>
        private void UpdateMove(float elapsed)
        {
            //Debug.Log(this.direction);
            float speed = GetSpeedMultiplier(this.direction) * this.movementSpeed;

            // Lerp the current speed to the wanted speed
            this.currentSpeed = this.currentSpeed < speed
                ? Mathf.Clamp(this.currentSpeed + this.movementAcceleration, float.MinValue, speed)
                : Mathf.Clamp(this.currentSpeed - this.movementDeceleration, speed, float.MaxValue);

            
            // Updates the wanted position
            this.targetPosition = this.transform.position + (this.transform.forward * this.currentSpeed);
            this.targetPosition.y = this.waveOffset; //Singletons.OceanManager.GetHeight(this.targetPosition, this.waveOffset);

            // Lerps to the position
            Vector3 newPosition = this.transform.position.LerpAll(this.targetPosition, elapsed);
            
            // Update positions
            Vector3 diff = newPosition - this.transform.position;
            //this.transform.position = newPosition;
            //_rb.AddForce(transform.forward * currentSpeed,ForceMode.Acceleration);
            _rb.MovePosition(newPosition);
            this.UpdateAboardPosition(diff);
        }

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
        protected override void OnUpdate(float elapsed)
        {
            // Only update when enabled
            if (this.IsEnabled)
            {
                //this.UpdateWheel(elapsed);
                this.UpdateTurn(elapsed);
            }
        }

        /// <inheritdoc/>
        protected override void OnFixedUpdate(float elapsed)
        {
            
            this.UpdateMove(elapsed);
            
            
        }

        /// <inheritdoc/>
        protected override void OnSwitchIn()
        {
            // Update cursor
            SetCursorLock(true);
        }

        /// <inheritdoc/>
        protected override void OnSwitchOut()
        {
            // Update cursor
            SetCursorLock(false);
        }

        #endregion
    
        #region MonoBehaviour

        /// <inheritdoc/>
        private void OnTriggerEnter(Collider other) 
        {
            if (other.gameObject.TryGetComponent(out Rigidbody rb))
            {
                this.aboardRbs.Add(rb);
                return;
            }

            if (other.gameObject.TryGetComponent(out CharacterController cc))
            {
                other.transform.SetParent(this.aboardParent != null ? this.aboardParent : this.transform);
                return;
            }
        }

        /// <inheritdoc/>
        private void OnTriggerExit(Collider other)
        {

            if (other.gameObject.TryGetComponent(out Rigidbody rb))
            {
                this.aboardRbs.Remove(rb);
                return;
            }

            if (other.gameObject.TryGetComponent(out CharacterController cc))
            {
                other.transform.SetParent(null);
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