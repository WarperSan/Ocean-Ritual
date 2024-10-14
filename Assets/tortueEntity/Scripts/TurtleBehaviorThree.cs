using BehaviourModule.Nodes.Generic;
using BehaviourModule.Nodes;

using BehaviourModule.Nodes.Controls;

using UnityEngine;
using BehaviourModule.Interfaces;
using EntityModule;
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
    [SerializeField] NavMeshAgent agent;
    [SerializeField] GameObject projectile;
    [SerializeField] float Distance = 3;
    [SerializeField] float animationTime = 5;
    [SerializeField] float animationVitesse = 5;
    [SerializeField] float RayonToHeal = 10;
    [SerializeField] float HealPower = 10;
    [SerializeField] float TimeBetwenneAttack = 10;

    [SerializeField] float cooldownHeal = 10;
   


    Parallel _root = new();
    /// <inheritdoc/>
    private void Start() => this.RebuildRoot();

    #region IVisualizable

    protected Node root;

    /// <inheritdoc/>
    public Node GetRoot() => this.root;

    /// <inheritdoc/>
    public void RebuildRoot() => this.root = this.SetUpTree();

    private Node SetUpTree()
    {

        //section heal
        Cooldown coldownHeal = new Cooldown(cooldownHeal);
        Heals heals = new (RayonToHeal, HealPower,transform);
        heals.Attach(heals);
        _root.Attach(heals);

        //1er embranchement
        Parallel Parallel2 = new Parallel();
     //   _root.Attach(Parallel2);

       // 2em embranchement section attack
        Sequence sequence1 = new Sequence();
        
       // DistanceSmaller DistanceSmaller = new DistanceSmaller(transform, CURRENT_TARGET, Distance);
      //  DistanceSmaller.Alias("ProxiBoat");
        ProxiBoat ProxiBoat = new ProxiBoat(this.transform, CURRENT_TARGET, Distance, _root);
        Cooldown coldownAttack = new Cooldown (TimeBetwenneAttack);
        AnimationAttack animationAttack = new AnimationAttack (animationTime, animationVitesse,transform, _root);
        Attacks attack = new Attacks (projectile);
        // sequence1.Attach(new Node[] { ProxiBoat, coldownAttack, animationAttack, attack });
        sequence1.Attach(new Node[] { ProxiBoat, coldownAttack, animationAttack , attack });

        //3em embranchement section mouvement
        //  Sequence sequence2 = new Sequence();
        //  AnnimationMovement annimationMovement = new AnnimationMovement ();
        FollowTarget folowTarget = new FollowTarget ();
      // sequence2.Attach(new Node[] { folowTarget });


        //jointure  embranchement 2 et 3 
        Parallel2.Attach(new Node[] { sequence1, folowTarget });


        _root.Attach(Parallel2);
        //_root += this.RotateSequence();


         _root.SetData(CURRENT_TARGET, this.target);
        _root.SetData(NeedFolow,false);
        _root.SetData(AGENT, this.agent);
        return _root;
    }

    #endregion

  public void HitSomething()
    {
        Debug.Log("j'ai toucher quelque chose");
        _root.SetData(BOOLHITS, true);
    }

    

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

}
