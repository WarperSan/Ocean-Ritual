using System.Collections.Generic;
using UnityEngine;

namespace InventoryModule
{
    /// <summary>
    /// Class that links items to assets
    /// </summary>
    public class Registry
    {
        #region Item

        /// <summary>
        /// Fetches an item that corresponds to the given data
        /// </summary>
        /// <param name="data">Data of the wanted item</param>
        /// <param name="item">Item found</param>
        /// <returns>Succeed to find an item</returns>
        public static bool GetItem<T>(ItemData data, out T item) where T : Item
        {
            // If wasn't fetch
            if (!wasLoaded)
            {
                Debug.LogWarning("Consider loading the registry before trying to access it.");
                Load();
            }

            // Prevent invalid namespaces
            data.Namespace ??= "";

            // If item not found, skip
            if (!registeredItems.TryGetValue(data.Namespace, out Item i))
            {
                item = default;
                return false;
            }

            // If item correct type, return
            if (i is T it)
            {
                item = it;
                return true;
            }

            item = default;
            return false;
        }

        /// <summary>
        /// Creates a copy of the item defined by the given data and loads the given data
        /// </summary>
        /// <returns>Succeed to create and load</returns>
        public static bool CreateInstance<T>(ItemData data, out T instance) where T : Item
        {
            // If item not found, skip
            if (!GetItem(data, out T item))
            {
                instance = default;
                return false;
            }

            return CreateInstance(item, data, out instance);
        }

        /// <summary>
        /// Creates a copy of the given instance and loads it with the given data
        /// </summary>
        /// <returns>Succeed to create and load</returns>
        public static bool CreateInstance<T>(T original, ItemData data, out T instance) where T : Item
        {
            instance = Object.Instantiate(original);
            return instance.Load(data);
        }

        #endregion

        #region Register

        private static readonly Dictionary<string, Item> registeredItems = new();

        /// <summary>
        /// Registers the given item to the database
        /// </summary>
        /// <returns>Success of the register</returns>
        private static bool Register(Item item)
        {
            string key = (item.Namespace ?? "").Trim();

#if UNITY_EDITOR
            if (!key.Equals(item.Namespace))
            {
                Debug.LogWarning($"A namespace was changed: '{item.Namespace}' => '{key}'.");          
            }
#endif

            // If invalid namespace
            if (key.Length == 0)
            {
                Debug.LogWarning($"Invalid namespace for '{item.name}'.");
                return false;
            }

            // Check if already exist
            if (registeredItems.ContainsKey(key))
            {
                Debug.LogWarning($"An item already exist under the name '{key}'.");
                return false;
            }

            registeredItems.Add(key, item);
            Debug.Log($"Registered '{item.name}' with '{key}'.");
            return true;
        }

        #endregion
    
        #region Load

        private static bool wasLoaded = false;

        /// <summary>
        /// Loads all the items in the folder 'Items'
        /// </summary>
        public static void Load()
        {
            Item[] items = Resources.LoadAll<Item>("Items");

            foreach (Item item in items)
                Register(item);

            Debug.Log(items.Length + " items loaded.");
            wasLoaded = true;
        }

        #endregion
    }
}