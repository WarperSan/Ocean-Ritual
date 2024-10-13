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


  
    public class Cooldown : Node
    {

        float TimeBetwenneAttack;
        float timelLapse;
        public Cooldown(float TimeBetwenneAttack)
        {
            this.TimeBetwenneAttack = TimeBetwenneAttack;
        }
        protected override NodeState OnEvaluate()
        {


            return NodeState.SUCCESS;
        }




        public override string GetText() => "Cooldown";

    }



    public class AnimationAttack : Node
    {
        float animationTime;
        float animationVitesse;
        float timeLapse;
        Transform target;
        float initialYRotation;  // Pour sauvegarder la rotation de départ sur l'axe Y

        public AnimationAttack(float animationTime, float animationVitesse, Transform target)
        {
            this.animationTime = animationTime;
            this.animationVitesse = animationVitesse;
            this.target = target;
            this.timeLapse = 0f;
            this.initialYRotation = target.eulerAngles.y; // Sauvegarde de la rotation Y initiale
        }

        protected override NodeState OnEvaluate()
        {
            if (timeLapse < animationTime)
            {
                DoAnimation();
                return NodeState.RUNNING; // L'animation est en cours
            }
            else
            {
                ResetAnimation(); // Réinitialisation après l'animation
                return NodeState.SUCCESS; // Animation terminée
            }
        }

        public void DoAnimation()
        {
            // Rotation pendant le temps défini
            if (timeLapse < animationTime)
            {
                // Calcul de l'angle de rotation actuel
                float rotationAmount = animationVitesse * (timeLapse / animationTime) * 360f; // 360° sur z
                target.rotation = Quaternion.Euler(-90, 0, initialYRotation + rotationAmount); // On fixe manuellement la rotation z
                timeLapse += Time.deltaTime;
            }
        }

        public void ResetAnimation()
        {
            // Remet la rotation Y à zéro (ou à la rotation initiale)
            target.rotation = Quaternion.Euler(0, initialYRotation, 0);
            timeLapse = 0f;
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
        public const string NeedFolow = "needFolow";
        protected override NodeState OnEvaluate()
        {
            Transform target = GetData<Transform>(CURRENT_TARGET);
            NavMeshAgent agent = GetData<NavMeshAgent>(AGENT);
            bool needFolow = GetData<bool>(NeedFolow);

            // Vérifier si la cible ou l'agent sont null
            if (target == null || agent == null)
            {
                Debug.Log("echec");
                return NodeState.FAILURE; // Retourne échec s'il n'y a pas de cible ou d'agent
            }
            // Si needFollow est false, arrêter le mouvement de l'agent
            if (!needFolow)
            {
                agent.ResetPath(); // Annule toute destination en cours
                Debug.Log("Arrêt du suivi de la cible");
                return NodeState.RUNNING; // Retourne FAILURE car le suivi est stoppé
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
