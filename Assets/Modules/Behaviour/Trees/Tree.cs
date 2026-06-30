using BehaviourModule.Interfaces;
using BehaviourModule.Nodes;
using UnityEngine;

namespace BehaviourModule.Trees
{
    /// <summary>
    /// List of nodes to create a behaviour
    /// </summary>
    // Notion from here: https://www.youtube.com/watch?v=aR6wt5BlE-E
    public abstract class Tree : MonoBehaviour, IVisualizable
    {
        #region Tree

        private Node root;

        /// <summary>
        /// Called when this tree is being created
        /// </summary>
        /// <returns>Tree to use</returns>
        protected abstract Node SetUpTree();

        #endregion

        #region MonoBehaviour

        /// <inheritdoc cref="Start" />
        private void Start() => RebuildRoot();

        /// <inheritdoc cref="Update" />
        private void Update()
        {
            // Disable if root is invalid
            if (root == null)
            {
                // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
                Debug.LogError("The root was invalid for '" + name + "'.");
                enabled = false;
                return;
            }

            root.Reset();
            root.Evaluate();
        }

        #endregion

        #region IVisualizable

        /// <inheritdoc/>
        public Node GetRoot() => root;

        /// <inheritdoc/>
        public void RebuildRoot() => root = SetUpTree();

        #endregion
    }
}