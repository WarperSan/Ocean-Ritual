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
            this.tree = this.GetComponent<IVisualizable>();

            this.root = this.tree.GetRoot();
            if (this.root != null)
            {
                Debug.LogWarning($"The tree for {this.name} was built from another source. Please don't build it manually.");
                return;
            }

            this.tree.RebuildRoot();
            this.root = this.tree.GetRoot();
        }

        #endregion

        #region Behaviour Tree

        protected IVisualizable tree;
        private Node root;

        protected NodeState UpdateTree() => this.root.Evaluate();

        #endregion
    }
}