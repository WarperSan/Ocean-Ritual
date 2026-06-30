using EntityModule;
using UnityEngine;

namespace BossesModule.Worm.Projectiles
{
    public class IceWave : Projectile
    {
        #region Fields

        [Header("Fields")]
        [SerializeField]
        [Min(0)]
        private float speed;

        #endregion

        #region Projectile

        public override    bool TakeDamage                                => true;
        protected override void OnPostApply(Entity entity, Attack attack) => gameObject.SetActive(false);

        /// <inheritdoc/>
        protected override void OnMove(float elapsed) => transform.position -= speed * elapsed * transform.forward;

        #endregion
    }
}