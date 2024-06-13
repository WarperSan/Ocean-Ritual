namespace BehaviourTree.Nodes.Generic
{
    /// <summary>
    /// Node that always returns the same state
    /// </summary>
    public class StableStateNode : Node 
    {
        private readonly NodeState stableState;

        /// <inheritdoc cref="StableStateNode"/>
        public StableStateNode(NodeState state)
        {
            this.stableState = state;
        }

        /// <inheritdoc/>
        public override NodeState Evaluate() => this.stableState;
    }
}