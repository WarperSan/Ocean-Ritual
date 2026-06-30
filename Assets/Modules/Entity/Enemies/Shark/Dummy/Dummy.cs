using UnityEngine;

namespace EntityModule.Enemies
{
    public class Dummy : Entity
    {
        public Animator animator;

        #region Entity

        /// <inheritdoc/>
        public override bool TakeDamage => false;

        /// <inheritdoc/>
        protected override void OnPreAttack(Projectile source) => animator.SetTrigger("Hit");

        #endregion
    }
}