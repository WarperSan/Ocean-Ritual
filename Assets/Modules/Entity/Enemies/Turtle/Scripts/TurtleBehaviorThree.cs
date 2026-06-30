using BehaviourModule.Nodes.Generic;
using BehaviourModule.Nodes;
using BehaviourModule.Nodes.Controls;
using UnityEngine;
using BehaviourModule.Interfaces;
using TortueNode;
using UnityEngine.AI;

public class TurtleBehaviorThree : MonoBehaviour, IVisualizable
{
    public const string CURRENT_TARGET = "currentTarget";
    public const string AGENT = "agent";
    public const string BOOLHITS = "hit";
    public const string WALK_SPEED = "walkSpeed";
    public const string NeedFolow = "needFolow";
    public Transform target;

    [SerializeField]
    private NavMeshAgent agent;

    [SerializeField]
    private GameObject projectile;

    [SerializeField]
    private float Distance = 3;

    [SerializeField]
    private float animationTime = 5;

    [SerializeField]
    private float animationVitesse = 5;

    [SerializeField]
    private float animationTimeLunch = 5;

    [SerializeField]
    private float animationVitesseLunch = 5;

    [SerializeField]
    private float RayonToHeal = 10;

    [SerializeField]
    private float HealPower = 10;

    [SerializeField]
    private float distanceDash = 5;

    [SerializeField]
    private float TimeBetwenneAttack = 10;

    [SerializeField]
    private GameObject parent;

    [SerializeField]
    private float cooldownHeal = 10;

    [SerializeField]
    private GameObject lunch;

    private Parallel _root = new();

    #region IVisualizable

    protected Node root;

    /// <inheritdoc/>
    public Node GetRoot() => root;

    /// <inheritdoc/>
    public void RebuildRoot() => root = SetUpTree();

    private Node SetUpTree()
    {
        //section heal
        var coldownHeal = new Cooldown(cooldownHeal);
        Heals heals = new(RayonToHeal, HealPower, transform);
        heals.Attach(coldownHeal);
        _root.Attach(heals);

        //1er embranchement
        var Parallel2 = new Parallel();
        //   _root.Attach(Parallel2);

        // 2em embranchement section Attaque
        // Sequence sequence1 = new Sequence();

        var ProxiBoat = new ProxiBoat(parent.transform,
            CURRENT_TARGET,
            Distance,
            _root);
        var coldownAttack = new Cooldown(TimeBetwenneAttack, true);

        var animationAttack = new AnimationAttack(distanceDash,
            animationTime,
            animationVitesse,
            animationTimeLunch,
            animationVitesseLunch,
            transform,
            lunch.transform);
        animationAttack.Attach(coldownAttack);
        var attack = new Attacks(projectile);
        attack.Attach(animationAttack);

        //3em embranchement section mouvement

        var folowTarget = new FollowTarget();
        // folowTarget.Attach(ProxiBoat);

        //jointure  embranchement 2 et 3 
        Parallel2.Attach(new Node[]
        {
            ProxiBoat,
            attack,
            folowTarget,
        });

        _root.Attach(Parallel2);
        //_root += this.RotateSequence();

        ProxiBoat.SetData(NeedFolow, false, 2);
        _root.SetData(CURRENT_TARGET, target);
        _root.SetData(NeedFolow, true);
        _root.SetData(AGENT, agent);

        return _root;
    }

    #endregion

    #region Rotation

    private Node RotateSequence()
    {
        Sequence rotateSequence = new();
        rotateSequence += new CallbackNode(RotateTowardsTarget).Alias("Rotate towards target");

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
}