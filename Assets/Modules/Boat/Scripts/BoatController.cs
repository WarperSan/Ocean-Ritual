using ControllerModule.Interfaces.Player;
using ExtensionsModule;
using System.Collections.Generic;
using UnityEditor.Rendering;
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
            {
                //_rb.angularVelocity = Vector3.zero;
                return;
            }
            

            //var eulerAngleVelocity = new Vector3();
            //Debug.Log(!(_rb.angularVelocity.sqrMagnitude > _stats.GetHandling()/50));
            //Debug.Log(_rb.angularVelocity.sqrMagnitude);
            //Debug.Log(_stats.GetHandling());
            
            
            //D�signe le sense de la rotation et la vitesse de rotation 
            if (this.direction.x > 0 && !(_rb.angularVelocity.sqrMagnitude > _stats.GetHandling()/50))
            {
                //eulerAngleVelocity = new Vector3(0, _stats.GetHandling(), 0);
                _rb.AddTorque(0, _stats.GetHandling() / 50, 0, ForceMode.Acceleration);
                
                //comparer transform.forward à la vélocité normalized

                _rb.velocity = this.transform.forward * _rb.velocity.magnitude;
            }
            else if (this.direction.x < 0 && !(_rb.angularVelocity.sqrMagnitude > _stats.GetHandling()/50))
            {
                //eulerAngleVelocity = new Vector3(0, -_stats.GetHandling(), 0);
                _rb.AddTorque(0, -_stats.GetHandling() / 50, 0, ForceMode.Acceleration);
                _rb.velocity = this.transform.forward * _rb.velocity.magnitude;
            }

           

            // Rotation RB
            //var deltaRotation = Quaternion.Euler(eulerAngleVelocity * elapsed);
            //_rb.MoveRotation(_rb.rotation * deltaRotation);

            

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
            float speed = GetSpeedMultiplier(this.direction) * _stats.GetSpeed();

            // Lerp the current speed to the wanted speed
            this.CurrentSpeed = this.CurrentSpeed < speed
                ? Mathf.Clamp(this.CurrentSpeed + this.movementAcceleration, float.MinValue, speed)
                : Mathf.Clamp(this.CurrentSpeed - this.movementDeceleration, speed, float.MaxValue);

            Debug.Log(this.CurrentSpeed);
            Debug.Log(_rb.velocity.magnitude);
            if (this.CurrentSpeed < 0 && _rb.velocity.magnitude >= 0 && _rb.velocity.magnitude < 1)
            {
                _rb.velocity = Vector3.zero;
                return;
            }

            // Updates the wanted position
            this.targetPosition = this.transform.position + (this.transform.forward * this.CurrentSpeed);
            this.targetPosition.y = this.waveOffset; //Singletons.OceanManager.GetHeight(this.targetPosition, this.waveOffset);

            // Lerps to the position
            Vector3 newPosition = this.transform.position.LerpAll(this.targetPosition, elapsed);
            newPosition.y = this.transform.position.y;

            //_rb.MovePosition(newPosition);
            _rb.AddForce(this.transform.forward * CurrentSpeed);
            _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, _stats.GetSpeed());

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

            if (other.gameObject.TryGetComponent(out CharacterController cc))
            {
                other.transform.SetParent(this.aboardParent != null ? this.aboardParent : this.transform);
                
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