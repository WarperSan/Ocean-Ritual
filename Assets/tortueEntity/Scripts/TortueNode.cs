using BehaviourModule.Nodes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



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


        protected override NodeState OnEvaluate()
        {


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


        protected override NodeState OnEvaluate()
        {


            return NodeState.SUCCESS;
        }




        public override string GetText() => "FollowTarget";

    }




}
