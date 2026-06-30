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

        protected MovementNode(
            WormEntity entity,
            Animator   animator,
            Collider   collider,
            string     CURRENT_TARGET,
            string     animationAliasStart,
            string     animationAliasEnd,
            string     movementTag
        )
        {
            this.entity = entity;
            this.animator = animator;
            this.collider = collider;
            currentTarget = CURRENT_TARGET;
            this.movementTag = movementTag;

            Attach(GetPreNodes());

            // Play diving animation
            startAnimation = new AnimationNode(StartAnimation);
            Attach(startAnimation.Alias(animationAliasStart));

            // Go towards target
            Attach(new GoToTarget(this.entity.transform, GetTargetDataKey(), SPEED));

            // Play emerge animation
            endAnimation = new AnimationNode(EndAnimation);
            Attach(endAnimation.Alias(animationAliasEnd));

            // Reset animations
            Attach(new CallbackNode(ResetSequence).Alias("Reset"));

            SetData(SPEED, 200f);
        }

        protected abstract string GetTargetDataKey();

        protected void StartAnimation()
        {
            SetData(movementTag, true, -1);

            animator.SetBool("isUnderwater", true);
            animator.SetInteger("diveAnimation", GetDiveAnimationIndex());

            // Disable collider
            collider.enabled = false;

            // Set random position
            SetData(GetTargetDataKey(), GetRandomPosition());
        }

        protected void EndAnimation()
        {
            Transform target = GetData<Transform>(currentTarget);

            // If target is valid, look at
            if (target != null)
            {
                Vector3 targetPosition = new(
                    target.position.x - entity.transform.position.x,
                    entity.transform.position.y,
                    target.position.z - entity.transform.position.z
                );

                // Rotate self towards target
                entity.transform.rotation = Quaternion.LookRotation(targetPosition);
            }

            animator.SetBool("isUnderwater", false);
            animator.SetInteger("emergeAnimation", GetEmergeAnimationIndex());

            // Enable collider
            collider.enabled = true;
        }

        protected abstract NodeState ResetSequence();

        protected Vector3 GetRandomPosition()
        {
            Vector3 selfPos = entity.transform.position;
            Vector3 arenaPos = entity.ArenaOrigin.position;
            Transform target = GetData<Transform>(currentTarget);
            Vector3 targetPos = target != null ? target.position : selfPos;
            Vector3 rndPos;

            float maxRadius = entity.ArenaRadius * RADIUS_OFFSET;

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

        public void OnStartAnimationEnded() => startAnimation.OnEnded();
        public void OnEndAnimationEnded()   => endAnimation.OnEnded();

        protected virtual Node[] GetPreNodes() => null;

        #region Node

        public override bool IsAutomaticallyHidden() => true;

        public override string GetText() => "Movement Node";

        #endregion
    }
}