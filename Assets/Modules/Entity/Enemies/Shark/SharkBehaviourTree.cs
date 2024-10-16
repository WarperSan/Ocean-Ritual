using BehaviourModule.Interfaces;
using BehaviourModule.Nodes;
using BehaviourModule.Nodes.Controls;
using BehaviourModule.Nodes.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace EntityModule.Enemies
{
    public class SharkBehaviourTree : MonoBehaviour, IVisualizable
    {
        public const string CURRENT_TARGET = "currentTarget";
        public const string WALK_SPEED = "walkSpeed";
        public Transform target;
        public NavMeshAgent agent;

        #region IVisualizable

        protected Node root;

        /// <inheritdoc/>
        public Node GetRoot() => this.root;

        /// <inheritdoc/>
        public void RebuildRoot()
        {
            // Attack when in range
            // Delay between strikes
            // Rushes towards the player and attacks it

            Sequence _root = new();
            
            _root += this.MovementSequence();
            _root += new Parallel(
                  this.AttackSequence()
            );

            _root.SetData(AGENT, this.agent);
            _root.SetData(CURRENT_TARGET, this.target);

            this.root = _root;
        }

        #endregion

        #region Attack
        private float attackCooldown =5;
        public float attackMinRange;
        public float attackMaxRange;
        public Collider hitboxCollider;
        public float attackDuration;
        private float durationTimer = 0;

        private Node AttackSequence()
        {
            Sequence attackSequence = new();
            
            Sequence attack = new();
            
            attack += new DistanceInBetween(this.transform, CURRENT_TARGET,attackMinRange,attackMaxRange);
            //attackSequence += this.AttackCooldown();


            attack += this.DoAttack();
            //faire l'attaque + animation
            attackSequence += attack;


            Sequence attackReset = new();
            //attackReset += this.SetAttackCooldown();
            attackReset += this.ResetHitbox();
            attackReset += this.AttackCooldown();
            attackReset += this.SetAttackCooldown();
            attackSequence += attackReset.Alias("Attack Reset");
            //reset animation

            return attackSequence.Alias("Attack Sequence");
        }

        private Node DoAttack() => new CallbackNode(() =>
        {
            if (!hitboxCollider.enabled)
            {
                hitboxCollider.enabled = true;
            }
            durationTimer += Time.deltaTime;
            
            if (durationTimer < attackDuration) 
            {
                return NodeState.RUNNING;
            }
            
            return NodeState.SUCCESS;
        }).Alias("Do Attack");

        private Node ResetHitbox() => new CallbackNode(() =>
        {
            hitboxCollider.enabled= false;
            
            return NodeState.SUCCESS;
        }).Alias("Reset Hitbox");

        private Node AttackCooldown() => new CallbackNode(() =>
        {
            this.attackCooldown -= Time.deltaTime;

            return this.attackCooldown > 0 ? NodeState.RUNNING : NodeState.SUCCESS;
        }).Alias("Attack Cooldown");
        private Node SetAttackCooldown() => new CallbackNode(() =>
        {
            this.attackCooldown = 5;
            durationTimer = 0;
            
            return NodeState.SUCCESS;
        }).Alias("Reset Attack Cooldown");


        #endregion

        #region Movement

        private Node MovementSequence()
        {
            Sequence movementSequence = new();
            movementSequence += this.Move();
            return movementSequence.Alias("Movement Sequence");
        }


        public const string AGENT = "agent";


        private Node Move() => new CallbackNode(() =>
        {
            Transform target = root.GetData<Transform>(CURRENT_TARGET);
            NavMeshAgent agent = root.GetData<NavMeshAgent>(AGENT);
            
            // V�rifier si la cible ou l'agent sont null
            if (target == null || agent == null)
            {
                Debug.Log("echec");
                return NodeState.FAILURE; // Retourne �chec s'il n'y a pas de cible ou d'agent
            }

            // D�finir la destination de l'agent sur la position de la cible
            agent.SetDestination(target.position);


            // V�rifier si l'agent est arriv� � destination
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                //  Debug.Log("succe");
                return NodeState.SUCCESS; // Retourne succ�s si l'agent est arriv�

            }
            // Debug.Log("en cour");
            return NodeState.SUCCESS; // Retourne en cours si l'agent est encore en mouvement
        }).Alias("Movement");

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


        private void OnDrawGizmos()
        {
            UnityEditor.Handles.color = Color.blue;
            UnityEditor.Handles.DrawWireDisc(this.transform.position, this.transform.up, this.attackMinRange);
            UnityEditor.Handles.DrawWireDisc(this.transform.position, this.transform.up, this.attackMaxRange);
        }

        //private void OnTriggerEnter(Collider other)
        //{
        //    UnityEngine.Debug.Log("touched");
        //    if (other.tag == "Player")
        //    {
        //        Debug.Log("Hit Player");
        //    }
        //}

    }
}