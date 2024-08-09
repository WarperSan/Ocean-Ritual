using System.Collections;
using System.Collections.Generic;

namespace BehaviourTree
{
    /// <summary>
    /// Class that represents a single element in the tree
    /// </summary>
    public abstract class Node : IEnumerable<Node>
    {
        public Node parent = null;

        #region Constructor

        public Node(params Node[] children)
        {
            this.AddChildren(children);
        }

        #endregion

        #region Data

        private readonly Dictionary<string, object> dataContext = new();

        /// <summary>
        /// Stores the given value at the given key
        /// </summary>
        public void SetData(string key, object value, bool inRoot = false) 
        {
            if (inRoot && this.parent != null)
            {
                this.parent.SetData(key, value, true);
            }
            else
            {
                this.dataContext[key] = value;
            }
        }

        /// <summary>
        /// Fetches the value of the given key
        /// </summary>
        /// <returns>Value or null</returns>
        public T GetData<T>(string key)
        {
            // If key in self, return
            if (this.dataContext.TryGetValue(key, out object value) && value is T t)
                return t;

            // Search in parent
            Node node = this.parent;
            while (node != null)
            {
                value = node.GetData<T>(key);
                if (value is T v)
                    return v;

                node = node.parent;
            }

            return default;
        }

        /// <summary>
        /// Removes the data associated with the given key
        /// </summary>
        /// <returns>The key was found</returns>
        public bool ClearData(string key)
        {
            // If has key, remove
            if (this.dataContext.ContainsKey(key))
            {
                this.dataContext.Remove(key);
                return true;
            }

            // Search in parent
            Node node = this.parent;
            while (node != null)
            {
                if (node.ClearData(key))
                    return true;

                node = node.parent;
            }

            return false;
        }

        #endregion
    
        #region State

        /// <summary>
        /// Node of this state
        /// </summary>
        protected NodeState state;

        /// <summary>
        /// Called when this node gets updated
        /// </summary>
        /// <returns>State of this node</returns>
        public virtual NodeState Evaluate() => NodeState.FAILURE;
        
        #endregion

        #region Children

        private readonly List<Node> children = new();

        /// <summary>
        /// Sets the parent of the given nodes to this
        /// </summary>
        private void AddChildren(params Node[] children)
        {
            foreach (Node item in children)
            {
                item.parent = this;
                this.children.Add(item);
            }
        }

        #endregion

        #region IEnumerable

        /// <inheritdoc/>
        public IEnumerator<Node> GetEnumerator() => this.children.GetEnumerator();
        
        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        #endregion
    }
}