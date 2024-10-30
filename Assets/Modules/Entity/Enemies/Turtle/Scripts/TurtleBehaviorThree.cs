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
    [SerializeField] float animationTimeLunch = 5;
    [SerializeField] float animationVitesseLunch = 5;
    [SerializeField] float RayonToHeal = 10;
    [SerializeField] float HealPower = 10;
    [SerializeField] float distanceDash = 5;
    [SerializeField] float TimeBetwenneAttack = 10;
    [SerializeField] GameObject parent;
    [SerializeField] float cooldownHeal = 10;
    
    [SerializeField] GameObject lunch;


    Parallel _root = new();
    
  

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
        heals.Attach(coldownHeal);
        _root.Attach(heals);

        //1er embranchement
        Parallel Parallel2 = new Parallel();
     //   _root.Attach(Parallel2);

       // 2em embranchement section Attaque
       // Sequence sequence1 = new Sequence();
        
   
        ProxiBoat ProxiBoat = new ProxiBoat(parent.transform, CURRENT_TARGET, Distance, _root);
        Cooldown coldownAttack = new Cooldown (TimeBetwenneAttack,true);
        AnimationAttack animationAttack = new AnimationAttack (distanceDash,animationTime, animationVitesse, animationTimeLunch, animationVitesseLunch, transform, lunch.transform   );
        animationAttack.Attach(coldownAttack);
        Attacks attack = new Attacks (projectile);
        attack.Attach(animationAttack);
       

        //3em embranchement section mouvement
        
        FollowTarget folowTarget = new FollowTarget ();
       // folowTarget.Attach(ProxiBoat);


        //jointure  embranchement 2 et 3 
        Parallel2.Attach(new Node[] { ProxiBoat, attack, folowTarget });


        _root.Attach(Parallel2);
        //_root += this.RotateSequence();

        ProxiBoat.SetData(NeedFolow, false, 2);
        _root.SetData(CURRENT_TARGET, this.target);
        _root.SetData(NeedFolow,true);
        _root.SetData(AGENT, this.agent);
     
        return _root;
    }

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

}
