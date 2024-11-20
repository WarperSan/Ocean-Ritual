using BehaviourModule.Nodes;
using BehaviourModule.Nodes.Controls;
using BehaviourModule.Nodes.Generic;
using UnityEngine;

namespace BossesModule.Worm.Nodes
{
    internal abstract class AttackNode : Sequence
    {
        protected readonly WormEntity entity;
        protected readonly Animator animator;
        protected readonly Collider collider;
        protected readonly string currentTarget;
        private readonly string isAttacking;

        public AttackNode(WormEntity entity, Animator animator, Collider collider, string currentTarget, string isAttacking)
        {
            this.entity = entity;
            this.animator = animator;
            this.collider = collider;
            this.currentTarget = currentTarget;
            this.isAttacking = isAttacking;

            this.startAnimation = new AnimationNode(this.StartAnimation);
            this.Attach(this.startAnimation.Alias("Start Animation"));
        }

        #region Animation

        protected readonly AnimationNode startAnimation;

        protected abstract int GetAttackAnimationIndex();

        private void StartAnimation()
        {
            this.animator.SetBool("isAttacking", true);
            this.SetData(isAttacking, true, -1);
            this.animator.SetInteger("attackAnimation", this.GetAttackAnimationIndex());

            this.OnStartAnimation();
        }

        protected virtual void OnStartAnimation() { }

        public void OnAnimationEnded()
        {
            this.animator.SetBool("isAttacking", false);
            this.startAnimation.OnEnded();
        }

        public NodeState ResetAttack()
        {
            this.startAnimation.ResetAnim();
            this.SetData(isAttacking, false, -1);
            this.ResetSelf();

            return NodeState.SUCCESS;
        }

        protected virtual void ResetSelf() {}

        #endregion
    }
}
