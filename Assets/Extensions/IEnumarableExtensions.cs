
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
    }
}