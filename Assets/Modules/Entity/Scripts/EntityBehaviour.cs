using BehaviourModule.Interfaces;
using BehaviourModule.Nodes;
using UnityEngine;

namespace EntityModule.Entities
{
    /// <summary>
    /// Class that represents an entity that has a behaviour tree
    /// </summary>
    [RequireComponent(typeof(IVisualizable))]
    public abstract class EntityBehaviour : Entity
    {
        #region Entity

        /// <inheritdoc/>
        protected override void OnStart()
        {
            tree = GetComponent<IVisualizable>();

            root = tree.GetRoot();

            if (root != null)
            {
                Debug.LogWarning($"The tree for {name} was built from another source. Please don't build it manually.");
                return;
            }

            tree.RebuildRoot();
            root = tree.GetRoot();
        }

        #endregion

        #region Behaviour Tree

        protected IVisualizable tree;
        private Node root;

        protected NodeState UpdateTree()
        {
            if (root == null)
                return NodeState.FAILURE;

            root.Reset();
            return root.Evaluate();
        }

        #endregion

        #region MonoBehaviour

        /// <inheritdoc/>
        private void Update() => UpdateTree();

        #endregion
    }
}