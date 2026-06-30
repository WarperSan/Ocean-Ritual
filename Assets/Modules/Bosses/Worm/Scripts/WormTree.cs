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
        private const string IS_ESCAPING = "isEscaping";
        private const string IS_REPOSITIONING = "isRepositioning";
        private const string IS_ATTACKING = "isAttacking";

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
        public Node GetRoot() => _root;

        /// <inheritdoc/>
        public void RebuildRoot()
        {
            Selector root = new();

            _attackNode = new AttackSequence(this,
                _entity,
                animator,
                _collider,
                CURRENT_TARGET,
                IS_ATTACKING);
            root += _attackNode;

            _escapeNode = new EscapeNode(_entity,
                animator,
                _collider,
                CURRENT_TARGET,
                IS_ESCAPING,
                IS_REPOSITIONING);
            root += _escapeNode;

            _repositionNode = new RepositionNode(_entity,
                animator,
                _collider,
                CURRENT_TARGET,
                IS_REPOSITIONING);
            root += _repositionNode;

            root += Rotate();

            root.SetData(CURRENT_TARGET, null);

            _root = root.Alias("Root");
        }

        public bool IsMoving()
        {
            if (_root == null)
                return false;

            return _root.GetData<bool>(IS_ESCAPING) || _root.GetData<bool>(IS_REPOSITIONING);
        }

        #endregion

        #region Attack

        private AttackSequence _attackNode;

        public void OnAttackEnded() => _attackNode.OnAnimationEnded();

        public void IceWaveStart() => _entity.StartIceWave();

        #endregion

        #region Escape

        private EscapeNode _escapeNode;

        public void OnEscapeStartEnded() => _escapeNode.OnStartAnimationEnded();
        public void OnEscapeEndEnded()   => _escapeNode.OnEndAnimationEnded();

        #endregion

        #region Reposition

        private RepositionNode _repositionNode;

        public void OnRepositionStartEnded() => _repositionNode.OnStartAnimationEnded();
        public void OnRepositionEndEnded()   => _repositionNode.OnEndAnimationEnded();

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
                target.position.x - transform.position.x,
                transform.position.y,
                target.position.z - transform.position.z
            );

            // Rotate self towards target
            var targetRotation = Quaternion.LookRotation(targetPosition);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 45 * Time.deltaTime);

            return NodeState.SUCCESS;
        }

        #endregion
    }
}