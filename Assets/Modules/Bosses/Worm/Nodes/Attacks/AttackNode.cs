using BehaviourModule.Nodes;
using BehaviourModule.Nodes.Controls;
using BehaviourModule.Nodes.Generic;
using UnityEngine;

namespace BossesModule.Worm.Nodes
{
    internal class AttackNode : Sequence
    {
        public const string CURRENT_ATTACK_TARGET = "currentAttackTarget";
        public const float COOLDOWN = 10f;
        CooldownNode cooldown;
        RandomAttackNode rdmAttackNode;
        Attack attack;

        public AttackNode(WormEntity entity, Animator animator, Collider collider, string currentTarget)
        {
            cooldown = new CooldownNode(COOLDOWN);
            this.Attach(cooldown.Alias("Cooldown"));

            rdmAttackNode = new(entity, animator, collider, currentTarget);
            this.Attach(rdmAttackNode.Alias("Random Attack"));

            
            this.Attach(new CallbackNode(this.ResetSequence).Alias("Reset"));
        }

        private NodeState ResetSequence() 
        {
            this.rdmAttackNode.ResetAttack(attack);
            cooldown.ResetCooldown();

            return NodeState.SUCCESS;
        }
        

        #region Node

        /// <inheritdoc/>
        public override bool IsAutomaticallyHidden() => true;

        /// <inheritdoc/>
        public override string GetText() => "Attack Sequence";

        #endregion
    }
}
