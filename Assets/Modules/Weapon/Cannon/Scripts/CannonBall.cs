using EntityModule;
using MapModule;
using UnityEngine;

namespace WeaponModule.Weapons.Cannon
{
    public class CannonBall : Projectile
    {
        #region Splash

        [Header("Splash")]
        [SerializeField]
        private GameObject splashPrefab;
        private bool hasSplashed = false;
        public float splashForce = 0;

        private void CreateSplash()
        {
            GameObject splash = Instantiate(this.splashPrefab);

            // If splash invalid, skip
            if (splash == null)
                return;

            splash.transform.position = this.transform.position;

            if (splash.TryGetComponent(out ParticleSystem particle))
            {
                // Burst count
                ParticleSystem.Burst burst = particle.emission.GetBurst(0);
                int count = 10;

                if (this.splashForce > 0.5f)
                    count += Mathf.FloorToInt(40 * this.splashForce);

                burst.count = count;
                particle.emission.SetBurst(0, burst);

                // Force Over Lifetime
                ParticleSystem.ForceOverLifetimeModule forceOverLifetime = particle.forceOverLifetime;
                var yForce = new ParticleSystem.MinMaxCurve
                {
                    constant = this.splashForce > 0.5f ? Random.Range(Mathf.Lerp(-10, -5, this.splashForce), Mathf.Lerp(-5, -1, this.splashForce)) : Random.Range(-10, -5)
                };

                forceOverLifetime.y = yForce;
            }
        }

        #endregion

        #region Projectile

        /// <inheritdoc/>
        protected override void OnUpdate(float elapsed)
        {
            // Stop checking if already splashed
            if (this.hasSplashed)
                return;

            Vector3 pos = this.transform.position;

            // Skip if above water
            if (OceanManager.WATER_HEIGHT < pos.y)
                return;

            this.hasSplashed = true;
            this.CreateSplash();

            if (this._collider != null)
                this._collider.enabled = false;
        }

        /// <inheritdoc/>
        protected override void OnReset()
        {
            this.hasSplashed = false;

            if (this._collider != null)
                this._collider.enabled = true;
        }

        #endregion
    }
}