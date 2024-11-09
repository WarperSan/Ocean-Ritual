using BehaviourModule.Interfaces;
using BehaviourModule.Nodes;
using BehaviourModule.Nodes.Controls;
using BehaviourModule.Nodes.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossesModule.Worm
{
    public class WormTree : MonoBehaviour, IVisualizable
    {
        public const string CURRENT_TARGET = "currentTarget";

        #region IVisualizable

        private Node root;

        /// <inheritdoc/>
        public Node GetRoot() => this.root;

        /// <inheritdoc/>
        public void RebuildRoot()
        {
            Selector _root = new();
            _root += new Parallel(
                this.RotateSequence()
            );

            _root.SetData(CURRENT_TARGET, null);

            this.root = _root.Alias("Root");

            //TargetGeneral.Instance.Target
        }

        #endregion

        #region Rotate

        private Node RotateSequence()
        {
            Sequence rotateSequence = new();
            rotateSequence += new CallbackNode(this.RotateTowardsTarget).Alias("Rotate towards target");

            return rotateSequence.Alias("Rotate Sequence");
        }

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

