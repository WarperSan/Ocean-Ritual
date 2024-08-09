using UnityEngine;

namespace BehaviourTree.Nodes.Generic
{
    /// <summary>
    /// Nodes that plays a given animation
    /// </summary>
    public class AnimationNode : Node
    {
        private readonly Animator animator;
        private float delay;
        private float animationTime;
        private readonly System.Action<Animator> onStart;

        public AnimationNode(Animator animator, System.Action<Animator> onStart = null)
        {
            this.animator = animator;
            this.onStart = onStart;
        }

        public AnimationNode(Animator animator, float delay, float length, System.Action<Animator> onStart = null) : this(animator, onStart)
        {
            this.Delay = delay;
            this.ClipLength = length;
        }

        #region Node

        /// <inheritdoc/>
        public override NodeState Evaluate()
        {
            float currentTime = Time.timeSinceLevelLoad;

            if (currentTime > this.delay)
            {
                this.OnAnimationStart(this.animator);

                this.animationTime = currentTime + this.ClipLength;
                this.delay = this.animationTime + this.Delay;
            }

            return currentTime >= this.animationTime ? NodeState.FAILURE : NodeState.SUCCESS;
        }

        #endregion

        #region Virtual

        /// <summary>
        /// Called when the animation starts
        /// </summary>
        protected virtual void OnAnimationStart(Animator animator) => this.onStart?.Invoke(animator);

        /// <summary>
        /// Time between the loops
        /// </summary>
        protected virtual float Delay { get; set; }

        /// <summary>
        /// Length of the animation in seconds
        /// </summary>
        protected virtual float ClipLength { get; set; }

        #endregion
    }
}