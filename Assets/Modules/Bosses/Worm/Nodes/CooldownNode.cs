using BehaviourModule.Nodes;
using UnityEngine;

namespace BossesModule.Worm.Nodes
{
    internal class CooldownNode : Node
    {
        private float cooldown;
        private readonly float initialCooldown;

        public CooldownNode(float cooldown, bool startOnCooldown = true)
        {
            initialCooldown = cooldown;

            this.cooldown = startOnCooldown ? cooldown : 0;
        }

        protected override NodeState OnEvaluate()
        {
            if (cooldown > 0)
                cooldown -= Time.deltaTime;

            if (cooldown <= 0)
                return NodeState.SUCCESS;

            return NodeState.FAILURE;
        }

        public void ResetCooldown() => cooldown = initialCooldown;
    }
}