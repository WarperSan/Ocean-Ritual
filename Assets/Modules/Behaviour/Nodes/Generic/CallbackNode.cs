namespace BehaviourModule.Nodes.Generic
{
    /// <summary>
    /// Node that executes the given action and returns a given state
    /// </summary>
    public class CallbackNode : Node
    {
        private readonly System.Func<Node, NodeState> CallBack;

        #region Constructor

        public CallbackNode(System.Action<Node> callback, NodeState state) : this(n => {
            callback?.Invoke(n);
            return state;
        }) {}
        
        public CallbackNode(System.Func<Node, NodeState> callback)
        {
            this.CallBack = callback;
        }

        public CallbackNode(System.Func<NodeState> callback)
        {
            this.CallBack = n => callback?.Invoke() ?? NodeState.FAILURE;
        }

        #endregion

        #region Node

        /// <inheritdoc/>
        protected override NodeState OnEvaluate() => this.CallBack(this);

        /// <inheritdoc/>
        public override string GetText()
        {
            string name = this.CallBack.Method.Name;

            return name.StartsWith('<') ? "Lambda" : name + "()";
        }

        #endregion
    }
}