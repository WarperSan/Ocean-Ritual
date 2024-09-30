using ControllerModule.Controllers;
using ControllerModule.Controllers.Interfaces;
using EntityModule;
using System;
using UnityEngine;
using WeaponModule.Interfaces;

namespace WeaponModule
{
    public abstract class WeaponController : Controller, IFirable
    {
        #region Shorthands

        private IOverheatable _overheatable = null;
        private IMultiMode _multiMode = null;

        /// <inheritdoc cref="IOverheatable.IsOverheated"/>
        private bool IsOverheated() => _overheatable?.IsOverheated() ?? false;

        #endregion

        #region Shoot

        /// <summary>
        /// Triggers this weapon to shoot a bullet
        /// </summary>
        public void Shoot()
        {
            GameObject prefab = this.GetBullet();

            // If no prefab set, skip
            if (prefab == null)
                return;

            GameObject bullet = this.CreateBullet(prefab);

            // If error occurred, skip
            if (bullet == null)
                return;

            bullet.SetActive(true);
            this.OnShoot(bullet);

            // Consume one bullet
            this.remainingBullets = Math.Max(0, this.remainingBullets - 1);

            if (this.remainingBullets == 0)
                this.OnReloadStart();
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

        protected float timeSinceLastShot = 0f;

        #endregion

        #region Bullet

        [Header("Bullet")]
        [SerializeField, Tooltip("Determines the object pool for this weapon")]
        protected ObjectPool localObjectPool = null;

        [SerializeField, Tooltip("Determines the prefab to use for the bullet")]
        private GameObject bulletPrefab;

        /// <summary>
        /// Creates a bullet of the given prefab
        /// </summary>
        private GameObject CreateBullet(GameObject prefab)
        {
            // If no prefab set, skip
            if (prefab == null)
                return null;

            GameObject bullet = this.FetchBulletInstance(prefab.name);

            // If error occurred, skip
            if (bullet == null)
                return null;

            // Set up projectile
            if (bullet.TryGetComponent(out Projectile projectile))
            {
                projectile.ResetSelf();
                projectile.Attribute(this.GetAttack());
                this.SetupProjectile(projectile);
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
        /// Fetches the attack value for the new projectile
        /// </summary>
        protected virtual Attack GetAttack() => null;

        /// <summary>
        /// Called when a new projectile is created from this weapon
        /// </summary>
        protected virtual void SetupProjectile(Projectile projectile) { }

        /// <summary>
        /// Fetches the prefab to use for this shot
        /// </summary>
        protected virtual GameObject GetBullet() => this.bulletPrefab;

        #endregion

        #region Reload

        protected uint remainingBullets;
        private float reloadTimer = 0;

        /// <summary>
        /// Updates the reload of this weapon
        /// </summary>
        protected void Reload(float elapsed)
        {
            uint clipSize = this.GetClipSize();

            // If already full, skip
            if (this.remainingBullets >= clipSize)
            {
                this.remainingBullets = clipSize;
                return;
            }

            // Ajoute le temps �coul� depuis la derni�re mise � jour au temps exc�dentaire
            reloadTimer += elapsed;

            // Calcule le temps n�cessaire pour recharger une balle
            float timeToReloadOneBullet = 1f / this.GetReloadSpeed(); // secondes par balle

            while (reloadTimer >= timeToReloadOneBullet)
            {
                reloadTimer -= timeToReloadOneBullet;
                this.remainingBullets = (uint)Mathf.Max(0, this.remainingBullets + 1);
            }

            this.OnReload();

            // Si les munitions sont compl�tement recharg�es, on d�sactive la surchauffe
            if (this.remainingBullets >= clipSize)
            {
                this.remainingBullets = clipSize;
                this.OnReloadCompleted();
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
                this._overheatable = overheated;

            if (this is IMultiMode multiMode)
                this._multiMode = multiMode;
        }

        /// <inheritdoc/>
        protected override void OnUpdate(float elapsed)
        {
            // Update the cooldown
            this.timeSinceLastShot += elapsed;

            // If overheated
            if (this.IsOverheated())
            {
                this.Reload(elapsed);
                return;
            }

            // Changer de mode avec le bouton molette
            if (Input.GetMouseButtonDown(2) && this._multiMode != null)
            {
                Enum nextMode = this._multiMode.NextMode();
                this._multiMode.SetMode(nextMode);
            }

            // If not firing, reload
            if (!this.isFiring)
            {
                this.Reload(elapsed);
                return;
            }

            // If can't shoot, skip
            if (!this.CanShoot())
                return;

            this.Shoot();
        }

        #endregion

        #region IFirable

        private bool isFiring = false;

        /// <inheritdoc/>
        public void OnFireStart()
        {
            this.isFiring = true;
            this.OnFirePressed();
        }

        /// <inheritdoc cref="IFirable.OnFireStart"/>
        protected virtual void OnFirePressed() { }

        /// <inheritdoc/>
        public void OnFireEnd()
        {
            this.isFiring = false;
            this.OnFireReleased();
        }

        /// <inheritdoc cref="IFirable.OnFireEnd"/>
        protected virtual void OnFireReleased() { }

        #endregion
    }
}