using BehaviourModule.Interfaces;
using BehaviourModule.Nodes;
using UnityEngine;

namespace EntityModule.Entities
{
    [RequireComponent(typeof(IVisualizable))]
    public abstract class EntityBehaviour : Entity
    {
        #region Entity

        /// <inheritdoc/>
        protected override void OnStart()
        {
            IVisualizable tree = this.GetComponent<IVisualizable>();
            tree.RebuildRoot();
            this.root = tree.GetRoot();
        }

        #endregion

        #region Behaviour Tree

        private Node root;

        private void UpdateTree() => root.Evaluate();

        private void Update()
        {
            UpdateTree();
        }

        #endregion
    }
}