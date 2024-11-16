using UnityEngine;

namespace BossesModule.Worm.Nodes
{
    internal class IceStormNode : AttackNode
    {
        const float COOLDOWN = 20f;
        readonly CooldownNode cooldown;

        public IceStormNode(WormEntity entity, Animator animator, Collider collider, string currentTarget, string isAttacking)
            : base(entity, animator, collider, currentTarget, isAttacking)
        {
            cooldown = new CooldownNode(COOLDOWN);
            this.Attach(this.cooldown.Alias("Cooldown"));
        }

        protected override int GetAttackAnimationIndex() => 0;

        protected override void ResetSelf()
        {
            cooldown.ResetCooldown();
        }

        #region Node

        /// <inheritdoc/>
        public override string GetText() => "Ice Storm Sequence";

        #endregion
    }
}
