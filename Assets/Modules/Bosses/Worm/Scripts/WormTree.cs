using BehaviourModule.Interfaces;
using BehaviourModule.Nodes;
using BehaviourModule.Nodes.Controls;
using BehaviourModule.Nodes.Generic;
using ExtensionsModule;
using UnityEngine;

namespace BossesModule.Worm
{
    public class WormTree : MonoBehaviour, IVisualizable
    {
        public const string CURRENT_TARGET = "currentTarget";

        #region Fields

        [Header("Fields")]
        [SerializeField]
        private Animator animator;

        [SerializeField]
        private Collider _collider;

        #endregion

        #region IVisualizable

        private Node _root;

        /// <inheritdoc/>
        public Node GetRoot() => this._root;

        /// <inheritdoc/>
        public void RebuildRoot()
        {
            Selector root = new();
            root += this.Rotate();

            root.SetData(CURRENT_TARGET, null);

            this._root = root.Alias("Root");
        }

        #endregion

        #region Rotate

        private Node Rotate()
        {
            CallbackNode rotate = new CallbackNode(RotateTowardsTarget);

            return rotate.Alias("Rotate");
        }

        private NodeState RotateTowardsTarget()
        {
            Transform target = this._root.GetData<Transform>(CURRENT_TARGET);

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

