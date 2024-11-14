using BehaviourModule.Nodes.Controls;
using BehaviourModule.Nodes.Generic;
using UnityEngine;

namespace BossesModule.Worm.Nodes
{
    internal class AttackNode : Sequence
    {
        public const string CURRENT_ATTACK_TARGET = "currentAttackTarget";
        public const float COOLDOWN = 10f;

        private readonly AnimationNode startAnimation;
        private readonly AnimationNode endAnimation;
        private bool isAttacking = false;

        // FIELDS
        private readonly Transform self;
        private readonly Animator animator;
        private readonly Collider collider;

        public AttackNode(Transform self, Animator animator, Collider collider, string CURRENT_TARGET)
        {
            this.self = self;
            this.animator = animator;
            this.collider = collider;

            CooldownNode cooldown = new CooldownNode(COOLDOWN);
            this.Attach(cooldown.Alias("Cooldown"));

        }

        #region Node

        /// <inheritdoc/>
        public override bool IsAutomaticallyHidden() => true;

        /// <inheritdoc/>
        public override string GetText() => "Attack Sequence";

        #endregion
    }
}
