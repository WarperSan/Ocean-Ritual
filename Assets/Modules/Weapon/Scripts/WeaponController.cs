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
        public abstract float GetDamage();// donne a la balle le nombre de d�gat qu'elle fait
        public abstract void Reload();//rechargement

        public abstract void SetBulletProriety(GameObject balle);// g�re les stat de la balle

        #region Shorthands

        private IOverheatable _overheatable = null;
        private IMultiMode _multiMode = null;

        /// <inheritdoc cref="IOverheatable.IsOverheated"/>
        private bool IsOverheated() => _overheatable?.IsOverheated() ?? false;

        #endregion

        #region Shoot

        /// <summary>
        /// Called when this weapon shoots
        /// </summary>
        public virtual void Shoot() { }

        /// <summary>
        /// Determines if this weapon can currently shoot
        /// </summary>
        /// <returns></returns>
        public virtual bool CanShoot() => true;

        #endregion

        #region Cooldown

        protected float timeSinceLastShot = 0f;

        #endregion

        #region Bullet

        [Header("Bullet")]
        [SerializeField, Tooltip("Determines the object pool for this weapon")]
        protected ObjectPool localObjectPool = null;

        /// <summary>
        /// Fetches a bullet from the given prefab
        /// </summary>
        protected GameObject GetBullet(string prefabName)
        {
            // Try to fetch from local
            GameObject bullet = localObjectPool == null ? null : localObjectPool.Get(prefabName);

            // Fetch inside global if not found
            if (bullet == null)
                bullet = ObjectPoolGlobal.Get(prefabName);

            return bullet;
        }

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
                this.Reload();
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
                this.Reload();
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
        public void OnFireStart() => this.isFiring = true;

        /// <inheritdoc/>
        public void OnFireEnd() => this.isFiring = false;

        #endregion
    }
}