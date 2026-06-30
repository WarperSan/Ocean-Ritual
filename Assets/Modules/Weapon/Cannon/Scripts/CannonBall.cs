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

        private bool hasSplashed;
        public float splashForce;

        private void CreateSplash()
        {
            GameObject splash = Instantiate(splashPrefab);

            // If splash invalid, skip
            if (splash == null)
                return;

            splash.transform.position = transform.position;

            if (splash.TryGetComponent(out ParticleSystem particle))
            {
                // Burst count
                ParticleSystem.Burst burst = particle.emission.GetBurst(0);
                int count = 10;

                if (splashForce > 0.5f)
                    count += Mathf.FloorToInt(40 * splashForce);

                burst.count = count;
                particle.emission.SetBurst(0, burst);

                // Force Over Lifetime
                ParticleSystem.ForceOverLifetimeModule forceOverLifetime = particle.forceOverLifetime;

                var yForce = new ParticleSystem.MinMaxCurve
                {
                    constant = splashForce > 0.5f
                        ? Random.Range(Mathf.Lerp(-10, -5, splashForce), Mathf.Lerp(-5, -1, splashForce))
                        : Random.Range(-10, -5),
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
            if (hasSplashed)
                return;

            Vector3 pos = transform.position;

            // Skip if above water
            if (OceanManager.WATER_HEIGHT < pos.y)
                return;

            hasSplashed = true;
            CreateSplash();

            if (_collider != null)
                _collider.enabled = false;
        }

        /// <inheritdoc/>
        protected override void OnReset()
        {
            hasSplashed = false;

            if (_collider != null)
                _collider.enabled = true;
        }

        #endregion
    }
}