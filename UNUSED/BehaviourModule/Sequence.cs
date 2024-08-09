namespace BehaviourTree
{
    /// <summary>
    /// Node that succeed when all nodes succeed (AND)
    /// </summary>
    public class Sequence : Node
    {
        #region Constructor

        /// <inheritdoc cref="Sequence"/>
        public Sequence(params Node[] children) : base(children) { }

        #endregion

        #region Node

        /// <inheritdoc/>
        public override NodeState Evaluate() 
        {
            this.state = NodeState.SUCCESS;

            foreach (Node child in this)
            {
                NodeState childState = child.Evaluate();

                // If child failed or is running
                if (childState == NodeState.FAILURE || childState == NodeState.RUNNING)
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