using BehaviourModule.Nodes;
using BehaviourModule.Nodes.Controls;
using BehaviourModule.Nodes.Generic;
using EntityModule;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TortueNode
{
    public class Heals : Node
    {
        private float RayonToHeal;
        private float HealPower;
        private Transform healerTransform; // Position � partir de laquelle on effectue la recherche

        private Heal heal = new();

        // Constructeur
        public Heals(float RayonToHeal, float HealPower, Transform healerTransform)
        {
            this.RayonToHeal = RayonToHeal;
            this.HealPower = HealPower;
            this.healerTransform = healerTransform;
            heal.Amount = HealPower;
        }

        // M�thode principale du node
        protected override NodeState OnEvaluate()
        {
            NodeState state;

            foreach (Node node in children)
            {
                state = node.Evaluate();

                if (state == NodeState.RUNNING)
                    return state;
            }

            List<Entity> entitiesToHeal = FindPeopleToHeal();

            if (entitiesToHeal.Count > 0)
            {
                // Logique de soin
                foreach (Entity entity in entitiesToHeal)
                    entity.UseHeal(heal); // Par exemple, une m�thode 'Heal' qui augmente la sant�

                return NodeState.SUCCESS;
            }

            return NodeState.SUCCESS;
        }

        // M�thode pour trouver les entit�s dans un rayon
        private List<Entity> FindPeopleToHeal()
        {
            var entitiesToHeal = new List<Entity>();

            // Utilisation d'un LayerMask pour ne chercher que les ennemis
            int enemyLayerMask = LayerMask.GetMask("Enemy");

            // On v�rifie que le healerTransform est bien initialis� avant d'appeler OverlapSphere
            if (healerTransform == null)
            {
                Debug.LogError("Healer Transform is not assigned!");
                return entitiesToHeal;
            }

            // Ex�cution de la d�tection dans un rayon avec filtrage par layer
            Collider[] hitColliders = Physics.OverlapSphere(healerTransform.position, RayonToHeal, enemyLayerMask);

            // Parcours des objets d�tect�s
            foreach (Collider hitCollider in hitColliders)
            {
                Entity entity = hitCollider.GetComponent<Entity>();

                // V�rification que l'entit� n'est pas nulle et diff�rente du casteur
                if (entity != null && entity.gameObject != healerTransform.gameObject)
                    entitiesToHeal.Add(entity);
            }

            return entitiesToHeal;
        }
    }

    public class Cooldown : Node
    {
        // Dur�e entre deux attaques
        private float TimeBetwenneCooldown;

        private bool impactReset;

        // Temps �coul� depuis la derni�re attaque
        private float timeLapse;
        public const string NeedFolow = "needFolow";

        public const string RESET_TAG = "Reset";

        // Constructeur pour initialiser le temps entre deux attaques
        public Cooldown(float TimeBetwenneAttack, bool impactReset = false)
        {
            TimeBetwenneCooldown = TimeBetwenneAttack;
            timeLapse = 0f; // Initialiser le temps �coul� � z�ro
            this.impactReset = impactReset;
        }

        // M�thode appel�e � chaque �valuation du n�ud
        protected override NodeState OnEvaluate()
        {
            bool NeedFolows = GetData<bool>(NeedFolow);
            bool reset = false;

            if (impactReset)
            {
                if (NeedFolows)
                    return NodeState.FAILURE;

                reset = GetData<bool>(RESET_TAG);

                if (reset)
                {
                    SetData(RESET_TAG, false, 3);
                    timeLapse = 0;
                }
            }
            // Incr�menter le temps �coul� depuis la derni�re attaque
            timeLapse += Time.deltaTime;

            // Si le temps �coul� d�passe ou atteint le temps d'attente entre les attaques
            if (timeLapse >= TimeBetwenneCooldown)
            {
                // Le cooldown est termin�, r�initialiser le temps �coul� et retourner le succ�s
                timeLapse = 0f;
                return NodeState.SUCCESS;
            }
            else
            {
                // Si le cooldown n'est pas encore termin�, retourne "RUNNING"
                return NodeState.RUNNING;
            }
        }

        // Fonction pour afficher le texte du n�ud (utile pour un �diteur de comportement, par exemple)
        public override string GetText() => $"Cooldown ({timeLapse:F2}/{TimeBetwenneCooldown} sec)";
    }

    public class ProxiBoat : Sequence
    {
        private DistanceSmaller DistanceSmaller; // Peut �tre une autre Node si vous en avez besoin
        private Node root;
        private Transform self;
        private string targets; // Nom ou cl� des cibles
        private float distance; // Distance de proximit� � v�rifier
        public const string NeedFolow = "needFolow";

        public ProxiBoat(
            Transform self,
            string    target,
            float     distance,
            Node      root
        )
        {
            this.self = self;
            this.distance = distance;
            targets = target;
            this.root = root;
        }

        protected override NodeState OnEvaluate()
        {
            // V�rifie si la distance � la cible est inf�rieure ou �gale � la distance donn�e
            if (IsClose())
            {
                //  Debug.Log("Cible � proximit�. Arr�ter le suivi et d�clencher l'animation.");
                SetData(NeedFolow, false, 1);
                // SetData(Reset, false); // Arr�ter le suivi si proche
                return NodeState.SUCCESS;
            }
            else
            {
                //   Debug.Log("Cible �loign�e. Continuer le suivi.");

                SetData(Cooldown.RESET_TAG, true, 1); // Continuer le suivi si loin
                SetData(NeedFolow, true, 1);          // Continuer le suivi si loin

                return NodeState.RUNNING;
            }
        }

        private bool IsClose()
        {
            Transform target = TargetGeneral.Instance.Target; // R�cup�re la cible via la cl�

            if (target == null)
            {
                Debug.LogError("Cible introuvable!");
                return false; // Si la cible est null, retourner faux
            }

            // Calculer la distance entre 'self' (bateau) et 'target'
            float currentDistance = Vector3.Distance(self.position, target.position);

            // Comparer avec la distance limite d�finie
            return currentDistance <= distance;
        }

        public override string GetText() => "ProxiBoat";
    }

    public class AnimationAttack : Node
    {
        private float animationTime;
        private float animationVitesse;
        private float animationTimeLunch;
        private float floatanimationVitesseLunch;
        private float timeLapse;

        private bool startAnimation;
        private bool lunchAnimation;
        private bool returning;
        public const string CURRENT_TARGET = "currentTarget";
        private Transform target;        // Rotation
        private Transform lunch;         // D�placement
        private Vector3 initialPosition; // Position initiale de lunch
        private float initialYRotation;  // Rotation Y initiale
        private float Distance;
        private float timeLapseRotation;
        private bool start;

        public AnimationAttack(
            float     distance,
            float     animationTime,
            float     animationVitesse,
            float     animationTimeLunch,
            float     floatanimationVitesseLunch,
            Transform target,
            Transform lunch
        )
        {
            this.animationTimeLunch = animationTimeLunch;
            this.floatanimationVitesseLunch = floatanimationVitesseLunch;
            Distance = distance;
            this.animationTime = animationTime;
            this.animationVitesse = animationVitesse;
            this.target = target;
            this.lunch = lunch;
            initialYRotation = target.eulerAngles.y; // Sauvegarde rotation Y initiale
        }

        /// <summary>
        /// ///////////
        /// </summary>
        /// <returns></returns>
        protected override NodeState OnEvaluate()
        {
            bool reset = GetData<bool>(Cooldown.RESET_TAG);

            NodeState state = children[0].Evaluate();

            if (state == NodeState.FAILURE)
            {
                if (reset && start)
                {
                    ResetAnimation();
                    start = false;
                    SetData(Cooldown.RESET_TAG, false, 2);
                }
                return state;
            }

            if (!startAnimation && state == NodeState.RUNNING)
            {
                startAnimation = true;
                start = true;
            }

            if (startAnimation && state == NodeState.SUCCESS)
            {
                if (!lunchAnimation)
                    initialPosition = lunch.position; // Sauvegarde la position initiale
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

            return NodeState.SUCCESS; // Animation termin�e
        }

        // Rotation simple sur l'axe Y
        public void DoRotation()
        {
            float rotationAmount = timeLapse / animationTime * 360f;                       // Calcul de la rotation en Z
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
                    timeLapseRotation = 0f; // R�initialise le timer pour le retour
                    timeLapse = 0f;
                }
            }
            else
            {
                Vector3 directionBack = initialPosition - lunch.position;

                if (directionBack.magnitude > moveDistance)
                    lunch.position += directionBack.normalized * moveDistance; // Retourne
                else
                {
                    lunch.position = initialPosition; // Retourne � la position initiale
                    returning = false;                // Fin du retour
                    lunchAnimation = false;

                    // ResetAnimation();
                }
            }
        }

        // R�initialisation apr�s l'animation
        public void ResetAnimation()
        {
            Debug.Log("REset animation");
            target.localRotation = Quaternion.Euler(-90, 0, 0); // Reset rotation Y
            lunch.localPosition = new Vector3(0, 0, 0);         // Reset position
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
        private GameObject bulletToActivate;
        private bool activateBullet;

        public Attacks(GameObject bullet)
        {
            bulletToActivate = bullet;
        }

        //
        protected override NodeState OnEvaluate()
        {
            NodeState state = children[0].Evaluate();

            if (state == NodeState.SUCCESS)
            {
                if (!activateBullet)
                {
                    activateBullet = true;
                    bulletToActivate.gameObject.SetActive(true);
                }
            }
            else
                activateBullet = false;

            if (state == NodeState.FAILURE)
                return NodeState.RUNNING;

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
            Transform target = TargetGeneral.Instance.Target;
            NavMeshAgent agent = GetData<NavMeshAgent>(AGENT);
            bool NeedFolows = GetData<bool>(NeedFolow);

            if (!NeedFolows)
            {
                agent.ResetPath();        // Annule toute destination en cours
                return NodeState.RUNNING; // Retourne FAILURE car le suivi est stopp�
            }

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
            return NodeState.RUNNING; // Retourne en cours si l'agent est encore en mouvement
        }

        public override string GetText() => "FollowTarget";
    }
}