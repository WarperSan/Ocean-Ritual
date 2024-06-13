using UnityEngine;

namespace BehaviourTree.Nodes.Generic
{
    /// <summary>
    /// Node that moves the owner towards a given target
    /// </summary>
    public class MoveToTarget : Node
    {
        public const string SPEED = "MoveSpeed";
        
        private readonly Transform self;
        private readonly System.Action<bool> setWalking;
        private readonly string target;

        public MoveToTarget(Transform self, string target, System.Action<bool> setWalking)
        {
            this.self = self;
            this.setWalking = setWalking;
            this.target = target;
        }

        /// <inheritdoc/>
        public override NodeState Evaluate() 
        {
            // If self is invalid, return failure
            if (this.self == null)
                return NodeState.FAILURE;

            Transform target = this.GetData<Transform>(this.target);

            // If target is invalid, return success
            if (target == null)
                return NodeState.SUCCESS;

            // Move self towards target
            float speed = this.GetData<float>(SPEED);
            Vector3 direction = (target.position - this.self.position).normalized;
            this.self.Translate(direction * speed, Space.World);
            this.setWalking(true);

            return NodeState.RUNNING;
        }
    }
}