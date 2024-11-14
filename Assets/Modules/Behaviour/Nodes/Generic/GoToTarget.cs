using System;
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
            Debug.Log(TARGET);
            Vector3 pos = this.GetData<Vector3>(TARGET);
            pos.y = this.self.position.y; // Walk straight

            // Move self towards target
            float speed = this.GetData<float>(SPEED);
            Vector3 direction = (pos - this.self.position).normalized;

            float stepSize = speed * Time.deltaTime;

            // If close enough, snap
            if (Vector3.Distance(pos, this.self.position) < stepSize)
            {
                this.self.position = pos;
                return NodeState.SUCCESS;
            }

            // Move towards
            this.self.Translate(stepSize * direction, Space.World);
            return NodeState.RUNNING;
        }

        /// <inheritdoc/>
        public override string GetText() => "Go To Target";
    }
}
