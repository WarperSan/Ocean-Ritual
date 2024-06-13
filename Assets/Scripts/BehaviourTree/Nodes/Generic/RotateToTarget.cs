using UnityEngine;

namespace BehaviourTree.Nodes.Generic
{
    /// <summary>
    /// Node that rotates the owner towards a given target
    /// </summary>
    public class RotateToTarget : Node
    {
        private readonly Transform self;
        private readonly string target;

        /// <inheritdoc cref="RotateToTarget"/>
        public RotateToTarget(Transform self, string target)
        {
            this.self = self;
            this.target = target;
        }

        /// <inheritdoc/>
        public override NodeState Evaluate() 
        {
            // If self is invalid, return failure
            if (this.self == null)
                return NodeState.FAILURE;

            Transform target = this.GetData<Transform>(this.target);

            // If target is invalid, return success
            if (target == null)
                return NodeState.SUCCESS;

            // Rotate self towards target
            var targetRotation = Quaternion.LookRotation(target.position - this.self.position);
            this.self.rotation = Quaternion.RotateTowards(this.self.rotation, targetRotation, 45 * Time.deltaTime);

            return NodeState.RUNNING;
        }
    }
}