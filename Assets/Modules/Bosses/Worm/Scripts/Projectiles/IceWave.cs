using EntityModule;
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

        public override bool TakeDamage => true;
        protected override void OnPostApply(Entity entity, Attack attack) => this.gameObject.SetActive(false);

        /// <inheritdoc/>
        protected override void OnMove(float elapsed) => this.transform.position -= this.speed * elapsed * this.transform.forward;

        #endregion
    }

}
