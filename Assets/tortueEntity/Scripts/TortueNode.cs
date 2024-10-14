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
           
            

            foreach (Node node in children)
            {
                state = node.Evaluate();
                if (state == NodeState.RUNNING)
                {
                    return state;
                }
            }

             List<Entity> entitiesToHeal = FindPeopleToHeal();

          
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
                return NodeState.SUCCESS;
            }
            else
            {
             //   Debug.Log("Cible éloignée. Continuer le suivi.");
               
                SetData(NeedFollow, true); // Continuer le suivi si loin
                return NodeState.RUNNING;
            }

          
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
        bool needToReset = false;
        Node root;
        Transform target; // Rotation s'applique ici
        Transform lunch;  // Le lancement s'applique ici
        Vector3 initialPosition; // Sauvegarde de la position initiale
        float initialYRotation;  // Sauvegarde de la rotation de départ sur l'axe Y
        public const string NeedFolow = "needFolow";
        bool startAnimation = false;
        bool lunchAnimation = false;
        bool returning = false; // Indique si l'objet est en phase de retour à sa position initiale

        public AnimationAttack(float animationTime, float animationVitesse, Transform target, Transform lunch)
        {
            this.root = root;
            this.animationTime = animationTime;
            this.animationVitesse = animationVitesse;
            this.target = target;
            this.timeLapse = 0f;
            this.initialPosition = lunch.position;  // Sauvegarde de la position initiale pour lunch
            this.initialYRotation = target.eulerAngles.y; // Sauvegarde de la rotation Y initiale pour target
            this.lunch = lunch;
        }

        protected override NodeState OnEvaluate()
        {
            NodeState state = children[0].Evaluate();

            if (!startAnimation && state == NodeState.RUNNING)
            {
                startAnimation = true;
            }

            if (startAnimation && state == NodeState.SUCCESS)
            {
                lunchAnimation = true;
            }

            if (startAnimation)
            {
                needToReset = true;
                DoAnimation(); // Rotation sur target

                if (lunchAnimation)
                {
                    DoLunchAnimation(); // Déplacement sur lunch

                    if (!returning) // Si l'objet n'est pas encore revenu
                    {
                        return NodeState.RUNNING;
                    }
                    else
                    {
                        return NodeState.SUCCESS; // L'animation de retour est terminée
                    }
                }

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

        // Rotation pendant le temps défini sur le target
        public void DoAnimation()
        {
            if (timeLapse < animationTime)
            {
                // Calcul de l'angle de rotation actuel
                float rotationAmount = animationVitesse * (timeLapse / animationTime) * 360f; // 360° sur Z
                target.rotation = Quaternion.Euler(-90, 0, initialYRotation + rotationAmount); // On fixe manuellement la rotation Z
                timeLapse += Time.deltaTime;
            }
        }

        // Déplacement en avant suivi d'un retour avec un effet de rebond sur le lunch
        public void DoLunchAnimation()
        {
            if (!returning)
            {
                // Avance en ligne droite
                float moveDistance = animationVitesse * Time.deltaTime;
                lunch.position += lunch.forward * moveDistance;

                // Vérifie si l'objet a atteint la fin de l'animation
                if (timeLapse >= animationTime)
                {
                    returning = true; // Commence la phase de retour
                    timeLapse = 0f; // Réinitialise le timer pour le retour
                }
            }
            else
            {
                // Retour avec effet de rebond
                float returnDistance = animationVitesse * Time.deltaTime;

                // On calcule la position cible
                Vector3 directionBack = initialPosition - lunch.position;
                if (directionBack.magnitude > returnDistance)
                {
                    lunch.position += directionBack.normalized * returnDistance;
                }
                else
                {
                    lunch.position = initialPosition; // Remet à la position initiale
                    returning = false; // Terminé
                }
            }
        }

        // Réinitialisation de l'animation
        public void ResetAnimation()
        {
            // Remet la rotation Y à zéro (ou à la rotation initiale) sur target
            target.rotation = Quaternion.Euler(-90, initialYRotation, 0);

            // Remet la position initiale sur lunch
            lunch.position = initialPosition;

            timeLapse = 0f;
            returning = false; // Assure que l'objet n'est plus en phase de retour
            startAnimation = false;
            lunchAnimation = false;
        }

        public override string GetText() => "AnimationAttack";
    }

    public class Attacks : Node
    {
        GameObject bulletToActivate;
        bool activateBullet = false;
        public Attacks(GameObject bullet){
           this.bulletToActivate = bullet;
            }
        protected override NodeState OnEvaluate()
        {
            NodeState state = children[0].Evaluate();
            if(state == NodeState.SUCCESS)
            {
                if (!activateBullet)
                {
                    activateBullet = true;
                    bulletToActivate.gameObject.SetActive(true);
                }

            }
            else
            {
                activateBullet = false;
            }
          



          
            return state;
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
            NodeState state = children[0].Evaluate();

            if (state == NodeState.SUCCESS)
            {
                agent.ResetPath(); // Annule toute destination en cours
                return NodeState.RUNNING; // Retourne FAILURE car le suivi est stoppé
            }

            
            

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
