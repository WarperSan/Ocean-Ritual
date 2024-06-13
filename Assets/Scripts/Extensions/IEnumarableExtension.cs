
using System;
using System.Collections.Generic;

namespace Extensions
{
    public static class IEnumarableExtension
    {
        /// <summary>
        /// Finds all the items that match the given condition
        /// </summary>
        public static T[] Where<T>(this IEnumerable<T> items, Func<T, bool> condition)
        {
            // If condition invalid, skip
            if (condition == null)
                return default;

            List<T> values = new();

            // Get all items that match the condition
            foreach (T item in items)
            {
                if (!condition.Invoke(item))
                    continue;

                values.Add(item);
            }

            return values.ToArray();
        }

        /// <summary>
        /// Finds all the unique items
        /// </summary>
        /// <param name="action"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <returns>All the unique items found</returns>
        public static IEnumerable<U> GetUniques<T, U>(this IEnumerable<T> array, Func<T, U> action)
        {
            // Skip if action is invalid
            if (action == null)
                return default;

            HashSet<U> unique = new();

            foreach (T item in array)
                unique.Add(action.Invoke(item));

            // Copy to array
            var result = new U[unique.Count];
            unique.CopyTo(result);

            return result;
        }
    }
}