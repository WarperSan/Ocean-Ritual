namespace BehaviourTree.Nodes.Generic
{
    /// <summary>
    /// Node that executes the given action and returns a given state
    /// </summary>
    public class CallbackNode : Node
    {
        private readonly System.Func<Node, NodeState> CallBack;

        public CallbackNode(System.Action<Node> callback, NodeState state) : this(n => {
            callback?.Invoke(n);
            return state;
        }) {}

        public CallbackNode(System.Func<Node, NodeState> callback) => this.CallBack = callback;

        #region Node

        /// <inheritdoc/>
        public override NodeState Evaluate() => this.CallBack(this);

        #endregion
    }
}