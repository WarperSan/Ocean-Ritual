using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class Registry
    {
        private static readonly Dictionary<string, Item> registeredItems = new();

        public static void FetchAll()
        {
            Item[] items = Resources.LoadAll<Item>("Items");

            foreach (Item item in items)
                Register(item);
        }

        /// <summary>
        /// Registers the given item
        /// </summary>
        /// <returns>Success of the register</returns>
        private static bool Register(Item item)
        {
            string key = item.Namespace;

            // Check if exist
            if (registeredItems.ContainsKey(key))
            {
                Debug.LogWarning($"An item already exist under the name '{key}'.");
                return false;
            }

            registeredItems.Add(key, item);
            Debug.Log($"Registered '{item.name}' with '{key}'.");
            return true;
        }

        #region Item

        /// <summary>
        /// Fetches the item with the given namespace
        /// </summary>
        /// <returns>Found a valid item</returns>
        public static bool GetItem<T>(string @namespace, out T item) where T : Item
        {
            // If item not found, skip
            if (!registeredItems.TryGetValue(@namespace, out Item i))
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

        /// <summary>Fetches and loads the item with the given data</summary>
        public static bool GetLoadedItem<T>(ItemData data, out T item) where T : Item
        {
            // If item not found, skip
            if (!GetItem(data.Namespace, out item))
            {
                item = default;
                return false;
            }

            item = Object.Instantiate(item);

            return item.Load(data);
        }

        #endregion
    }
}