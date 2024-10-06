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
            this.tree.RebuildRoot();
            this.root = this.tree.GetRoot();
        }

        #endregion

        #region Behaviour Tree

        protected IVisualizable tree;
        private Node root;

        protected NodeState UpdateTree() => this.root.Evaluate();

        #endregion

        #region MonoBehaviour

        /// <inheritdoc/>
        private void Update() => this.UpdateTree();

        #endregion
    }
}