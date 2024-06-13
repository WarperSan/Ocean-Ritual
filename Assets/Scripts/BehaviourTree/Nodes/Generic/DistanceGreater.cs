using BehaviourTree.Nodes.Abstract;
using UnityEngine;

namespace BehaviourTree.Nodes.Generic
{
    /// <summary>
    /// Node that succeeds when the target is at the given distance or more
    /// </summary>
    public class DistanceGreater : DistanceNode
    {
        private readonly float distance;

        public DistanceGreater(Transform self, string target, float distance) : base(self, target)
        {
            this.distance = distance;
        }

        #region DistanceNode

        /// <inheritdoc/>
        protected override NodeState GetState(float distance) => distance >= this.distance ? NodeState.SUCCESS : NodeState.FAILURE;

        #endregion
    }
}