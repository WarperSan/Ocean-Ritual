using BehaviourModule.Nodes;
using BehaviourModule.Nodes.Controls;
using BehaviourModule.Nodes.Generic;
using UnityEngine;

namespace BossesModule.Worm.Nodes
{
    public abstract class MovementNode : Sequence
    {
        protected const string SPEED = "speed";
        protected const float MIN_TARGET_DISTANCE = 100f;
        protected const float RADIUS_OFFSET = 0.9f;

        protected readonly AnimationNode startAnimation;
        protected readonly AnimationNode endAnimation;
        protected readonly WormEntity entity;
        protected readonly Animator animator;
        protected readonly Collider collider;
        protected readonly string currentTarget;
        protected readonly string movementTag;

        protected MovementNode(WormEntity entity, Animator animator, Collider collider, string CURRENT_TARGET, string animationAliasStart, string animationAliasEnd, string movementTag)
        {
            this.entity = entity;
            this.animator = animator;
            this.collider = collider;
            this.currentTarget = CURRENT_TARGET;
            this.movementTag = movementTag;

            this.Attach(GetPreNodes());

            // Play diving animation
            this.startAnimation = new AnimationNode(this.StartAnimation);
            this.Attach(this.startAnimation.Alias(animationAliasStart));

            // Go towards target
            this.Attach(new GoToTarget(this.entity.transform, GetTargetDataKey(), SPEED));

            // Play emerge animation
            this.endAnimation = new AnimationNode(this.EndAnimation);
            this.Attach(this.endAnimation.Alias(animationAliasEnd));

            // Reset animations
            this.Attach(new CallbackNode(this.ResetSequence).Alias("Reset"));

            this.SetData(SPEED, 200f);
        }

        protected abstract string GetTargetDataKey();

        protected void StartAnimation()
        {
            this.SetData(this.movementTag, true, -1);

            this.animator.SetBool("isUnderwater", true);
            this.animator.SetInteger("diveAnimation", GetDiveAnimationIndex());

            // Disable collider
            this.collider.enabled = false;

            // Set random position
            this.SetData(GetTargetDataKey(), GetRandomPosition());
        }

        protected void EndAnimation()
        {
            Transform target = this.GetData<Transform>(currentTarget);

            // If target is valid, look at
            if (target != null)
            {
                Vector3 targetPosition = new(
                   target.position.x - this.entity.transform.position.x,
                   this.entity.transform.position.y,
                   target.position.z - this.entity.transform.position.z
               );

                // Rotate self towards target
                this.entity.transform.rotation = Quaternion.LookRotation(targetPosition);
            }

            this.animator.SetBool("isUnderwater", false);
            this.animator.SetInteger("emergeAnimation", GetEmergeAnimationIndex());

            // Enable collider
            this.collider.enabled = true;
        }

        protected abstract NodeState ResetSequence();

        protected Vector3 GetRandomPosition()
        {
            Vector3 selfPos = this.entity.transform.position;
            Vector3 arenaPos = this.entity.ArenaOrigin.position;
            Transform target = this.GetData<Transform>(currentTarget);
            Vector3 targetPos = target != null ? target.position : selfPos;
            Vector3 rndPos;

            float maxRadius = this.entity.ArenaRadius * RADIUS_OFFSET;

            while (true)
            {
                float radius = Random.Range(0, maxRadius);
                rndPos = UtilsModule.Random.RandomOnCircumference(radius, arenaPos);

                // If too close to target, skip
                if (Vector3.Distance(rndPos, targetPos) < MIN_TARGET_DISTANCE)
                    continue;

                // If too close to self, skip
                if (Vector3.Distance(rndPos, selfPos) < 40)
                    continue;

                break;
            }

            return rndPos;
        }

        protected abstract int GetDiveAnimationIndex();
        protected abstract int GetEmergeAnimationIndex();

        public void OnStartAnimationEnded() => this.startAnimation.OnEnded();
        public void OnEndAnimationEnded() => this.endAnimation.OnEnded();

        protected virtual Node[] GetPreNodes() => null;

        #region Node

        public override bool IsAutomaticallyHidden() => true;

        public override string GetText() => "Movement Node";

        #endregion
    }
}
