using BehaviourModule.Interfaces;
using BehaviourModule.Nodes;
using BehaviourModule.Nodes.Controls;
using BehaviourModule.Nodes.Generic;
using ExtensionsModule;
using UnityEngine;

namespace BossesModule.Worm
{
    public class WormTree1 : MonoBehaviour, IVisualizable
    {
        public const string CURRENT_TARGET = "currentTarget";
        public const string CURRENT_WALK_TARGET = "currentWalkTarget";
        public const string WALK_SPEED = "walkSpeed";
        public const string REQUEST_DIVE = "requestDive";

        #region Fields

        [Header("Fields")]
        [SerializeField]
        private Animator animator;

        [SerializeField]
        private Collider _collider;

        #endregion

        #region IVisualizable

        private Node root;

        /// <inheritdoc/>
        public Node GetRoot() => this.root;

        /// <inheritdoc/>
        public void RebuildRoot()
        {
            Selector _root = new();
            _root += this.AttackSequence();
            _root += this.DiveSequence();
            _root += new Parallel(
                this.RotateSequence()
            );

            _root.SetData(CURRENT_TARGET, null);
            _root.SetData(CURRENT_WALK_TARGET, this.transform.position);
            _root.SetData(REQUEST_DIVE, false);
            _root.SetData(WALK_SPEED, 5f);

            this.root = _root.Alias("Root");

            //TargetGeneral.Instance.Target
        }

        #endregion

        #region Attack

        private Node AttackSequence()
        {
            Sequence root = new();

            root += new CallbackNode((Node n) =>
            {
                Transform target = n.GetData<Transform>(CURRENT_TARGET);

                if (target == null)
                    return NodeState.FAILURE;

                bool diveRequested = n.GetData<bool>(REQUEST_DIVE);

                if (diveRequested)
                    return NodeState.SUCCESS;

                if (this.transform.Distance(target) > 40)
                    return NodeState.FAILURE;

                this.root.SetData(REQUEST_DIVE, true);
                return NodeState.SUCCESS;
            }).Alias("< TEMP >"); // TEMP
            root += new CallbackNode(() => NodeState.FAILURE).Alias("ALWAYS FAILURE");

            return root.Alias("Attack Sequence");
        }

        #endregion

        #region Walk

        private AnimationNode diveBackNode;
        private AnimationNode emergeNode;

        private Node DiveSequence()
        {
            Sequence root = new();

            // Check if requested dive
            root += new CallbackNode((Node n) => n.GetData<bool>(REQUEST_DIVE) ? NodeState.SUCCESS : NodeState.FAILURE).Alias("Dive requested?");

            // Play diving animation
            this.diveBackNode = new AnimationNode(this.DiveBack);
            root += this.diveBackNode.Alias("Dive");

            // Walk towards target
            root += new CallbackNode(this.WalkToTarget);

            // Play emerge animation
            this.emergeNode = new AnimationNode(this.Emerge);
            root += this.emergeNode.Alias("Emerge");

            // Clear dive request
            root += new CallbackNode(this.ClearWalk);

            return root.Alias("Walk Sequence");
        }

        private void DiveBack()
        {
            this.animator.SetBool("isUnderwater", true);

            // Disable collider
            this._collider.enabled = false;

            // PICK RANDOM LOCATION
            this.root.SetData(CURRENT_WALK_TARGET, TargetGeneral.Instance.BoatTarget.position);
        }

        private void Emerge()
        {
            this.animator.SetBool("isUnderwater", false);

            // Enable collider
            this._collider.enabled = true;
        }

        private NodeState ClearWalk()
        {
            this.root.SetData(REQUEST_DIVE, false);
            this.diveBackNode.ResetAnim();
            this.emergeNode.ResetAnim();

            return NodeState.SUCCESS;
        }

        public void OnDiveEnded() => this.diveBackNode.OnEnded();
        public void OnEmergeEnded() => this.emergeNode.OnEnded();

        private NodeState WalkToTarget(Node n)
        {
            Vector3 pos = n.GetData<Vector3>(CURRENT_WALK_TARGET);
            pos.y = this.transform.position.y; // Walk straight

            // Move self towards target
            float speed = n.GetData<float>(WALK_SPEED);
            Vector3 direction = (pos - this.transform.position).normalized;

            float stepSize = speed * Time.deltaTime;

            // If close enough, snap
            if (Vector3.Distance(pos, this.transform.position) < stepSize)
            {
                this.transform.position = pos;
                return NodeState.SUCCESS;
            }

            // Move towards
            this.transform.Translate(stepSize * direction, Space.World);
            return NodeState.RUNNING;
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

