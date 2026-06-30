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

        public AttackNode(
            WormEntity entity,
            Animator   animator,
            Collider   collider,
            string     currentTarget,
            string     isAttacking
        )
        {
            this.entity = entity;
            this.animator = animator;
            this.collider = collider;
            this.currentTarget = currentTarget;
            this.isAttacking = isAttacking;

            startAnimation = new AnimationNode(StartAnimation);
            Attach(startAnimation.Alias("Start Animation"));
        }

        #region Animation

        protected readonly AnimationNode startAnimation;

        protected abstract int GetAttackAnimationIndex();

        private void StartAnimation()
        {
            animator.SetBool("isAttacking", true);
            SetData(isAttacking, true, -1);
            animator.SetInteger("attackAnimation", GetAttackAnimationIndex());

            OnStartAnimation();
        }

        protected virtual void OnStartAnimation() { }

        public void OnAnimationEnded()
        {
            animator.SetBool("isAttacking", false);
            startAnimation.OnEnded();
        }

        public NodeState ResetAttack()
        {
            startAnimation.ResetAnim();
            SetData(isAttacking, false, -1);
            ResetSelf();

            return NodeState.SUCCESS;
        }

        protected virtual void ResetSelf() { }

        #endregion
    }
}