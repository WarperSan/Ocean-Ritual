using EntityModule;
using UnityEngine;

namespace BossesModule.Worm.Projectiles
{
    public class FallingIceProjectile : Projectile
    {
        #region Fields

        [Header("Fields")]
        [SerializeField, Min(0)]
        private float speed;

        #endregion

        #region Projectile

        protected override void OnStart()
        {
            this.ResetSelf();
        }

        /// <inheritdoc/>
        protected override void OnMove(float elapsed) => this.transform.position -= new Vector3(0, this.speed, 0) * elapsed;

        #endregion
    }
}