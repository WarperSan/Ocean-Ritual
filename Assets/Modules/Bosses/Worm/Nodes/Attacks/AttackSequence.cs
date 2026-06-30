using BehaviourModule.Nodes;
using BehaviourModule.Nodes.Controls;
using BehaviourModule.Nodes.Generic;
using UnityEngine;

namespace BossesModule.Worm.Nodes
{
    internal class AttackSequence : Sequence
    {
        public const string CURRENT_ATTACK_TARGET = "currentAttackTarget";
        public const float COOLDOWN = 5f;
        private CooldownNode cooldown;

        public AttackSequence(
            WormTree   tree,
            WormEntity entity,
            Animator   animator,
            Collider   collider,
            string     currentTarget,
            string     isAttacking
        )
        {
            cooldown = new CooldownNode(COOLDOWN);
            Attach(cooldown.Alias("Cooldown"));

            Selector conditionSelector = new();

            conditionSelector +=
                new CallbackNode((Node n) => n.GetData<bool>(isAttacking) ? NodeState.SUCCESS : NodeState.FAILURE).Alias(
                    "Is Already Attacking");
            conditionSelector += new CallbackNode(() => tree.IsMoving() ? NodeState.FAILURE : NodeState.SUCCESS).Alias("Is Not Moving");

            Attach(conditionSelector.Alias("Condition Selector"));

            rdmAttackNode = new RandomAttackNode(entity,
                animator,
                collider,
                currentTarget,
                isAttacking);
            Attach(rdmAttackNode.Alias("Random Attack"));

            Attach(new CallbackNode(ResetSequence).Alias("Reset"));
        }

        private NodeState ResetSequence()
        {
            cooldown.ResetCooldown();
            rdmAttackNode.ResetAttack();
            rdmAttackNode.ChooseNextAttack();

            return NodeState.SUCCESS;
        }

        #region Random Attack

        private RandomAttackNode rdmAttackNode;

        public void OnAnimationEnded() => rdmAttackNode.OnAnimationEnded();

        #endregion

        #region Node

        /// <inheritdoc/>
        //public override bool IsAutomaticallyHidden() => true;

        /// <inheritdoc/>
        public override string GetText() => "Attack Sequence";

        #endregion
    }
}