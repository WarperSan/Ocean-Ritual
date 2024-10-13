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
    public const string WALK_SPEED = "walkSpeed";
    public Transform target;
    [SerializeField] NavMeshAgent agent;

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

        



        Parallel _root = new();


        //section heal
        PeopleToHeals peopleToHeal = new ();
        Heals heals = new ();
        heals.Attach(peopleToHeal);
      //  _root.Attach(heals);

        //1er embranchement
        Parallel Parallel2 = new Parallel();
     //   _root.Attach(Parallel2);

       // 2em embranchement section attack
        Sequence sequence1 = new Sequence(); 
        ProxiBoat proxiBoat = new ProxiBoat ();
        Cooldown coldown = new Cooldown ();
        AnimationAttack animationAttack = new AnimationAttack ();
        Attacks attack = new Attacks ();
        sequence1.Attach(new Node[] { proxiBoat, coldown, animationAttack, attack });


        //3em embranchement section mouvement
        Sequence sequence2 = new Sequence();
      //  AnnimationMovement annimationMovement = new AnnimationMovement ();
        FollowTarget folowTarget = new FollowTarget ();
        sequence2.Attach(new Node[] { folowTarget });


        //jointure  embranchement 2 et 3 
        Parallel2.Attach(new Node[] { sequence1, sequence2 });


        _root.Attach(folowTarget);
        //_root += this.RotateSequence();


         _root.SetData(CURRENT_TARGET, this.target);
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
