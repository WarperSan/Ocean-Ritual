using EntityModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossesModule.Worm.Projectiles
{
    public class IceWave : Projectile
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

        public override bool TakeDamage => true;

        /// <inheritdoc/>
        protected override void OnMove(float elapsed) => this.transform.position -= this.speed * elapsed * this.transform.forward;

        #endregion
    }

}
