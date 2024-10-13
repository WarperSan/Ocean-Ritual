using BehaviourModule.Nodes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



namespace TortueNode
{


    public  class  Heals : Node
    {


        protected override NodeState OnEvaluate()
        {
           

            return NodeState.SUCCESS;
        }




        public override string GetText() => "Heal";

    }





    public class PeopleToHeals : Node
    {


        protected override NodeState OnEvaluate()
        {


            return NodeState.SUCCESS;
        }




        public override string GetText() => "PeopleToHeal";

    }


    public class ProxiBoat : Node
    {

        public const string CURRENT_TARGET = "currentTarget";
      
        protected override NodeState OnEvaluate()
        {
            Transform target = GetData<Transform>(CURRENT_TARGET);

            return NodeState.SUCCESS;
        }




        public override string GetText() => "ProxiBoat";

    }
    public class Cooldown : Node
    {


        protected override NodeState OnEvaluate()
        {


            return NodeState.SUCCESS;
        }




        public override string GetText() => "Cooldown";

    }



    public class AnimationAttack : Node
    {


        protected override NodeState OnEvaluate()
        {


            return NodeState.SUCCESS;
        }




        public override string GetText() => "AnimationAttack";

    }
    public class Attacks : Node
    {


        protected override NodeState OnEvaluate()
        {


            return NodeState.SUCCESS;
        }




        public override string GetText() => "Attacks";

    }

    public class AnnimationMovement : Node
    {


        protected override NodeState OnEvaluate()
        {


            return NodeState.SUCCESS;
        }




        public override string GetText() => "AnnimationMovement";

    }


    public class FollowTarget : Node
    {
        public const string AGENT = "agent";
        public const string CURRENT_TARGET = "currentTarget";

        protected override NodeState OnEvaluate()
        {
            Transform target = GetData<Transform>(CURRENT_TARGET);
            NavMeshAgent agent = GetData<NavMeshAgent>(AGENT);
           
            // Vérifier si la cible ou l'agent sont null
            if (target == null || agent == null)
            {
                Debug.Log("echec");
                return NodeState.FAILURE; // Retourne échec s'il n'y a pas de cible ou d'agent
            }

            // Définir la destination de l'agent sur la position de la cible
            agent.SetDestination(target.position);
          
         
            // Vérifier si l'agent est arrivé à destination
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
              //  Debug.Log("succe");
                return NodeState.SUCCESS; // Retourne succès si l'agent est arrivé
                
            }
           // Debug.Log("en cour");
            return NodeState.RUNNING; // Retourne en cours si l'agent est encore en mouvement
        }




        public override string GetText() => "FollowTarget";

    }




}
