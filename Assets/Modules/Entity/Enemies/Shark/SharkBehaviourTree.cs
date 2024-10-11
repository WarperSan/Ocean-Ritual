using BehaviourModule.Interfaces;
using BehaviourModule.Nodes;
using BehaviourModule.Nodes.Controls;
using BehaviourModule.Nodes.Generic;
using UnityEngine;

namespace EntityModule.Enemies
{
    public class SharkBehaviourTree : MonoBehaviour, IVisualizable
    {
        public const string CURRENT_TARGET = "currentTarget";
        public const string WALK_SPEED = "walkSpeed";
        public Transform target;

        /// <inheritdoc/>
        private void Start() => this.RebuildRoot();

        #region IVisualizable

        protected Node root;

        /// <inheritdoc/>
        public Node GetRoot() => this.root;

        /// <inheritdoc/>
        public void RebuildRoot() => this.root = this.SetUpTree();

        private Node SetUpTree()
        {
            // Attack when in range
            // Delay between strikes
            // Rushes towards the player and attacks it

            Sequence _root = new();
            //_root += this.AttackSequence();
            _root += this.RotateSequence();
            //_root += new Parallel(
            //    this.RotateSequence(),
            //    this.MovementSequence()
            //);

            _root.SetData(CURRENT_TARGET, this.target);

            return _root;
        }

        #endregion

        #region Attack
        private float attackCooldown;
        public float attackMinRange;
        public float attackMaxRange;
        private Node AttackSequence()
        {
            Sequence attackSequence = new();
            
            attackSequence += new DistanceInBetween(this.transform, CURRENT_TARGET,attackMinRange,attackMaxRange);

            Sequence attack = new();
            attack += this.AttackCooldown();

            attackSequence += attack;
            return attackSequence.Alias("Attack Sequence");
        }

        private Node AttackCooldown() => new CallbackNode(() =>
        {
            this.attackCooldown -= Time.deltaTime;

            return this.attackCooldown > 0 ? NodeState.FAILURE : NodeState.SUCCESS;
        }).Alias("Attack Cooldown");
        private Node SetAttackCooldown() => new CallbackNode(() =>
        {
            this.attackCooldown = Random.Range(5, 10);

            return NodeState.SUCCESS;
        }).Alias("Reset Attack Cooldown");


        #endregion

        #region Movement

        private Node MovementSequence()
        {
            Sequence movementSequence = new();

            return movementSequence.Alias("Movement Sequence");
        }
        #endregion

        #region Rotation
        private Node RotateSequence()
        {
            Sequence rotateSequence = new();
            rotateSequence += new CallbackNode(this.RotateTowardsTarget).Alias("Rotate towards target");

            return rotateSequence.Alias("Rotate Sequence");
        }

        private NodeState RotateTowardsTarget(Node n)
        {
            Transform target = n.GetData<Transform>(CURRENT_TARGET);
            Debug.Log("Rotate");
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