using BehaviourModule.Nodes;
using System.Collections.Generic;
using UnityEngine;

namespace BossesModule.Worm.Nodes
{
    internal class RandomAttackNode : Node
    {
        public const string CURRENT_ATTACK_TARGET = "currentAttackTarget";
        public const float COOLDOWN = 10f;

        public RandomAttackNode(WormEntity entity, Animator animator, Collider collider, string currentTarget, string isAttacking)
        {
            notUsedAttacks = new List<AttackNode>() {
                new IceStormNode(entity, animator, collider, currentTarget, isAttacking)
            };

#if UNITY_EDITOR
            foreach (AttackNode atk in notUsedAttacks)
                this.Attach(atk);
#endif

            this.ChooseNextAttack();
        }

        #region Attack List

        private List<AttackNode> usedAttacks = new();
        private List<AttackNode> notUsedAttacks;
        private AttackNode currentAttack;

        public void ChooseNextAttack()
        {
            this.currentAttack = this.notUsedAttacks[0];
        }

        public NodeState OnAnimationEnded()
        {
            this.currentAttack.OnAnimationEnded();
            return NodeState.SUCCESS;
        }

        public void ResetAttack()
        {
            this.currentAttack.ResetAttack();
        }

        #endregion

        #region Node

        /// <inheritdoc/>
        protected override NodeState OnEvaluate() => this.currentAttack?.Evaluate() ?? NodeState.FAILURE;

        #endregion
    }
}
