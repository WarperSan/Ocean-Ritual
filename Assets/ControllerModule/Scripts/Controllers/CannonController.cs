using ExtensionsModule;
using ProjectilesModule.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace ControllerModule.Controllers
{
    /// <summary>
    /// Controller that manages how the cannon behaves
    /// </summary>
    public class CannonController : Controller
    {
        #region Rotation

        [Header("Rotation")]
        [SerializeField, Tooltip("Determines how fast the cannon can turn on each axis")]
        private Vector2 turningSpeed;

        [SerializeField, Tooltip("Determines how far the cannon can turn on each axis")]
        private Vector3 maxAnglesSelf;

        [SerializeField, Tooltip("Determines the axis on which the cannon clamps it's rotation")]
        private Vector3 clampAxisSelf;

        private Vector3 defaultRotation;
        private Vector3 angles;
        private Vector2 direction;

        public LookController handlesLC;
        public LookController cannonLC;

        /// <summary>
        /// Updates the rotation of the cannon
        /// </summary>
        /// <param name="direction">Direction of the rotation</param>
        private void UpdateRotation(Vector3 direction)
        {
            direction = Vector3.Scale(direction, this.turningSpeed);

            this.handlesLC.UpdateRotation(new Vector2(direction.y, 0), Time.deltaTime);
            this.cannonLC.UpdateRotation(new Vector2(-direction.x, 0), Time.deltaTime);
        }

        #endregion

        #region Shoot

        [Header("Cannon Shoot")]
        [SerializeField, Tooltip("Prefab that the cannon will shoot")]
        private GameObject ball;

        [SerializeField, Tooltip("Determines the origin and the direction of the shot")]
        private Transform origin;

        [SerializeField, Min(0), Tooltip("Determines how much force is put on the projectile upon launch")]
        private float strength;

        [SerializeField, Min(0), Tooltip("Determines the base force of the projectile, no matter the thrust applied")]
        private float baseStrength;

        [SerializeField]
        private ParticleSystem shootParticles;

        [SerializeField]
        private AudioSource shootAudio;

        /// <summary>
        /// Shoots a shot
        /// </summary>
        private void Shoot(float forcePercent)
        {
            GameObject projectile = Instantiate(this.ball);

            if (projectile == null)
                return;

            // Place projectile
            projectile.transform.position = this.origin.position;
            projectile.transform.up = this.origin.forward;

            if (projectile.TryGetComponent(out IDespawnable despawnable))
            {
                despawnable.ResetSelf();
            }

            if (projectile.TryGetComponent(out Rigidbody rb))
            {
                // Apply initial velocity
                rb.velocity = Vector3.zero;
                rb.AddForce(projectile.transform.up * ((this.strength * forcePercent) + this.baseStrength));
            }

            if (projectile.TryGetComponent(out ProjectilesModule.BeachBall beachBall))
            {
                beachBall.force = forcePercent;
            }

            if (this.shootParticles != null)
                this.shootParticles.Play();

            if (this.shootAudio != null)
                this.shootAudio.Play();
        }

        #endregion

        #region Thrust

        [Header("Thrust")]
        [SerializeField, Tooltip("Object to activate to show the thrust meter")]
        private GameObject thrustCanvas;

        [SerializeField, Tooltip("Image that shows the progress of the meter")]
        private Image thrustIcon;

        [SerializeField, Tooltip("Gradient that determines the colors to use")]
        private Gradient thrustColor;

        private float thrustAmount = 0;
        private float thrustMultiplier = 1;
        private bool releaseForShoot = false;

        /// <summary>
        /// Updates the thurst amount
        /// </summary>
        /// <param name="elapsed">Time passed since the last frame</param>
        private void UpdateThrust(float elapsed)
        {
            this.thrustAmount += elapsed * this.thrustMultiplier;

            if (this.thrustAmount >= 1)
            {
                this.thrustAmount = 1 - (this.thrustAmount - 1);
                this.thrustMultiplier = -1;
            }
            else if (this.thrustAmount <= 0)
            {
                this.thrustAmount = -this.thrustAmount;
                this.thrustMultiplier = 1;
            }

            // If icon invalid, skip
            if (this.thrustIcon == null)
                return;

            // Set the fill and the color
            this.thrustIcon.fillAmount = this.thrustAmount;
            this.thrustIcon.color = this.thrustColor.Evaluate(this.thrustAmount);
        }

        /// <summary>
        /// Enables the thrust metter
        /// </summary>
        private void StartThrust()
        {
            this.releaseForShoot = true;
            this.thrustAmount = 0;
            this.thrustMultiplier = 1;
            this.UpdateThrust(0);

            if (this.thrustCanvas != null)
                this.thrustCanvas.SetActive(true);
        }

        /// <summary>
        /// Disables the thurst metetr
        /// </summary>
        private void EndThrust()
        {
            this.releaseForShoot = false;
            if (this.thrustCanvas != null)
                this.thrustCanvas.SetActive(false);
        }

        #endregion

        #region Controller

        /// <inheritdoc/>
        public override void OnMove(Vector2 direction) => this.direction = direction;

        /// <inheritdoc/>
        public override void OnFireStart() => this.StartThrust();

        /// <inheritdoc/>
        public override void OnFireEnd()
        {
            if (this.releaseForShoot)
                this.Shoot(this.thrustAmount);
            this.EndThrust();
        }

        /// <inheritdoc/>
        protected override void OnSwitchIn()
        {
            SetCursorLock(true);

            // Reset the direction
            this.direction = Vector3.zero;

            // Updates the cannon's rotation
            this.defaultRotation = this.transform.eulerAngles;
            this.UpdateRotation(Vector3.zero);
        }

        /// <inheritdoc/>
        protected override void OnSwitchOut()
        {
            SetCursorLock(false);

            this.EndThrust();

            // Put back the cannon at it's default rotation
            this.transform.eulerAngles = this.defaultRotation;
        }

        /// <inheritdoc/>
        protected override void OnUpdate(float elapsed)
        {
            if (this.direction.magnitude != 0)
                this.UpdateRotation(new Vector3(-this.direction.y, this.direction.x, 0));

            this.UpdateThrust(elapsed);
        }

        #endregion
    }
}