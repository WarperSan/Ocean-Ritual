using BehaviourModule.Nodes;
using BehaviourModule.Nodes.Generic;
using BehaviourModule.Nodes.Controls;
using UnityEngine;
using BehaviourModule.Interfaces;
using EntityModule;

namespace BossesModule.Golem
{
    public class GolemTree : MonoBehaviour, IVisualizable
    {
        public const string CURRENT_TARGET = "currentTarget";
        public const string WALK_SPEED = "walkSpeed";

        public Transform target;

        #region IVisualizable

        private Node root;

        /// <inheritdoc/>
        public Node GetRoot() => this.root;

        /// <inheritdoc/>
        public void RebuildRoot()
        {
            Selector _root = new();
            _root += this.AttackSequence();
            _root += new Parallel(
                this.RotateSequence(),
                this.WalkSequence()
            );

            _root.SetData(CURRENT_TARGET, this.target);
            _root.SetData(WALK_SPEED, 1f);

            this.golemAnimationEvents.throwTarget = this.target;

            this.root = _root.Alias("Root");
        }

        #endregion

        #region Animation

        [Header("Animation")]
        [SerializeField]
        private Animator animator;

        [SerializeField]
        private GolemAnimationEvents golemAnimationEvents;

        #endregion

        #region Attack

        public enum GolemAttackFlags
        {
            None = 0,
            Throw = 1,
            Stomp = 2,
            RageThrow = 3,
        }

        private float attackCooldown;

        private Node AttackSequence()
        {
            Sequence attackSequence = new();

            attackSequence += this.AttackCooldown();
            attackSequence += this.CancelWalk();

            //attackSequence += this.RageThrow();
            //attackSequence += this.Throw(CURRENT_TARGET, this.throwMinRange, this.throwMaxRange);
            //attackSequence += new CallbackNode(n => SetAttack(this.animator, GolemAttackFlags.None), NodeState.FAILURE);

            attackSequence += this.ThrowSequence();

            Sequence attackReset = new();
            attackReset += this.SetAttackCooldown();
            attackReset += new CallbackNode(_ => this.throwAnim.ResetAnim(), NodeState.SUCCESS).Alias("Reset Attack Animation");

            attackSequence += attackReset.Alias("Attack Reset");

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

        private void SetAttack(GolemAttackFlags attack)
        {
            this.animator.SetInteger("Attack", (int)attack);
            this.animator.SetBool("IsAttacking", attack != GolemAttackFlags.None);
        }

        #endregion

        #region Throw

        [Header("Throw")]
        private AnimationNode throwAnim;
        public float throwMinRange;
        public float throwMaxRange;

        public ObjectPool throwPool;

        private Node ThrowSequence()
        {
            this.throwAnim = new AnimationNode(() => this.SetAttack(GolemAttackFlags.Throw));

            Sequence throwSequence = new();
            throwSequence += new DistanceInBetween(this.transform, CURRENT_TARGET, this.throwMinRange, this.throwMaxRange);
            throwSequence += this.throwAnim.Alias("Throw Animation");

            return throwSequence.Alias("Throw Sequence");
        }

        public void ThrowEnded()
        {
            this.throwAnim.OnEnded();
            this.SetAttack(GolemAttackFlags.None);
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

        #region Walk

        [Header("Walk")]
        public float walkMinRange;
        public float walkMaxRange;

        private Node WalkSequence()
        {
            Selector walkSequence = new();

            Sequence tryWalk = new();
            tryWalk += new DistanceInBetween(this.transform, CURRENT_TARGET, this.walkMinRange, this.walkMaxRange);
            tryWalk += new CallbackNode(this.WalkToTarget);
            tryWalk += new CallbackNode(_ => this.SetWalking(true), NodeState.SUCCESS).Alias("Set to Walking");

            walkSequence += tryWalk.Alias("Try Walk");
            walkSequence += this.CancelWalk();

            return walkSequence.Alias("Walk Sequence");
        }

        private NodeState WalkToTarget(Node n)
        {
            Transform target = n.GetData<Transform>(CURRENT_TARGET);

            // If target is invalid, return fail
            if (target == null)
                return NodeState.FAILURE;

            // Move self towards target
            float speed = n.GetData<float>(WALK_SPEED);
            Vector3 direction = (target.position - this.transform.position).normalized;
            this.transform.Translate(direction * speed * Time.deltaTime, Space.World);
            this.SetWalking(true);

            return NodeState.RUNNING;
        }

        private void SetWalking(bool IsWalking) => this.animator.SetBool("IsWalking", IsWalking);

        private Node CancelWalk()
        {
            CallbackNode cancelWalk = new(n => this.SetWalking(false), NodeState.SUCCESS);

            return cancelWalk.Alias("Cancel Walk");
        }

        #endregion

        #region Gizmos
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            UnityEditor.Handles.color = Color.red;
            UnityEditor.Handles.DrawWireDisc(this.transform.position, this.transform.up, this.throwMinRange);
            UnityEditor.Handles.DrawWireDisc(this.transform.position, this.transform.up, this.throwMaxRange);

            UnityEditor.Handles.color = Color.blue;
            UnityEditor.Handles.DrawWireDisc(this.transform.position, this.transform.up, this.walkMinRange);
            UnityEditor.Handles.DrawWireDisc(this.transform.position, this.transform.up, this.walkMaxRange);
        }
#endif
        #endregion
    }
}

