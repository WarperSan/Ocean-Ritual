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

        #region IVisualizable

        private Node root;

        /// <inheritdoc/>
        public Node GetRoot() => root;

        /// <inheritdoc/>
        public void RebuildRoot()
        {
            Selector _root = new();
            _root += AttackSequence();

            _root += new Parallel(
                RotateSequence(),
                WalkSequence()
            );

            _root.SetData(CURRENT_TARGET, null);
            _root.SetData(WALK_SPEED, 3f);

            root = _root.Alias("Root");

            //TargetGeneral.Instance.Target
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

            attackSequence += AttackCooldown();
            attackSequence += CancelWalk();

            //attackSequence += this.RageThrow();
            //attackSequence += this.Throw(CURRENT_TARGET, this.throwMinRange, this.throwMaxRange);
            //attackSequence += new CallbackNode(n => SetAttack(this.animator, GolemAttackFlags.None), NodeState.FAILURE);

            attackSequence += ThrowSequence();

            Sequence attackReset = new();
            attackReset += SetAttackCooldown();
            attackReset += new CallbackNode(_ => throwAnim.ResetAnim(), NodeState.SUCCESS).Alias("Reset Attack Animation");

            attackSequence += attackReset.Alias("Attack Reset");

            return attackSequence.Alias("Attack Sequence");
        }

        private Node AttackCooldown() => new CallbackNode(() =>
        {
            attackCooldown -= Time.deltaTime;

            return attackCooldown > 0 ? NodeState.FAILURE : NodeState.SUCCESS;
        }).Alias("Attack Cooldown");

        private Node SetAttackCooldown() => new CallbackNode(() =>
        {
            attackCooldown = Random.Range(5, 10);

            return NodeState.SUCCESS;
        }).Alias("Reset Attack Cooldown");

        private void SetAttack(GolemAttackFlags attack)
        {
            animator.SetInteger("Attack", (int)attack);
            animator.SetBool("IsAttacking", attack != GolemAttackFlags.None);
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
            throwAnim = new AnimationNode(() => SetAttack(GolemAttackFlags.Throw));

            Sequence throwSequence = new();

            throwSequence += new DistanceInBetween(transform,
                CURRENT_TARGET,
                throwMinRange,
                throwMaxRange);
            throwSequence += throwAnim.Alias("Throw Animation");

            return throwSequence.Alias("Throw Sequence");
        }

        public void ThrowEnded()
        {
            throwAnim.OnEnded();
            SetAttack(GolemAttackFlags.None);
        }

        #endregion

        #region Rotate

        private Node RotateSequence()
        {
            Sequence rotateSequence = new();
            rotateSequence += new CallbackNode(RotateTowardsTarget).Alias("Rotate towards target");

            return rotateSequence.Alias("Rotate Sequence");
        }

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

        #region Walk

        [Header("Walk")]
        public float walkMinRange;

        public float walkMaxRange;

        private Node WalkSequence()
        {
            Selector walkSequence = new();

            Sequence tryWalk = new();

            tryWalk += new DistanceInBetween(transform,
                CURRENT_TARGET,
                walkMinRange,
                walkMaxRange);
            tryWalk += new CallbackNode(WalkToTarget);
            tryWalk += new CallbackNode(_ => SetWalking(true), NodeState.SUCCESS).Alias("Set to Walking");

            walkSequence += tryWalk.Alias("Try Walk");
            walkSequence += CancelWalk();

            return walkSequence.Alias("Walk Sequence");
        }

        private NodeState WalkToTarget(Node n)
        {
            Transform target = n.GetData<Transform>(CURRENT_TARGET);

            // If target is invalid, return fail
            if (target == null)
                return NodeState.FAILURE;

            Vector3 pos = target.position;
            pos.y = transform.position.y; // Walk straight

            // Move self towards target
            float speed = n.GetData<float>(WALK_SPEED);
            Vector3 direction = (pos - transform.position).normalized;
            transform.Translate(speed * Time.deltaTime * direction, Space.World);
            SetWalking(true);

            return NodeState.RUNNING;
        }

        private void SetWalking(bool IsWalking) => animator.SetBool("IsWalking", IsWalking);

        private Node CancelWalk()
        {
            CallbackNode cancelWalk = new(n => SetWalking(false), NodeState.SUCCESS);

            return cancelWalk.Alias("Cancel Walk");
        }

        #endregion

        #region Gizmos

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            UnityEditor.Handles.color = Color.red;
            UnityEditor.Handles.DrawWireDisc(transform.position, transform.up, throwMinRange);
            UnityEditor.Handles.DrawWireDisc(transform.position, transform.up, throwMaxRange);

            UnityEditor.Handles.color = Color.blue;
            UnityEditor.Handles.DrawWireDisc(transform.position, transform.up, walkMinRange);
            UnityEditor.Handles.DrawWireDisc(transform.position, transform.up, walkMaxRange);
        }
        #endif

        #endregion
    }
}