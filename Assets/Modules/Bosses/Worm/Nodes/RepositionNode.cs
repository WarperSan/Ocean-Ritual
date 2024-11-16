using BehaviourModule.Nodes;
using BehaviourModule.Nodes.Controls;
using BehaviourModule.Nodes.Generic;
using UnityEngine;

namespace BossesModule.Worm.Nodes
{
    public class RepositionNode : MovementNode
    {
        private const float COOLDOWN = 5f;
        private const string CURRENT_REPOSITION_TARGET = "currentRepositionTarget";
        private CooldownNode cooldown;
        private readonly string isRepositioning;

        public RepositionNode(WormEntity entity, Animator animator, Collider collider, string currentTarget, string isRepositioning)
            : base(entity, animator, collider, currentTarget, "Reposition Start Animation", "Reposition End Animation", isRepositioning)
        {
            this.isRepositioning = isRepositioning;
        }

        private Node RepositionSelector()
        {
            Selector repositionSelector = new();
            repositionSelector += new CallbackNode((Node n) => n.GetData<bool>(isRepositioning) ? NodeState.SUCCESS : NodeState.FAILURE).Alias("Is Repositioning");
            cooldown = new CooldownNode(COOLDOWN);
            repositionSelector += this.cooldown.Alias("Cooldown");

            return repositionSelector.Alias("Reposition Selector");
        }

        protected override NodeState ResetSequence()
        {
            this.SetData(isRepositioning, false, -1);

            this.startAnimation.ResetAnim();
            this.endAnimation.ResetAnim();
            this.cooldown.ResetCooldown();

            return NodeState.SUCCESS;
        }
        
        protected override string GetTargetDataKey() => CURRENT_REPOSITION_TARGET;

        protected override int GetDiveAnimationIndex() => 1;
        protected override int GetEmergeAnimationIndex() => 1;
        protected override Node[] GetPreNodes() => new Node[] { this.RepositionSelector() };

        #region Node

        public override string GetText() => "Reposition Node";

        #endregion
    }

}
