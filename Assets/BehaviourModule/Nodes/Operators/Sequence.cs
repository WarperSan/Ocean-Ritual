using BehaviourModule.Nodes.Generic;

namespace BehaviourModule.Nodes.Operators
{
    /// <summary>
    /// Node that only succeed when all of its children succeed. (AND)
    /// </summary>
    /// <remarks>
    /// If a child's state is <see cref="NodeState.RUNNING"/> or <see cref="NodeState.FAILURE"/>, this node exists with this state.
    /// </remarks>
    public class Sequence : Node
    {
        #region Constructor

        /// <inheritdoc cref="Node(Node[])"/>
        public Sequence(params Node[] children) : base(children) { }

        #endregion

        #region Node

        /// <inheritdoc/>
        protected override NodeState OnEvaluate() 
        {
            foreach (Node child in this)
            {
                NodeState childState = child.Evaluate();

                // If child succeed, skip
                if (childState == NodeState.SUCCESS) 
                    continue;
                
                // Exit
                return childState;
            }

            return NodeState.SUCCESS;
        }

        /// <inheritdoc/>
        public override string GetText() => "AND";

        #endregion
    }
}