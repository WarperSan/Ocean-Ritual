using BehaviourModule.Nodes;
using BehaviourModule.Nodes.Controls;
using BehaviourModule.Nodes.Generic;
using UnityEngine;

namespace BossesModule.Worm.Nodes
{
    public class RepositionNode : Sequence
    {
        private const float COOLDOWN = 5f;
        private const string CURRENT_REPOSITION_TARGET = "currentRepositionTarget";
        private const string SPEED = "speed";

        private readonly AnimationNode startAnimation;
        private readonly AnimationNode endAnimation;
        private bool isRepositioning = false;

        // FIELDS
        private readonly Transform self;
        private readonly Animator animator;
        private readonly Collider collider;

        public RepositionNode(Transform self, Animator animator, Collider collider, string CURRENT_TARGET)
        {
            this.self = self;
            this.animator = animator;
            this.collider = collider;

            this.isRepositioning = true;

            Selector repositionSelector = new();
            // Cooldown
            // Distance check
            repositionSelector += new CallbackNode(() => isRepositioning ? NodeState.SUCCESS : NodeState.FAILURE).Alias("Is Repositioning");
            this.Attach(repositionSelector.Alias("Reposition Selector"));

            // Play diving animation
            this.startAnimation = new AnimationNode(this.StartAnimation);
            this.Attach(this.startAnimation.Alias("Reposition Start Animation"));

            // Go towards target
            this.Attach(new GoToTarget(this.self, CURRENT_REPOSITION_TARGET, SPEED));

            // Play emerge animation
            this.endAnimation = new AnimationNode(this.EndAnimation);
            this.Attach(this.endAnimation.Alias("Reposition End Animation"));

            this.Attach(new CallbackNode(() =>
            {
                this.startAnimation.ResetAnim();
                this.endAnimation.ResetAnim();

                return NodeState.SUCCESS;

            }).Alias("Reset Animations"));

            this.SetData(SPEED, 5f);
        }

        private void StartAnimation()
        {
            isRepositioning = true;

            this.animator.SetBool("isUnderwater", true);
            this.animator.SetInteger("diveAnimation", 1);

            // Disable collider
            this.collider.enabled = false;

            // PICK RANDOM LOCATION
            this.SetData(CURRENT_REPOSITION_TARGET, TargetGeneral.Instance.BoatTarget.position);
        }

        private void EndAnimation()
        {
            isRepositioning = false;

            this.animator.SetBool("isUnderwater", false);
            this.animator.SetInteger("emergeAnimation", 1);

            // Enable collider
            this.collider.enabled = true;
        }

        public void OnStartAnimationEnded() => this.startAnimation.OnEnded();
        public void OnEndAnimationEnded() => this.endAnimation.OnEnded();

        #region Node

        /// <inheritdoc/>
        public override bool IsAutomaticallyHidden() => true;

        /// <inheritdoc/>
        public override string GetText() => "Reposition Sequence";

        #endregion
    }
}
