using UnityEngine;

namespace BossesModule.Worm.Nodes
{
    internal class IceWaveNode : AttackNode
    {
        public const float COOLDOWN = 30f;
        readonly CooldownNode cooldown;

        public IceWaveNode(WormEntity entity, Animator animator, Collider collider, string currentTarget, string isAttacking)
            : base(entity, animator, collider, currentTarget, isAttacking)
        {
            cooldown = new CooldownNode(COOLDOWN);
            this.Attach(this.cooldown.Alias("Cooldown"));
        }

        protected override int GetAttackAnimationIndex() => 1;

        protected override void ResetSelf()
        {
            cooldown.ResetCooldown();
        }

        #region Node

        /// <inheritdoc/>
        public override string GetText() => "Ice Wave Sequence";

        #endregion
    }
}
