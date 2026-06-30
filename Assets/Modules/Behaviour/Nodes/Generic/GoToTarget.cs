using UnityEngine;

namespace BehaviourModule.Nodes.Generic
{
    public class GoToTarget : Node
    {
        private readonly Transform self;
        private readonly string TARGET;
        private readonly string SPEED;

        public GoToTarget(Transform self, string TARGET, string SPEED)
        {
            this.self = self;
            this.TARGET = TARGET;
            this.SPEED = SPEED;
        }

        /// <inheritdoc/>
        protected override NodeState OnEvaluate()
        {
            Vector3 pos = GetData<Vector3>(TARGET);
            pos.y = self.position.y; // Walk straight

            // Move self towards target
            float speed = GetData<float>(SPEED);
            Vector3 direction = (pos - self.position).normalized;

            float stepSize = speed * Time.deltaTime;

            // If close enough, snap
            if (Vector3.Distance(pos, self.position) < stepSize)
            {
                self.position = pos;
                return NodeState.SUCCESS;
            }

            // Move towards
            self.Translate(stepSize * direction, Space.World);
            return NodeState.RUNNING;
        }

        /// <inheritdoc/>
        public override string GetText() => "Go To Target";
    }
}