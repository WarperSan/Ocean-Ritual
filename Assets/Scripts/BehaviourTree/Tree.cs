using UnityEngine;

namespace BehaviourTree
{
    /// <summary>
    /// List of nodes to create a behaviour
    /// </summary>
    // Notion from here: https://www.youtube.com/watch?v=aR6wt5BlE-E
    public abstract class Tree : MonoBehaviour
    {
        #region Tree

        private Node root = null;

        /// <summary>
        /// Replaces the current tree with a new one
        /// </summary>
        public void RefreshTree() => this.root = this.SetUpTree();

        /// <summary>
        /// Called when this tree is being created
        /// </summary>
        /// <returns>Tree to use</returns>
        protected abstract Node SetUpTree();

        #endregion

        #region MonoBehaviour

        /// <inheritdoc/>
        private void Start() => this.RefreshTree();

        /// <inheritdoc/>
        private void Update() 
        {
            // Disable if root is invalid
            if (this.root == null)
            {
                this.enabled = false;
                return;
            }

            this.root.Evaluate();
        }

        #endregion
    }
}