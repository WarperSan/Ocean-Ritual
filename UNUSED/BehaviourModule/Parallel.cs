namespace BehaviourTree
{
    /// <summary>
    /// Node that fails when at least one node fails
    /// </summary>
    public class Parallel : Node
    {
        #region Constructor

        /// <inheritdoc cref="Parallel"/>
        public Parallel(params Node[] children) : base(children) { }

        #endregion

        #region Node

        /// <inheritdoc/>
        public override NodeState Evaluate() 
        {
            this.state = NodeState.RUNNING;

            foreach (Node child in this)
            {
                NodeState childState = child.Evaluate();

                // If child failed 
                if (childState == NodeState.FAILURE)
                {
                    // Copy and exit
                    this.state = childState;
                    break;
                }
            }

            return this.state;
        }

        #endregion
    }
}