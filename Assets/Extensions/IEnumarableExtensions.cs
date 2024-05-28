
using System;
using System.Collections.Generic;

namespace Extensions
{
    public static class IEnumarableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            // Skip if action is invalid
            if (action == null)
                return;

            foreach (T item in items)
                action.Invoke(item);
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
    }
}