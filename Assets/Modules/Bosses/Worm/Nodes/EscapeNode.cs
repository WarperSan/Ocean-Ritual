using BehaviourModule.Nodes;
using BehaviourModule.Nodes.Controls;
using BehaviourModule.Nodes.Generic;
using UnityEngine;

namespace BossesModule.Worm.Nodes
{
    public class EscapeNode : MovementNode
    {
        private const float MIN_TRIGGER_DISTANCE = 40f;
        private const string CURRENT_ESCAPE_TARGET = "currentEscapeTarget";
        private readonly string isEscaping;
        private readonly string isRepositioning;

        public EscapeNode(WormEntity entity, Animator animator, Collider collider, string currentTarget, string isEscaping, string isRepositioning)
            : base(entity, animator, collider, currentTarget, "Escape Start Animation", "Escape End Animation", isEscaping)
        {
            this.isEscaping = isEscaping;
            this.isRepositioning = isRepositioning;
        }

        private Node EscapeSelector()
        {
            Selector escapeSelector = new();
            escapeSelector += new CallbackNode((Node n) => n.GetData<bool>(isEscaping) ? NodeState.SUCCESS : NodeState.FAILURE).Alias("Is Escaping");

            Sequence conditionSequence = new();
            conditionSequence += new CallbackNode((Node n) => n.GetData<bool>(isRepositioning) ? NodeState.FAILURE : NodeState.SUCCESS).Alias("Is Not Repositioning");
            conditionSequence += new DistanceSmaller(this.entity.transform, this.currentTarget, MIN_TRIGGER_DISTANCE).Alias("Is Within Range");

            escapeSelector += conditionSequence.Alias("Condition Sequence");

            return escapeSelector.Alias("Escape Selector");
        }

        protected override NodeState ResetSequence()
        {
            this.SetData(isEscaping, false, -1);

            this.startAnimation.ResetAnim();
            this.endAnimation.ResetAnim();

            return NodeState.SUCCESS;
        }

        protected override string GetTargetDataKey() => CURRENT_ESCAPE_TARGET;
        protected override int GetDiveAnimationIndex() => 0;
        protected override int GetEmergeAnimationIndex() => 0;

        protected override Node[] GetPreNodes() => new Node[] { this.EscapeSelector() };

        #region Node

        public override string GetText() => "Escape Node";

        #endregion
    }

}
