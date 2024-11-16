using BehaviourModule.Nodes;
using BehaviourModule.Nodes.Controls;
using BehaviourModule.Nodes.Generic;
using UnityEngine;

namespace BossesModule.Worm.Nodes
{
    internal abstract class Attack : Sequence
    {
        protected readonly AnimationNode startAnimation;
        protected readonly WormEntity entity;
        protected readonly Animator animator;
        protected readonly Collider collider;
        protected readonly string currentTarget;
        protected readonly string attackTag;

        public Attack(WormEntity entity, Animator animator, Collider collider, string currentTarget, string animationAlias)
        {
            this.entity = entity;
            this.animator = animator;
            this.collider = collider;
            this.currentTarget = currentTarget;

            this.startAnimation = new AnimationNode(this.StartAnimation);
            this.Attach(this.startAnimation.Alias(animationAlias));
        }

        protected abstract int GetAttackAnimationIndex();

        protected void StartAnimation()
        {
            this.animator.SetInteger("attackAnimation", GetAttackAnimationIndex());

            // Disable collider
            this.collider.enabled = false;
        }

        public NodeState ResetAttackAnimation()
        {
            this.startAnimation.ResetAnim();

            return NodeState.SUCCESS;
        }

        public void OnAnimationEnded() => this.startAnimation.OnEnded();


    }
}
