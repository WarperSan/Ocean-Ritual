using System;

namespace Inventory
{
    /// <summary>
    /// Data saved for an item that can be stacked
    /// </summary>
    /// <typeparam name="T">Type of the data to compare to</typeparam>
    [Serializable]
    public class ItemStackData<T> : ItemData, IEquatable<ItemData> where T : ItemData
    {
        /// <summary>
        /// Amount of items in this stack
        /// </summary>
        public uint Amount;

        #region IEquatable

        /// <inheritdoc/>
        public bool Equals(ItemData other) 
        {
            // If other invalid
            if (other is null)
                return false;

            // If namespaces don't match
            if (!this.Namespace.Equals(other.Namespace))
                return false;

            // If types don't match
            if (other is not T item)
                return false;

            // Check if they are the same
            return this.IsSame(item);
        }

        /// <summary>
        /// Checks if the given item is the same as this item
        /// </summary>
        /// <returns>Both items are the same</returns>
        public virtual bool IsSame(T other) => true;

        #endregion
    }
}