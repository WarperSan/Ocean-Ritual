using BehaviourModule.Nodes;
using BehaviourModule.Nodes.Controls;
using BehaviourModule.Nodes.Generic;
using UnityEngine;

namespace BossesModule.Worm.Nodes
{
    public class EscapeNode : Sequence
    {
        private const float MIN_TRIGGER_DISTANCE = 40f;
        private const float MIN_TARGET_DISTANCE = 70f;
        public const string CURRENT_ESCAPE_TARGET = "currentEscapeTarget";
        private const string SPEED = "speed";
        private const float RADIUS_OFFSET = 0.9f;

        private readonly AnimationNode startAnimation;
        private readonly AnimationNode endAnimation;
        private bool isEscaping = false;

        // FIELDS
        private readonly WormEntity entity;
        private readonly Animator animator;
        private readonly Collider collider;
        private readonly string currentTarget;

        public EscapeNode(WormEntity entity, Animator animator, Collider collider, string CURRENT_TARGET, string IS_REPOSITIONING)
        {
            this.entity = entity;
            this.animator = animator;
            this.collider = collider;
            this.currentTarget = CURRENT_TARGET;

            // Start/Continue conditions
            this.Attach(EscapeSelector(IS_REPOSITIONING));

            // Play diving animation
            this.startAnimation = new AnimationNode(this.StartAnimation);
            this.Attach(this.startAnimation.Alias("Escape Start Animation"));

            // Go towards target
            this.Attach(new GoToTarget(this.entity.transform, CURRENT_ESCAPE_TARGET, SPEED));

            // Play emerge animation
            this.endAnimation = new AnimationNode(this.EndAnimation);
            this.Attach(this.endAnimation.Alias("Escape End Animation"));

            // Reset animations
            this.Attach(new CallbackNode(() =>
            {
                isEscaping = false;

                this.startAnimation.ResetAnim();
                this.endAnimation.ResetAnim();

                return NodeState.SUCCESS;

            }).Alias("Reset"));

            this.SetData(SPEED, 100f);
        }

        private Node EscapeSelector(string IS_REPOSITIONING)
        {
            Selector escapeSelector = new();
            escapeSelector += new CallbackNode(() => isEscaping ? NodeState.SUCCESS : NodeState.FAILURE).Alias("Is Escaping");

            Sequence conditionSequence = new();
            conditionSequence += new CallbackNode((Node n) => n.GetData<bool>(IS_REPOSITIONING) ? NodeState.FAILURE : NodeState.SUCCESS).Alias("Is Not Repositioning");
            conditionSequence += new DistanceSmaller(this.entity.transform, this.currentTarget, MIN_TRIGGER_DISTANCE).Alias("Is Within Range");

            escapeSelector += conditionSequence.Alias("Condition Sequence");

            return escapeSelector.Alias("Escape Selector");
        }

        private void StartAnimation()
        {
            isEscaping = true;

            this.animator.SetBool("isUnderwater", true);
            this.animator.SetInteger("diveAnimation", 0);

            // Disable collider
            this.collider.enabled = false;

            // Get Random Position
            this.SetData(CURRENT_ESCAPE_TARGET, GetRandomPosition());
        }

        private void EndAnimation()
        {
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

        #region Random Position

        private Vector3 GetRandomPosition()
        {
            Vector3 arenaPos = this.entity.ArenaOrigin.position;
            Vector3 targetPos = this.GetData<Vector3>(currentTarget);
            Vector3 rndPos;

            float maxRadius = this.entity.ArenaRadius * RADIUS_OFFSET;

            while (true)
            {
                float radius = Random.Range(0, maxRadius);
                rndPos = UtilsModule.Random.RandomOnCircumference(radius, arenaPos);

                if (Vector3.Distance(rndPos, targetPos) >= MIN_TARGET_DISTANCE)
                    break;
            }

            return rndPos;
        }

        #endregion
    }
}
