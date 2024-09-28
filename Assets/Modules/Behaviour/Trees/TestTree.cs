using BehaviourModule.Nodes;
using BehaviourModule.Nodes.Generic;
using BehaviourModule.Nodes.Controls;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using BehaviourModule.Interfaces;

namespace BehaviourModule.Trees
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class TestTree : MonoBehaviour, IVisualizable
    {
        [SerializeField]
        private Transform target;

        [SerializeField]
        private Transform[] destinations;

        [SerializeField]
        private float detectionRange = 3f;

        private Node root;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Handles.color = Color.red;
            Handles.DrawWireDisc(this.transform.position, Vector3.up, this.detectionRange, 5);
        }
#endif

        #region IVisualizable

        /// <inheritdoc/>
        public Node GetRoot() => this.root;

        /// <inheritdoc/>
        public void RebuildRoot()
        {
            NavMeshAgent agent = this.GetComponent<NavMeshAgent>();

            var root = new Selector();
            root += new ChaseNode(
                agent,
                this.target,
                this.detectionRange
            );
            root += new PatrolNode(
                agent,
                this.destinations
            );

            this.root = root;
        }

        private void Start()
        {
            this.RebuildRoot();
        }

        private void Update()
        {
            this.root.Evaluate();
        }

        #endregion
    }

    public class GoToTarget : Node
    {
        private readonly NavMeshAgent agent;
        private readonly string target;

        public GoToTarget(NavMeshAgent agent, string target)
        {
            this.agent = agent;
            this.target = target;
        }

        /// <inheritdoc/>
        protected override NodeState OnEvaluate()
        {
            this.agent.destination = this.GetData<Transform>(this.target).position;

            if (this.agent.remainingDistance <= this.agent.stoppingDistance)
                return NodeState.SUCCESS;

            return NodeState.RUNNING;
        }
    }

    public class Delay : Node
    {
        private float delayAmount;
        private float remaining;

        public Delay(float amount)
        {
            this.delayAmount = amount;
            this.remaining = amount;
        }

        /// <inheritdoc/>
        protected override NodeState OnEvaluate()
        {
            this.remaining -= Time.deltaTime;

            if (this.remaining <= 0)
            {
                this.remaining = this.delayAmount;
                return NodeState.SUCCESS;
            }

            return NodeState.RUNNING;
        }
    }

    public class PatrolNode : Sequence
    {
        private readonly Transform[] targets;
        private int currentIndex;

        const string CURRENT_TARGET = "currentTarget";

        public override bool IsAutomaticallyHidden() => true;

        public PatrolNode(NavMeshAgent agent, Transform[] targets)
        {
            // Aller à la prochaine destination
            this.Attach(
                new GoToTarget(agent, CURRENT_TARGET)
            );

            // Attendre un certain temps
            this.Attach(new Delay(5));

            // Changer la destination
            this.Attach(new CallbackNode(this.ChangeTarget));

            // Set data
            this.SetData(CURRENT_TARGET, targets[0]);
            this.targets = targets;
        }

        private NodeState ChangeTarget()
        {
            this.currentIndex++;

            if (this.currentIndex >= this.targets.Length)
                this.currentIndex = 0;

            this.SetData(CURRENT_TARGET, this.targets[this.currentIndex]);

            return NodeState.SUCCESS;
        }

        /// <inheritdoc/>
        public override string GetText() => "Patrol";
    }

    public class ChaseNode : Sequence
    {
        const string TARGET = "target";

        public ChaseNode(NavMeshAgent agent, Transform target, float range)
        {
            this.Attach(
                new DistanceSmaller(agent.transform, TARGET, range)
            );

            this.Attach(
                new GoToTarget(agent, TARGET)
            );

            // Set data
            this.SetData(TARGET, target);
        }

        /// <inheritdoc/>
        public override string GetText() => "Chase";
    }
}