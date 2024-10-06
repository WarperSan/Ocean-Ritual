using BehaviourModule.Interfaces;
using BehaviourModule.Nodes;
using UnityEngine;

namespace EntityModule.Enemies
{
    public class SharkBehaviourTree : MonoBehaviour, IVisualizable
    {
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
            // Attack when in range
            // Delay between strikes
            // Rushes towards the player and attacks it

            return null;
        }

        #endregion
    }
}