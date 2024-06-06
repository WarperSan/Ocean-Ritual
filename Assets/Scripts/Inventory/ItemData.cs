namespace Inventory
{
    /// <summary>
    /// Data saved for an item
    /// </summary>
    [System.Serializable]
    public struct ItemData
    {
        /// <summary>
        /// Unique name for the wanted item
        /// </summary>
        public string Namespace;

        /// <summary>
        /// Extra data for this item
        /// </summary>
        public string ExtraData;
    }
}