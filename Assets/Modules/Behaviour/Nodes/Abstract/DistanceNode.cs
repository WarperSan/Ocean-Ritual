using UnityEngine;

namespace BehaviourModule.Nodes.Abstract
{
    /// <summary>
    /// Nodes that calculates the distance between self and the target
    /// </summary>
    public abstract class DistanceNode : Node
    {
        private readonly Transform self;
        private readonly string target;

        #region Constructor

        public DistanceNode(Transform self, string target)
        {
            this.self = self;
            this.target = target;
        }

        #endregion

        #region Node

        /// <inheritdoc/>
        protected override NodeState OnEvaluate()
        {
            // If self is invalid, return failure
            if (self == null)
                return NodeState.FAILURE;

            // If target is invalid, return success
            Transform _target = GetData<Transform>(target);

            if (_target == null)
                return NodeState.FAILURE;

            // Get distance
            float distance = Vector3.Distance(self.position, _target.position);

            // If close enough from target
            return GetState(distance);
        }

        #endregion

        #region Abstract

        /// <returns>State of this node depending on the distance</returns>
        protected abstract NodeState GetState(float distance);

        #endregion
    }
}