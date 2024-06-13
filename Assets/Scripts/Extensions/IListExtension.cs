using System.Collections.Generic;

namespace Extensions
{
    public static class IListExtension
    {
        /// <summary>
        /// Swaps the position of the two given items
        /// </summary>
        /// <param name="a">Index of the first item</param>
        /// <param name="b">Inedx of the second item</param>
        /// <returns>Succeed to swap</returns>
        public static bool Swap<T>(this IList<T> array, int a, int b)
        {
            // If indexes not valid, skip
            if (a < 0 || b < 0 || a >= array.Count || b >= array.Count)
                return false;

            // If same position, skip
            if (a == b)
                return true;

            // Swap
            (array[a], array[b]) = (array[b], array[a]);
            return true;
        }
  
        /// <summary>
        /// Creates a string containing every element of the array, separated with the given value
        /// </summary>
        /// <param name="separator">Separator used</param>
        /// <returns>Result</returns>
        public static string Join<T>(this IList<T> array, string separator)
        {
            string result = "";
            int size = array.Count;
            for (int i = 0; i < size; i++)
            {
                result += array[i].ToString();

                if (i != size - 1)
                    result += separator;
            }

            return result;
        }
    
        /// <summary>
        /// Fetches a random item in the given array
        /// </summary>
        /// <param name="index">Index of the item fetched</param>
        /// <returns>Item fetched</returns>
        public static T Random<T>(this IList<T> array, out int index) 
        {
            int amount = array.Count;

            if (amount == 0)
            {
                index = -1;
                return default;
            }

            index = amount == 1 ? 0 : Utils.Random.RandomToMax(array.Count);

            return array[index];
        }
    }
}