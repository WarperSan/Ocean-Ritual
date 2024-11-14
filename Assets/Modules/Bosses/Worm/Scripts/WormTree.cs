using BehaviourModule.Interfaces;
using BehaviourModule.Nodes;
using BehaviourModule.Nodes.Controls;
using BehaviourModule.Nodes.Generic;
using BossesModule.Worm.Nodes;
using UnityEngine;

namespace BossesModule.Worm
{
    public class WormTree : MonoBehaviour, IVisualizable
    {
        public const string CURRENT_TARGET = "currentTarget";
        private const string IS_REPOSITIONING = "isRepositioning";

        #region Fields

        [Header("Fields")]
        [SerializeField]
        private Animator animator;

        [SerializeField]
        private Collider _collider;

        [SerializeField]
        private WormEntity _entity;
       
        #endregion

        #region IVisualizable

        private Node _root;


        /// <inheritdoc/>
        public Node GetRoot() => this._root;

        /// <inheritdoc/>
        public void RebuildRoot()
        {
            Selector root = new();

            //this._attackNode = new AttackNode(this.transform, this.animator, this._collider, CURRENT_TARGET);
            //root += this._attackNode;

            this._escapeNode = new EscapeNode(this._entity, this.animator, this._collider, CURRENT_TARGET, IS_REPOSITIONING);
            root += this._escapeNode;

            //this._repositionNode = new RepositionNode(this.transform, this.animator, this._collider, CURRENT_TARGET, IS_REPOSITIONING);
            //root += this._repositionNode;

            root += this.Rotate();

            root.SetData(CURRENT_TARGET, null);

            this._root = root.Alias("Root");
        }

        #endregion

        #region Attack

        private AttackNode _attackNode;

        #endregion

        #region Escape

        private EscapeNode _escapeNode;

        public void OnEscapeStartEnded() => this._escapeNode.OnStartAnimationEnded();
        public void OnEscapeEndEnded() => this._escapeNode.OnEndAnimationEnded();

        #endregion

        #region Reposition

        private RepositionNode _repositionNode;

        public void OnRepositionStartEnded() => this._repositionNode.OnStartAnimationEnded();
        public void OnRepositionEndEnded() => this._repositionNode.OnEndAnimationEnded();

        #endregion

        #region Rotate

        private Node Rotate() => new CallbackNode(RotateTowardsTarget).Alias("Rotate");

        private NodeState RotateTowardsTarget(Node n)
        {
            Transform target = n.GetData<Transform>(CURRENT_TARGET);

            // If target is invalid, return fail
            if (target == null)
                return NodeState.FAILURE;

            Vector3 targetPosition = new(
                target.position.x - this.transform.position.x,
                this.transform.position.y,
                target.position.z - this.transform.position.z
            );

            // Rotate self towards target
            var targetRotation = Quaternion.LookRotation(targetPosition);
            this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, targetRotation, 45 * Time.deltaTime);

            return NodeState.SUCCESS;
        }

        #endregion

    }
}

