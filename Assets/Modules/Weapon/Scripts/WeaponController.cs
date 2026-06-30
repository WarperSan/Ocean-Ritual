using ControllerModule.Controllers;
using ControllerModule.Interfaces.Player;
using EntityModule;
using System;
using UnityEngine;
using WeaponModule.Interfaces;

namespace WeaponModule
{
    public abstract class WeaponController : Controller, IFirable
    {
        #region Shorthands

        private IOverheatable _overheatable;
        private IMultiMode _multiMode;

        /// <inheritdoc cref="IOverheatable.IsOverheated"/>
        private bool IsOverheated() => _overheatable?.IsOverheated() ?? false;

        #endregion

        #region Shoot

        /// <summary>
        /// Triggers this weapon to shoot a bullet
        /// </summary>
        public void Shoot()
        {
            GameObject prefab = GetBullet();

            // If no prefab set, skip
            if (prefab == null)
                return;

            GameObject bullet = CreateBullet(prefab);

            // If error occurred, skip
            if (bullet == null)
                return;

            bullet.SetActive(true);
            OnShoot(bullet);

            // Consume one bullet
            remainingBullets = Math.Max(0, remainingBullets - 1);

            if (remainingBullets == 0)
                OnReloadStart();
        }

        /// <summary>
        /// Called when this weapon shot a bullet
        /// </summary>
        protected virtual void OnShoot(GameObject bullet) { }

        /// <summary>
        /// Determines if this weapon can currently shoot
        /// </summary>
        /// <remarks>
        /// This only applies when the weapon uses a cooldown system. If the weapon shoots by itself, it won't be checked
        /// </remarks>
        public virtual bool CanShoot() => true;

        #endregion

        #region Cooldown

        protected float timeSinceLastShot;

        #endregion

        #region Bullet

        [Header("Bullet")]
        [SerializeField]
        [Tooltip("Determines the object pool for this weapon")]
        protected ObjectPool localObjectPool;

        [SerializeField]
        [Tooltip("Determines the prefab to use for the bullet")]
        private GameObject bulletPrefab;

        /// <summary>
        /// Creates a bullet of the given prefab
        /// </summary>
        private GameObject CreateBullet(GameObject prefab)
        {
            // If no prefab set, skip
            if (prefab == null)
                return null;

            GameObject bullet = FetchBulletInstance(prefab.name);

            // If error occurred, skip
            if (bullet == null)
                return null;

            // Set up projectile
            if (bullet.TryGetComponent(out Projectile projectile))
            {
                projectile.ResetSelf();
                projectile.Attribute(GetAttack());
                SetupProjectile(projectile);
            }

            return bullet;
        }

        /// <summary>
        /// Fetches a bullet from the given prefab
        /// </summary>
        private GameObject FetchBulletInstance(string prefabName)
        {
            // Try to fetch from local
            GameObject bullet = localObjectPool == null ? null : localObjectPool.Get(prefabName);

            // Fetch inside global if not found
            if (bullet == null)
                bullet = ObjectPoolGlobal.Get(prefabName);

            return bullet;
        }

        /// <summary>
        /// Fetches the Attaque value for the new projectile
        /// </summary>
        protected virtual Attack GetAttack() => null;

        /// <summary>
        /// Called when a new projectile is created from this weapon
        /// </summary>
        protected virtual void SetupProjectile(Projectile projectile) { }

        /// <summary>
        /// Fetches the prefab to use for this shot
        /// </summary>
        protected virtual GameObject GetBullet() => bulletPrefab;

        #endregion

        #region Reload

        protected uint remainingBullets;
        private float reloadTimer;

        /// <summary>
        /// Updates the reload of this weapon
        /// </summary>
        protected void Reload(float elapsed)
        {
            uint clipSize = GetClipSize();

            // If already full, skip
            if (remainingBullets >= clipSize)
            {
                remainingBullets = clipSize;
                return;
            }

            // Ajoute le temps �coul� depuis la derni�re mise � jour au temps exc�dentaire
            reloadTimer += elapsed;

            // Calcule le temps n�cessaire pour recharger une balle
            float timeToReloadOneBullet = 1f / GetReloadSpeed(); // secondes par balle

            while (reloadTimer >= timeToReloadOneBullet)
            {
                reloadTimer -= timeToReloadOneBullet;
                remainingBullets = (uint)Mathf.Max(0, remainingBullets + 1);
            }

            OnReload();

            // Si les munitions sont compl�tement recharg�es, on d�sactive la surchauffe
            if (remainingBullets >= clipSize)
            {
                remainingBullets = clipSize;
                OnReloadCompleted();
            }
        }

        /// <summary>
        /// Called when this weapon emptied its clip and needs to reload manually
        /// </summary>
        protected virtual void OnReloadStart() { }

        /// <summary>
        /// Called after this weapon reloaded
        /// </summary>
        protected virtual void OnReload() { }

        /// <summary>
        /// Called when this weapon finished to reload completely
        /// </summary>
        protected virtual void OnReloadCompleted() { }

        /// <summary>
        /// Fetches the number of bullets this weapon can shoot before needing to reload
        /// </summary>
        protected virtual uint GetClipSize() => 0;

        /// <summary>
        /// Fetches how many bullet this weapon reloads per second
        /// </summary>
        protected virtual float GetReloadSpeed() => 0;

        #endregion

        #region Controller

        /// <inheritdoc/>
        protected override void OnStart()
        {
            if (this is IOverheatable overheated)
                _overheatable = overheated;

            if (this is IMultiMode multiMode)
                _multiMode = multiMode;
        }

        /// <inheritdoc/>
        protected override void OnUpdate(float elapsed)
        {
            // Update the cooldown
            timeSinceLastShot += elapsed;

            // If overheated
            if (IsOverheated())
            {
                Reload(elapsed);
                return;
            }

            // Changer de mode avec le bouton molette
            if (Input.GetMouseButtonDown(2) && _multiMode != null)
            {
                Enum nextMode = _multiMode.NextMode();
                _multiMode.SetMode(nextMode);
            }

            // If not firing, reload
            if (!isFiring)
            {
                Reload(elapsed);
                return;
            }

            // If can't shoot, skip
            if (!CanShoot())
                return;

            Shoot();
        }

        #endregion

        #region IFirable

        private bool isFiring;

        /// <inheritdoc/>
        public void OnFireStart()
        {
            isFiring = true;
            OnFirePressed();
        }

        /// <inheritdoc cref="IFirable.OnFireStart"/>
        protected virtual void OnFirePressed() { }

        /// <inheritdoc/>
        public void OnFireEnd()
        {
            isFiring = false;
            OnFireReleased();
        }

        /// <inheritdoc cref="IFirable.OnFireEnd"/>
        protected virtual void OnFireReleased() { }

        #endregion
    }
}