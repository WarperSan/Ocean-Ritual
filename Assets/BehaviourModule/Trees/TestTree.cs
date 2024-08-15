using BehaviourModule.Nodes;
using BehaviourModule.Nodes.Generic;
using BehaviourModule.Nodes.Operators;
using UnityEngine;

namespace BehaviourModule.Trees
{
    public class TestTree : Tree
    {
        protected override Node SetUpTree()
        {
            return new Selector(
                new Sequence(
                    new CallbackNode(this.TestABC),
                    new Selector(
                        new Selector(
                            new Selector(
                                new Selector(
                                    new CallbackNode(n => this.TestABC(n))
                                ),
                                new CallbackNode(this.TestABC)
                            ),
                            new CallbackNode(this.TestABC)
                        ),
                        new CallbackNode(this.TestABC),
                        new CallbackNode(this.TestDBC)
                    ),
                    new CallbackNode(this.TestDBC)
                ),
                new CallbackNode(this.TestABC),
                new CallbackNode(this.TestDBC)
            );
        }

        public NodeState TestABC(Node n)
        {
            return Time.time % 2 < 1 ? NodeState.SUCCESS : NodeState.FAILURE;
        }

        public NodeState TestDBC(Node n)
        {
            return Time.time % 2 > 1 ? NodeState.SUCCESS : NodeState.FAILURE;
        }
    }
}