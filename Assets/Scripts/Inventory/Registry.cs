using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class Registry
    {
        public static void FetchAll()
        {
            Item[] items = Resources.LoadAll<Item>("Items");

            foreach (Item item in items)
                Register(item);

            Debug.Log(items.Length + " items loaded.");
        }

        #region Item

        /// <summary>
        /// Fetches an item that corresponds to the given data
        /// </summary>
        /// <param name="data">Data of the wanted item</param>
        /// <param name="item">Item found</param>
        /// <returns>Succeed to find an item</returns>
        public static bool GetItem<T>(ItemData data, out T item) where T : Item
        {
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
        /// Fetches an item and loads an instance with the given data
        /// </summary>
        /// <param name="data">Data to load from</param>
        /// <param name="item">Loaded instance</param>
        /// <returns>Succeed to load</returns>
        public static bool GetLoadedItem<T>(ItemData data, out T item) where T : Item
        {
            // If item not found, skip
            if (!GetItem(data, out item))
            {
                item = default;
                return false;
            }

            item = Object.Instantiate(item);

            return item.Load(data);
        }

        /// <summary>
        /// Fetches the item and only loads the extra data 
        /// </summary>
        /// <remarks>
        /// This does not check if the data is corrupted 
        /// </remarks>
        /// <param name="data">Data to load from</param>
        /// <param name="extraData">Loaded extra data</param>
        /// <returns>Succeed to load</returns>
        public static bool GetExtraData<T>(ItemData data, out T extraData)
        {
            // If item not found, skip
            if (!GetItem<Item>(data, out _))
            {
                extraData = default;
                return false;
            }

            try
            {
                extraData = Item<T>.FromJson(data.ExtraData);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error while parsing extra data to '{typeof(T).Name}': {e.Message}");
                extraData = default;
                return false;            
            }

            return true;
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
    }
}