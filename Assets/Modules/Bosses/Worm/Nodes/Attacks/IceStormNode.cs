using BehaviourModule.Nodes;
using System;
using UnityEngine;

namespace BossesModule.Worm.Nodes
{
    internal class IceStormNode : Attack
    {
        const float COOLDOWN = 20f;
        CooldownNode cooldown;

        public IceStormNode(WormEntity entity, Animator animator, Collider collider, string currentTarget, string animationAlias) 
            : base(entity, animator, collider, currentTarget, animationAlias)
        {
            cooldown = new CooldownNode(COOLDOWN);
            this.Attach(this.cooldown.Alias("Cooldown"));
        }

        protected override int GetAttackAnimationIndex() => 0;
        protected override NodeState OnEvaluate() => throw new NotImplementedException();
    }
}
