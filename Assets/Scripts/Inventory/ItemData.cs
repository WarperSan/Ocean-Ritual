using System;

namespace Inventory
{
    /// <summary>
    /// Data saved by an item
    /// </summary>
    [Serializable]
    public class ItemData
    {
        /// <summary>
        /// Name used by the item to register
        /// </summary>
        public string Namespace;

        /// <summary>
        /// Fetches the asset defined by this data
        /// </summary>
        /// <returns>Fetched asset or null</returns>
        public T GetAsset<T>() where T : Item 
            => Registry.GetItem(this, out T item) ? item : null;
    }
}