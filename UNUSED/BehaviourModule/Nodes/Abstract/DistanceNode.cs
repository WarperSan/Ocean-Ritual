using Extensions;
using UnityEngine;

namespace BehaviourTree.Nodes.Abstract
{
    /// <summary>
    /// Nodes that calculates the distance between self and the target
    /// </summary>
    public abstract class DistanceNode : Node 
    {
        private readonly Transform self;
        private readonly string target;

        public DistanceNode(Transform self, string target)
        {
            this.self = self;
            this.target = target;
        }

        #region Node

        /// <inheritdoc/>
        public sealed override NodeState Evaluate() 
        {
            // If self is invalid, return failure
            if (this.self == null)
                return NodeState.FAILURE;

            // If target is invalid, return success
            Transform target = this.GetData<Transform>(this.target);

            if (target == null)
                return NodeState.SUCCESS;

            // Get distance
            float distance = this.self.Distance(target);

            // If close enough from target
            return this.GetState(distance);
        }

        #endregion

        #region Abstract

        /// <returns>State of this node depending on the distance</returns>
        protected abstract NodeState GetState(float distance);

        #endregion
    }
}