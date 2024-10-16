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
            Collider[] hitColliders = Physics.OverlapSphere(healerTransform.position, RayonToHeal, enemyLayerMask);

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
        public const string Reset = "Reset";
        private bool impactReset = false;
        // Temps écoulé depuis la dernière attaque
        private float timeLapse;

        // Constructeur pour initialiser le temps entre deux attaques
        public Cooldown(float TimeBetwenneAttack, bool impactReset = false)
        {
            
            this.TimeBetwenneCooldown = TimeBetwenneAttack;
            this.timeLapse = 0f; // Initialiser le temps écoulé à zéro
            this.impactReset = impactReset;
        }

        // Méthode appelée à chaque évaluation du nœud
        protected override NodeState OnEvaluate()
        {
            bool reset = false;
            if (impactReset)
            {
                reset= GetData<bool>(Reset);
                if (reset)
                {
                    timeLapse = 0;
                }
            }
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
        public const string Reset = "Reset";
        DistanceSmaller DistanceSmaller; // Peut être une autre Node si vous en avez besoin
        Node root;
        Transform self;
        string targets; // Nom ou clé des cibles
        float distance; // Distance de proximité à vérifier
        public const string NeedFolow = "needFolow";
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
                SetData(NeedFolow, false, 2);
                // SetData(Reset, false); // Arrêter le suivi si proche
                return NodeState.SUCCESS;
            }
            else
            {
             //   Debug.Log("Cible éloignée. Continuer le suivi.");
               
                SetData(Reset, true,2); // Continuer le suivi si loin
                SetData(NeedFolow, true, 2); // Continuer le suivi si loin
               

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
        float animationTimeLunch;
        float floatanimationVitesseLunch;
        float timeLapse;
        
        bool startAnimation = false;
        bool lunchAnimation = false;
        bool returning = false;
        public const string CURRENT_TARGET = "currentTarget";
        Transform target;   // Rotation
        Transform lunch;    // Déplacement
        Vector3 initialPosition; // Position initiale de lunch
        float initialYRotation;  // Rotation Y initiale
        float Distance;
        float timeLapseRotation =0;
        public const string Reset = "Reset";
        bool start = false;
        public AnimationAttack(float distance,float animationTime, float animationVitesse, float animationTimeLunch, float floatanimationVitesseLunch, Transform target, Transform lunch)
        {
            this.animationTimeLunch = animationTimeLunch;
            this.floatanimationVitesseLunch = floatanimationVitesseLunch;
            Distance = distance;
            this.animationTime = animationTime;
            this.animationVitesse = animationVitesse;
            this.target = target;
            this.lunch = lunch;
            this.initialYRotation = target.eulerAngles.y; // Sauvegarde rotation Y initiale
        }
        //
        protected override NodeState OnEvaluate()
        {
          bool   reset = GetData<bool>(Reset);
            Debug.Log(reset);
            if (reset && start) { ResetAnimation();

                Debug.Log("CCCCCCCCC");
                SetData(Reset, false, 3);
            }
            NodeState state = children[0].Evaluate();

            if (!startAnimation && state == NodeState.RUNNING)
            {
                startAnimation = true;
                start = true;
            }

            if (startAnimation && state == NodeState.SUCCESS)
            {
                if (!lunchAnimation)
                {
                    initialPosition = lunch.position; // Sauvegarde la position initiale
                }
                lunchAnimation = true;
            }

            if (startAnimation)
            {
                timeLapse += Time.deltaTime;

                // Rotation sur target pendant l'animation
                DoRotation();

                // Animation du mouvement de lunch
                if (lunchAnimation)
                {
                    DoLunchAnimation();

                    if (!returning) // Si l'objet n'est pas encore revenu
                        return NodeState.RUNNING;

                    return NodeState.SUCCESS; // Fin de l'animation
                }

                return NodeState.RUNNING; // L'animation est en cours
            }

           

            return NodeState.SUCCESS; // Animation terminée
        }

        // Rotation simple sur l'axe Y
        public void DoRotation()
        {
            float rotationAmount = (timeLapse / animationTime) * 360f; // Calcul de la rotation en Z
            target.rotation = Quaternion.Euler(-90, 0, initialYRotation + rotationAmount); // Applique la rotation
        }

        // Avance puis retour de lunch avec effet de rebond
        public void DoLunchAnimation()
        {
            timeLapseRotation += Time.deltaTime;
            float moveDistance = floatanimationVitesseLunch * Time.deltaTime;
           
            if (!returning)
            {
                
                lunch.position += lunch.forward * moveDistance; // Avance

                if (timeLapseRotation >= animationTimeLunch)
                {
                    returning = true;
                    timeLapseRotation = 0f; // Réinitialise le timer pour le retour
                    timeLapse = 0f;
                }
            }
            else
            {
               
                Vector3 directionBack = initialPosition - lunch.position;
                if (directionBack.magnitude > moveDistance)
                {
                    lunch.position += directionBack.normalized * moveDistance; // Retourne
                  
                }
                else
                {
                    lunch.position = initialPosition; // Retourne à la position initiale
                    returning = false; // Fin du retour

                    Debug.Log("BBBBBBBBBBB");
                    ResetAnimation();
                }
            }
        }

        // Réinitialisation après l'animation
        public void ResetAnimation()
        {
            Debug.Log("aAAAAAAAAAAA");
            target.rotation = Quaternion.Euler(-90, initialYRotation, 0); // Reset rotation Y
            lunch.position = initialPosition; // Reset position
            timeLapse = 0f;
            returning = false;
            startAnimation = false;
            lunchAnimation = false;
        }

        public override string GetText() => "AnimationAttack";
    }


    public class Attacks : Node
    {
        public const string BOOLHITS = "hit";
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




    public class FollowTarget : Node
    {
        public const string AGENT = "agent";
        public const string CURRENT_TARGET = "currentTarget";
        public const string NeedFolow = "needFolow";

        protected override NodeState OnEvaluate()
        {
            Transform target = GetData<Transform>(CURRENT_TARGET);
            NavMeshAgent agent = GetData<NavMeshAgent>(AGENT);
            bool NeedFolows = GetData<bool>(NeedFolow);
           

            if (!NeedFolows)
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
