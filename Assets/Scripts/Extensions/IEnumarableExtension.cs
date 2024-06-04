
using System;
using System.Collections.Generic;
using System.Linq;
using static Utils.Random;

namespace Extensions
{
    public static class IEnumarableExtension
    {
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            // Skip if action is invalid
            if (action == null)
                return;

            foreach (T item in items)
                action.Invoke(item);
        }
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

        public static IEnumerable<T> GetUniques<T, U>(this IEnumerable<U> values, Func<U, T> action)
        {
            // Skip if action is invalid
            if (action == null)
                return default;

            HashSet<T> unique = new();

            foreach (U item in values)
                unique.Add(action.Invoke(item));

            // Copy to array
            var result = new T[unique.Count];
            unique.CopyTo(result);

            return result;
        }

        public static T Random<T>(this IEnumerable<T> array, out int index) 
        {
            int amount = array.Count();

            if (amount == 0)
            {
                index = -1;
                return default;
            }

            index = amount == 1 ? 0 : RandomToMax(array.Count());

            return array.ElementAt(index);
        }
 
        public static string Join<T>(this IEnumerable<T> array, string separator)
        {
            string result = "";
            int size = array.Count();
            for (int i = 0; i < size; i++)
            {
                result += array.ElementAt(i).ToString();

                if (i != size - 1)
                    result += separator;
            }

            return result;
        }
    }
}