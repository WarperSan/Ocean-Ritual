using EntityModule;
using UnityEngine;

namespace BossesModule.Worm.Projectiles
{
    public class FallingIceProjectile : Projectile
    {
        #region Fields

        [Header("Fields")]
        [SerializeField]
        [Min(0)]
        private float speed;

        #endregion

        #region Projectile

        protected override void OnStart() => ResetSelf();

        /// <inheritdoc/>
        protected override void OnMove(float elapsed) => transform.position -= speed * elapsed * transform.up;

        #endregion
    }
}