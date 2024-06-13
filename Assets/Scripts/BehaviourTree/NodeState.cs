namespace BehaviourTree
{
    /// <summary>
    /// Current state of the node
    /// </summary>
    public enum NodeState
    {
        /// <summary>This node succeed its task</summary>
        SUCCESS = 0,

        /// <summary>This node failed its task</summary>
        FAILURE = 1,

        /// <summary>This node is processing its task</summary>
        RUNNING = 2,
    }
}