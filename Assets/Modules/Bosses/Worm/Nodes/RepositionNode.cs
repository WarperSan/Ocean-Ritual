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
        private readonly CooldownNode cooldown;

        // FIELDS
        private readonly Transform self;
        private readonly Animator animator;
        private readonly Collider collider;
        private readonly string IS_REPOSITIONING;

        public RepositionNode(Transform self, Animator animator, Collider collider, string CURRENT_TARGET, string IS_REPOSITIONING)
        {
            this.self = self;
            this.animator = animator;
            this.collider = collider;
            this.IS_REPOSITIONING = IS_REPOSITIONING;

            Selector repositionSelector = new();
            // Distance check
            repositionSelector += new CallbackNode((Node n) => n.GetData<bool>(IS_REPOSITIONING) ? NodeState.SUCCESS : NodeState.FAILURE).Alias("Is Repositioning");
            cooldown = new CooldownNode(COOLDOWN);
            repositionSelector += cooldown.Alias("Cooldown");
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
                this.cooldown.ResetCooldown();

                return NodeState.SUCCESS;

            }).Alias("Reset"));

            this.SetData(SPEED, 10f);
            this.SetData(IS_REPOSITIONING, false, -1);
        }

        private void StartAnimation()
        {
            this.SetData(IS_REPOSITIONING, true, -1);

            this.animator.SetBool("isUnderwater", true);
            this.animator.SetInteger("diveAnimation", 1);

            // Disable collider
            this.collider.enabled = false;

            // PICK RANDOM LOCATION
            this.SetData(CURRENT_REPOSITION_TARGET, TargetGeneral.Instance.BoatTarget.position);
        }

        private void EndAnimation()
        {
            this.SetData(IS_REPOSITIONING, false, -1);

            this.animator.SetBool("isUnderwater", false);
            this.animator.SetInteger("emergeAnimation", 1);

            // Enable collider
            this.collider.enabled = true;
        }

        public void OnStartAnimationEnded() => this.startAnimation.OnEnded();
        public void OnEndAnimationEnded()=> this.endAnimation.OnEnded();

        #region Node

        /// <inheritdoc/>
        public override bool IsAutomaticallyHidden() => true;

        /// <inheritdoc/>
        public override string GetText() => "Reposition Sequence";

        #endregion
    }
}
