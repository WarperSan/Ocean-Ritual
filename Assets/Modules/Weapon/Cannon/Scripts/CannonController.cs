using ControllerModule.Controllers;
using ControllerModule.Interfaces.Player;
using EntityModule;
using UnityEngine;
using UnityEngine.UI;

namespace WeaponModule.Weapons.Cannon
{
    /// <summary>
    /// Controller that manages how the cannon behaves
    /// </summary>
    public class CannonController : WeaponController, IMovable
    {
        #region Stats

        [Header("Stats")]
        [SerializeField]
        private CannonStats _stats;

        #endregion

        #region Rotation

        [Header("Rotation")]
        [SerializeField]
        [Tooltip("Determines how fast the cannon can turn on each axis")]
        private Vector2 turningSpeed;

        [SerializeField]
        [Tooltip("Determines how far the cannon can turn on each axis")]
        private Vector3 maxAnglesSelf;

        [SerializeField]
        [Tooltip("Determines the axis on which the cannon clamps it's rotation")]
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
            direction = Vector3.Scale(direction, turningSpeed);

            handlesLC.UpdateRotation(new Vector2(direction.y, 0), Time.deltaTime);
            cannonLC.UpdateRotation(new Vector2(0, -direction.x), Time.deltaTime);
        }

        #endregion

        #region Shoot

        [Header("Cannon Shoot")]
        [SerializeField]
        [Tooltip("Determines the origin and the direction of the shot")]
        private Transform origin;

        [SerializeField]
        [Min(0)]
        [Tooltip("Determines how much force is put on the projectile upon launch")]
        private float strength;

        [SerializeField]
        [Min(0)]
        [Tooltip("Determines the base force of the projectile, no matter the thrust applied")]
        private float baseStrength;

        [SerializeField]
        private ParticleSystem shootParticles;

        [SerializeField]
        private AudioSource shootAudio;

        /// <inheritdoc/>
        protected override void SetupProjectile(Projectile projectile)
        {
            // Place projectile
            projectile.transform.position = origin.position;
            projectile.transform.up = origin.forward;

            // Set thrust
            if (projectile is CannonBall beachBall)
                beachBall.splashForce = thrustAmount;
        }

        /// <inheritdoc/>
        protected override void OnShoot(GameObject bullet)
        {
            // Apply initial velocity
            if (bullet.TryGetComponent(out Rigidbody rb))
            {
                rb.linearVelocity = Vector3.zero;
                rb.AddForce(bullet.transform.up * (strength * thrustAmount + baseStrength));
            }

            if (shootParticles != null)
                shootParticles.Play();

            if (shootAudio != null)
                shootAudio.Play();
        }

        protected override Attack GetAttack() => new()
        {
            Damage = _stats.GetDamage(),
            Type = AttackType.NORMAL,
            TargetType = ProjectileTarget.OPPONENTS,
        };

        #endregion

        #region Thrust

        [Header("Thrust")]
        [SerializeField]
        [Tooltip("Object to activate to show the thrust meter")]
        private GameObject thrustCanvas;

        [SerializeField]
        [Tooltip("Image that shows the progress of the meter")]
        private Image thrustIcon;

        [SerializeField]
        [Tooltip("Gradient that determines the colors to use")]
        private Gradient thrustColor;

        private float thrustAmount;
        private float thrustMultiplier = 1;
        private bool releaseForShoot;

        /// <summary>
        /// Updates the thurst amount
        /// </summary>
        /// <param name="elapsed">Time passed since the last frame</param>
        private void UpdateThrust(float elapsed)
        {
            thrustAmount += elapsed * thrustMultiplier;

            if (thrustAmount >= 1)
            {
                thrustAmount = 1 - (thrustAmount - 1);
                thrustMultiplier = -1;
            }
            else if (thrustAmount <= 0)
            {
                thrustAmount = -thrustAmount;
                thrustMultiplier = 1;
            }

            // If icon invalid, skip
            if (thrustIcon == null)
                return;

            // Set the fill and the color
            thrustIcon.fillAmount = thrustAmount;
            thrustIcon.color = thrustColor.Evaluate(thrustAmount);
        }

        /// <summary>
        /// Enables the thrust metter
        /// </summary>
        private void StartThrust()
        {
            releaseForShoot = true;
            thrustAmount = 0;
            thrustMultiplier = 1;
            UpdateThrust(0);

            if (thrustCanvas != null)
                thrustCanvas.SetActive(true);
        }

        /// <summary>
        /// Disables the thurst metetr
        /// </summary>
        private void EndThrust()
        {
            releaseForShoot = false;

            if (thrustCanvas != null)
                thrustCanvas.SetActive(false);
        }

        #endregion

        #region IMovable

        /// <inheritdoc/>
        public void OnMove(Vector2 direction) => this.direction = direction;

        #endregion

        #region WeaponController

        /// <inheritdoc/>
        protected override void OnFirePressed() => StartThrust();

        /// <inheritdoc/>
        protected override void OnFireReleased()
        {
            if (releaseForShoot)
                Shoot();
            EndThrust();
        }

        /// <inheritdoc/>
        public override bool CanShoot() => false;

        #endregion

        #region Controller

        /// <inheritdoc/>
        protected override void OnSwitchIn()
        {
            SetCursorLock(true);

            // Reset the direction
            direction = Vector3.zero;

            // Updates the cannon's rotation
            //this.defaultRotation = this.transform.eulerAngles;
            UpdateRotation(Vector3.zero);
        }

        /// <inheritdoc/>
        protected override void OnSwitchOut()
        {
            SetCursorLock(false);

            EndThrust();

            // Put back the cannon at it's default rotation
            //this.transform.eulerAngles = this.defaultRotation;
        }

        /// <inheritdoc/>
        protected override void OnUpdate(float elapsed)
        {
            base.OnUpdate(elapsed);

            if (direction.magnitude != 0)
                UpdateRotation(new Vector3(-direction.y, direction.x, 0));

            UpdateThrust(elapsed);
        }

        #endregion
    }
}