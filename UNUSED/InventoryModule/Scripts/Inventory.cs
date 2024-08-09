using Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    /// <summary>
    /// Class that stores a list of items and manages them
    /// </summary>
    /// <typeparam name="U">Type of data to store</typeparam>
    [System.Serializable]
    public class Inventory<U> : ISerializationCallbackReceiver, IEnumerable<U> where U : ItemData
    {
        #region Items

        [SerializeField, HideInInspector]
        private List<U> Items = new();

        public int Count => this.Items.Count;

        public U this[int i] => this.Items[i];

        #endregion

        #region Add

        /// <summary>
        /// Adds the given items to this inventory
        /// </summary>
        /// <returns>Succeed to add all the items</returns>
        public bool Add<T>(params T[] items) where T : Item<U>
        {
            bool succeed = true;

            foreach (T item in items)
                succeed &= this.AddToStack(item, item.Save());

            return succeed;
        }

        /// <summary>
        /// Adds the given data to this inventory while checking for stacks
        /// </summary>
        /// <returns>Succeed to add</returns>
        private bool AddToStack<T>(T item, U data) where T : Item<U>
        {
            // If not stackable, default
            if (data is not ItemStackData<U> stackData)
                return this.AddToSelf(item, data);

            // If amount invalid, skip
            if (stackData.Amount == 0)
            {
                Debug.Log($"Tried to add '{stackData.Namespace}' with no quantity.");
                return true;
            }

            foreach (U currentData in this.Items)
            {
                // If data is not ItemStackData, skip
                if (currentData is not ItemStackData<U> itemStack)
                    continue;

                // If items not equals, skip
                if (!stackData.Equals(itemStack))
                    continue;

                // Add quantity
                itemStack.Amount += stackData.Amount;
                return true;
            }

            // If no valid stack found, default
            return this.AddToSelf(item, data);
        }

        /// <summary>
        /// Adds the given data to this inventory
        /// </summary>
        /// <returns>Succeed to add</returns>
        private bool AddToSelf<T>(T item, U data) where T : Item<U>
        {
            // If item or data invalid
            if (item == null || data == null)
                return false;

            // If namespace not the same, skip
            if (!item.Namespace.Equals(data.Namespace))
            {
                Debug.LogWarning("Tried to add an item with mismatch data.");
                return false;
            }

            // If item not registered, skip
            if (!Registry.GetItem<T>(data, out _))
            {
                Debug.LogWarning($"Tried to add '{data.Namespace}' while it is not registered.");
                return false;
            }

            this.Items.Add(data);
            return true;
        }

        #endregion

        #region Remove

        /// <summary>
        /// Removes all the items with the given namespace
        /// </summary>
        public bool RemoveAll(string @namespace) => this.RemoveAll(i => i.Namespace.Equals(@namespace));

        /// <summary>
        /// Removes the first item that meets the given condition
        /// </summary>
        /// <returns>Removed an item</returns>
        private bool RemoveFirst(System.Predicate<U> predicate)
        {
            for (int i = 0; i < this.Items.Count; i++)
            {
                if (!predicate.Invoke(this[i]))
                    continue;

                this.Items.Swap(i, this.Items.Count - 1);
                this.Items.RemoveAt(this.Items.Count - 1);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes every item that meets the given condition
        /// </summary>
        /// <returns>Removed at least one item</returns>
        private bool RemoveAll(System.Predicate<U> predicate)
        {
            int count = 0;

            for (int i = 0; i < this.Items.Count; i++)
            {
                if (!predicate.Invoke(this[i]))
                    continue;

                count++;
                this.Items.Swap(i, this.Items.Count - 1);
                this.Items.RemoveAt(this.Items.Count - 1);
            }

            return count > 0;
        }

        #endregion

        #region Inventory Operations

        /// <summary>
        /// Adds the content of the given inventory into this one
        /// </summary>
        /// <param name="quitOnFail">Exit after the first failure</param>
        /// <returns>At least one item failed to get added</returns>
        public bool Combine(Inventory<U> inventory, bool quitOnFail = false)
        {
            bool oneItemFailed = false;

            foreach (U data in inventory)
            {
                Item<U> asset = data.GetAsset<Item<U>>();

                // Add to stack
                oneItemFailed &= !this.AddToStack(asset, data);

                // Exit if fail and requested
                if (quitOnFail && oneItemFailed)
                    break;
            }

            return oneItemFailed;
        }

        /// <summary>
        /// Clears all the items in this inventory
        /// </summary>
        public void Clear() => this.Items.Clear();

        /// <summary>
        /// Creates a new inventory with the same items
        /// </summary>
        /// <remarks>
        /// The clone can be different if the original inventory has problems
        /// </remarks>
        /// <returns>New inventory</returns>
        public Inventory<U> Clone()
        {
            Inventory<U> clone = new();

            clone.Combine(this);

            return clone;
        }

        /// <summary>
        /// Checks and fixes inventory problems
        /// </summary>
        /// <returns>Fixed problems</returns>
        public bool Squish()
        {
            Debug.LogWarning("This action can be expensive. Consider using it only when there is a problem.");

            Inventory<U> clone = this.Clone();

            // If nothing changed, skip
            if (clone.Count == this.Count)
                return false;

            // Copy clone
            this.Clear();
            this.Combine(clone);

            return true;
        }

        #endregion

        #region ISerializationCallbackReceiver

        /// <inheritdoc/>
        public void OnBeforeSerialize() { /* NO CHECK TO DO HERE */ }

        /// <inheritdoc/>
        public void OnAfterDeserialize()
        {
            // Check for the existence of every item
            for (int i = this.Items.Count - 1; i >= 0; i--)
            {
                U data = this.Items[i];

                // Item should be registered
                // Data should not be corrupted
                if (Registry.GetItem(data, out Item<U> item) && !item.IsCorrupted(data))
                    continue;

                // Swap item to end and delete
                this.Items.Swap(i, this.Items.Count - 1);
                this.Items.RemoveAt(this.Items.Count - 1);
            }
        }

        #endregion

        #region IEnumarable

        /// <inheritdoc/>
        public IEnumerator<U> GetEnumerator() => this.Items.GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        #endregion
    }
}