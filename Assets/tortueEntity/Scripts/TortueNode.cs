using BehaviourModule.Nodes;
using BehaviourModule.Nodes.Controls;
using BehaviourModule.Nodes.Generic;
using EntityModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Unity.VisualScripting.Metadata;



namespace TortueNode
{

    public class Heals : Node
    {
        private float RayonToHeal;
        private float HealPower;
        private Transform healerTransform; // Position à partir de laquelle on effectue la recherche
        Heal heal = new Heal();
        // Constructeur
        public Heals(float RayonToHeal, float HealPower, Transform healerTransform)
        {
            this.RayonToHeal = RayonToHeal;
            this.HealPower = HealPower;
            this.healerTransform = healerTransform;
            heal.Amount = HealPower;

        }

        // Méthode principale du node
        protected override NodeState OnEvaluate()
        {
            NodeState state;
            List<Entity> entitiesToHeal = new();
            // List<Entity> entitiesToHeal = FindPeopleToHeal();
            foreach (Node node in children)
            {
                state = node.Evaluate();
                if (state == NodeState.RUNNING)
                {
                    return state;
                }
            }
            if (entitiesToHeal.Count > 0)
            {

                // Logique de soin
                foreach (Entity entity in entitiesToHeal)
                {

                    entity.UseHeal(heal); // Par exemple, une méthode 'Heal' qui augmente la santé
                }

                return NodeState.SUCCESS;
            }

            return NodeState.SUCCESS;
        }

        // Méthode pour trouver les entités dans un rayon
        private List<Entity> FindPeopleToHeal()
        {
            List<Entity> entitiesToHeal = new List<Entity>();

            // Utilisation d'un LayerMask pour ne chercher que les ennemis
            int enemyLayerMask = LayerMask.GetMask("Enemy");

            // On vérifie que le healerTransform est bien initialisé avant d'appeler OverlapSphere
            if (healerTransform == null)
            {
                Debug.LogError("Healer Transform is not assigned!");
                return entitiesToHeal;
            }

            // Exécution de la détection dans un rayon avec filtrage par layer
            Collider[] hitColliders = Physics.OverlapSphere(healerTransform.position, RayonToHeal);

            // Parcours des objets détectés
            foreach (Collider hitCollider in hitColliders)
            {
                Entity entity = hitCollider.GetComponent<Entity>();

                // Vérification que l'entité n'est pas nulle et différente du casteur
                if (entity != null && entity.gameObject != healerTransform.gameObject)
                {
                    entitiesToHeal.Add(entity);
                }
            }

            return entitiesToHeal;
        }




    }




    public class Cooldown : Node
    {
        // Durée entre deux attaques
        private float TimeBetwenneCooldown;

        // Temps écoulé depuis la dernière attaque
        private float timeLapse;

        // Constructeur pour initialiser le temps entre deux attaques
        public Cooldown(float TimeBetwenneAttack)
        {
            this.TimeBetwenneCooldown = TimeBetwenneAttack;
            this.timeLapse = 0f; // Initialiser le temps écoulé à zéro
        }

        // Méthode appelée à chaque évaluation du nœud
        protected override NodeState OnEvaluate()
        {
            // Incrémenter le temps écoulé depuis la dernière attaque
            timeLapse += Time.deltaTime;

            // Si le temps écoulé dépasse ou atteint le temps d'attente entre les attaques
            if (timeLapse >= TimeBetwenneCooldown)
            {
                // Le cooldown est terminé, réinitialiser le temps écoulé et retourner le succès
                timeLapse = 0f;
                return NodeState.SUCCESS;
            }
            else
            {
                // Si le cooldown n'est pas encore terminé, retourne "RUNNING"
                return NodeState.RUNNING;
            }
        }

        // Fonction pour afficher le texte du nœud (utile pour un éditeur de comportement, par exemple)
        public override string GetText() => $"Cooldown ({timeLapse:F2}/{TimeBetwenneCooldown} sec)";
    }



    public class ProxiBoat : Sequence
    {
        public const string NeedFollow = "needFollow";
        DistanceSmaller DistanceSmaller; // Peut être une autre Node si vous en avez besoin
        Node root;
        Transform self;
        string targets; // Nom ou clé des cibles
        float distance; // Distance de proximité à vérifier

        public ProxiBoat(Transform self, string target, float distance, Node root)
        {
            this.self = self;
            this.distance = distance;
            this.targets = target;
            this.root = root;
        }

        protected override NodeState OnEvaluate()
        {
          

            // Vérifie si la distance à la cible est inférieure ou égale à la distance donnée
            if (IsClose())
            {
              //  Debug.Log("Cible à proximité. Arrêter le suivi et déclencher l'animation.");
               
                SetData(NeedFollow, false); // Arrêter le suivi si proche
            }
            else
            {
             //   Debug.Log("Cible éloignée. Continuer le suivi.");
               
                SetData(NeedFollow, true); // Continuer le suivi si loin
            }

            return NodeState.SUCCESS;
        }

        private bool IsClose()
        {
            Transform target = GetData<Transform>(targets); // Récupère la cible via la clé

            if (target == null)
            {
                Debug.LogError("Cible introuvable!");
                return false; // Si la cible est null, retourner faux
            }

            // Calculer la distance entre 'self' (bateau) et 'target'
            float currentDistance = Vector3.Distance(self.position, target.position);
           
            
            // Comparer avec la distance limite définie
            return currentDistance <= distance;
        }

        public override string GetText() => "ProxiBoat";
    }

    public class AnimationAttack : Node
    {
        float animationTime;
        float animationVitesse;
        float timeLapse;
        bool needToReset =false;
        Node root;
        Transform target;
        float initialYRotation;  // Pour sauvegarder la rotation de départ sur l'axe Y
        public const string NeedFolow = "needFolow";
        public AnimationAttack(float animationTime, float animationVitesse, Transform target, Node root)
        {
            this.root = root;
            this.animationTime = animationTime;
            this.animationVitesse = animationVitesse;
            this.target = target;
            this.timeLapse = 0f;
            this.initialYRotation = target.eulerAngles.y; // Sauvegarde de la rotation Y initiale
        }

        protected override NodeState OnEvaluate()
        {
            bool needFolow = GetData<bool>(NeedFolow);
            Debug.Log(needFolow);
            if (timeLapse < animationTime && !needFolow)
            {
                needToReset=true;
                DoAnimation();
                return NodeState.RUNNING; // L'animation est en cours
            }
            else
            {
                if (needToReset)
                {
                    needToReset = !needToReset;
                    ResetAnimation(); // Réinitialisation après l'animation
                }
              
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
            target.rotation = Quaternion.Euler(-90, initialYRotation, 0);
            timeLapse = 0f;
        }

        public override string GetText() => "AnimationAttack";
    }

    public class Attacks : Node
    {
        GameObject bulletToActivate;

        public Attacks(GameObject bullet){
           this.bulletToActivate = bullet;
            }
        protected override NodeState OnEvaluate()
        {

            bulletToActivate.gameObject.SetActive(true);
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
               // Debug.Log("Arrêt du suivi de la cible");
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
