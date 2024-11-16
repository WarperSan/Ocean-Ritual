using BehaviourModule.Nodes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BossesModule.Worm.Nodes
{
    internal class RandomAttackNode : Node
    {
        public const string CURRENT_ATTACK_TARGET = "currentAttackTarget";
        public const float COOLDOWN = 10f;
        private List<Attack> usedAttacks = new();
        private List<Attack> notUsedAttacks;
        public RandomAttackNode(WormEntity entity, Animator animator, Collider collider, string currentTarget)
        {
            notUsedAttacks = new List<Attack>() {
                new IceStormNode(entity, animator, collider, currentTarget, "Ice Storm Animation")
            };

            this.Attach(this.notUsedAttacks[0].Alias("Ice Storm Sequence"));
        }

        protected override NodeState OnEvaluate() => throw new NotImplementedException();

        public NodeState ResetAttack(Attack attack)
        {
            attack.ResetAttackAnimation();

            return NodeState.SUCCESS;
        }

    }
}
