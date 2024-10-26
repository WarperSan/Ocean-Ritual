using ControllerModule.Interfaces.Player;
using ExtensionsModule;
using System.Collections.Generic;
using UnityEngine;
using static EnumGeneral;

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
        [SerializeField, Min(0), Tooltip("Determines how fast the boat can turn")]
        private float turningSpeed = 1;
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
                eulerAngleVelocity = new Vector3(0, GetHandling(), 0);
            }
            else if (this.direction.x < 0)
            {
                eulerAngleVelocity = new Vector3(0, -GetHandling(), 0);
            }

            // Rotation RB
            var deltaRotation = Quaternion.Euler(eulerAngleVelocity * elapsed);
            _rb.MoveRotation(_rb.rotation * deltaRotation);
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

        //For player movememnt correction
        private Vector3 movement;
        public Vector3 MovementBoat
        {
            get
            {
                return movement;
            }
        }

        /// <summary>
        /// Updates the movement of the boat
        /// </summary>
        /// <param name="elapsed">Time passed since the last frame</param>
        private void UpdateMove(float elapsed)
        {

            float speed = GetSpeedMultiplier(this.direction) * this.GetSpeed();

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
        private void LateUpdate()
        {
            movement = Vector3.zero;
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
        protected override void OnUpdate(float elapsed)
        {
            // Only update when enabled
            if (this.IsEnabled)
            {
                //this.UpdateWheel(elapsed);

            }
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

        #region Stats

        [SerializeField] private TypeQuantity<TypeBoat> BASE_LIFE = new(TypeBoat.Life, 1f);
        [SerializeField] private TypeQuantity<TypeBoat> BASE_SPEED = new(TypeBoat.navigateSpeed, 4f);
        [SerializeField] private TypeQuantity<TypeBoat> BASE_HANDLING = new(TypeBoat.Handling, 10f);

        [SerializeField] private TypeQuantity<TypeBoat> BOOST_LIFE = new(TypeBoat.Life, 1f);
        [SerializeField] private TypeQuantity<TypeBoat> BOOST_SPEED = new(TypeBoat.navigateSpeed, 4f);
        [SerializeField] private TypeQuantity<TypeBoat> BOOST_HANDLING = new(TypeBoat.Handling, 10f);

        [SerializeField] private componentGBN ComponantGBN;
        public componentGBN componentGBN => ComponantGBN;

        public void UpdateStat()
        {
            var baseStats = new List<TypeQuantity<TypeBoat>>
            {
                BASE_LIFE,
                BASE_SPEED, BASE_HANDLING
            };

                // Cr�ation de la liste des statistiques boost�es
            var boostedStats = new List<TypeQuantity<TypeBoat>>
            {
                BOOST_LIFE, BOOST_SPEED, BOOST_HANDLING
            };

            // Mise � jour des statistiques avec les boosts
            ComponantGBN.GBNScript.UpdateStatsWithBoost(baseStats, boostedStats);
        }

        public float GetLife(bool getBoosted = true) => getBoosted ? this.BOOST_LIFE.Quantite : this.BASE_LIFE.Quantite;
        public float GetSpeed(bool getBoosted = true) => getBoosted ? this.BOOST_SPEED.Quantite : this.BASE_SPEED.Quantite;
        public float GetHandling(bool getBoosted = true) => getBoosted ? this.BOOST_HANDLING.Quantite : this.BASE_HANDLING.Quantite;

        protected override void OnStart()
        {
            this.UpdateStat();
        }
        #endregion
    }
}