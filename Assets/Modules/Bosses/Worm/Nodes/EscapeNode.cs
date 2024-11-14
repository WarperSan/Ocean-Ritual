using BehaviourModule.Nodes;
using BehaviourModule.Nodes.Controls;
using BehaviourModule.Nodes.Generic;
using UnityEngine;

namespace BossesModule.Worm.Nodes
{
    public class EscapeNode : Sequence
    {
        private const float MIN_DISTANCE = 40f;
        public const string CURRENT_ESCAPE_TARGET = "currentEscapeTarget";
        private const string SPEED = "speed";

        private readonly AnimationNode startAnimation;
        private readonly AnimationNode endAnimation;
        private bool isEscaping = false;

        // FIELDS
        private readonly Transform self;
        private readonly Animator animator;
        private readonly Collider collider;

        public EscapeNode(Transform self, Animator animator, Collider collider, string CURRENT_TARGET)
        {
            this.self = self;
            this.animator = animator;
            this.collider = collider;

            // Start/Continue conditions
            Selector escapeSelector = new();
            escapeSelector += new CallbackNode(() => isEscaping ? NodeState.SUCCESS : NodeState.FAILURE).Alias("Is Escaping");
            escapeSelector += new DistanceSmaller(this.self, CURRENT_TARGET, MIN_DISTANCE).Alias("Is Within Range");
            this.Attach(escapeSelector.Alias("Escape Selector"));

            // Play diving animation
            this.startAnimation = new AnimationNode(this.StartAnimation);
            this.Attach(this.startAnimation.Alias("Escape Start Animation"));

            // Go towards target
            this.Attach(new GoToTarget(this.self, CURRENT_ESCAPE_TARGET, SPEED));

            // Play emerge animation
            this.endAnimation = new AnimationNode(this.EndAnimation);
            this.Attach(this.endAnimation.Alias("Escape End Animation"));

            // Reset animations
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
            isEscaping = true;

            this.animator.SetBool("isUnderwater", true);
            this.animator.SetInteger("diveAnimation", 0);

            // Disable collider
            this.collider.enabled = false;

            // PICK RANDOM LOCATION
            this.SetData(CURRENT_ESCAPE_TARGET, TargetGeneral.Instance.BoatTarget.position);
        }

        private void EndAnimation()
        {
            isEscaping = false;

            this.animator.SetBool("isUnderwater", false);
            this.animator.SetInteger("emergeAnimation", 0);

            // Enable collider
            this.collider.enabled = true;
        }

        public void OnStartAnimationEnded() => this.startAnimation.OnEnded();
        public void OnEndAnimationEnded() => this.endAnimation.OnEnded();

        #region Node

        /// <inheritdoc/>
        public override bool IsAutomaticallyHidden() => true;

        /// <inheritdoc/>
        public override string GetText() => "Escape Sequence";

        #endregion
    }
}
