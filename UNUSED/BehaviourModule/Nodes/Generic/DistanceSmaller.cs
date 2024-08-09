using BehaviourTree.Nodes.Abstract;
using UnityEngine;

namespace BehaviourTree.Nodes.Generic
{
    /// <summary>
    /// Node that succeeds when the target is at the given distance or less
    /// </summary>
    public class DistanceSmaller : DistanceNode
    {
        private readonly float distance;

        public DistanceSmaller(Transform self, string target, float distance) : base(self, target)
        {
            this.distance = distance;
        }

        #region DistanceNode

        /// <inheritdoc/>
        protected override NodeState GetState(float distance) => distance <= this.distance ? NodeState.SUCCESS : NodeState.FAILURE;

        #endregion
    }
}