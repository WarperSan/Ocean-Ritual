using BehaviourModule.Nodes;
using ExtensionsModule;
using System.Collections.Generic;
using UnityEngine;

namespace BossesModule.Worm.Nodes
{
    internal class RandomAttackNode : Node
    {
        public const string CURRENT_ATTACK_TARGET = "currentAttackTarget";
        public const float COOLDOWN = 10f;

        public RandomAttackNode(
            WormEntity entity,
            Animator   animator,
            Collider   collider,
            string     currentTarget,
            string     isAttacking
        )
        {
            notUsedAttacks = new List<AttackNode>
            {
                new IceStormNode(entity,
                    animator,
                    collider,
                    currentTarget,
                    isAttacking),
                new IceWaveNode(entity,
                    animator,
                    collider,
                    currentTarget,
                    isAttacking),
            };

            #if UNITY_EDITOR
            foreach (AttackNode atk in notUsedAttacks)
                Attach(atk);
            #endif

            ChooseNextAttack();
        }

        #region Attack List

        private List<AttackNode> usedAttacks = new();
        private List<AttackNode> notUsedAttacks;
        private AttackNode currentAttack;

        public void ChooseNextAttack()
        {
            if (notUsedAttacks.Count == 0)
            {
                notUsedAttacks.AddRange(usedAttacks);
                usedAttacks.Clear();
            }

            currentAttack = notUsedAttacks.Random(out int index);
            notUsedAttacks.RemoveAt(index);
            usedAttacks.Add(currentAttack);
        }

        public NodeState OnAnimationEnded()
        {
            currentAttack.OnAnimationEnded();
            return NodeState.SUCCESS;
        }

        public void ResetAttack() => currentAttack.ResetAttack();

        #endregion

        #region Node

        /// <inheritdoc/>
        protected override NodeState OnEvaluate() => currentAttack?.Evaluate() ?? NodeState.FAILURE;

        #endregion
    }
}