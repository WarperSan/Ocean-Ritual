using ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;

namespace Extensions
{
    public static class TerritoryExtension
    {
        /// <summary>
        /// Finds all the unique fishes in the given territories
        /// </summary>
        /// <param name="territories">Territories to check</param>
        /// <returns>Fishes found</returns>
        public static Fish[] GetFishes(this IEnumerable<TerritoryScript> territories)
        {
            HashSet<Fish> fishes = new();

            // Finds all the unique fishes
            foreach (TerritoryScript territory in territories)
            {
                foreach (Fish item in territory.GetFishes())
                    fishes.Add(item);
            }

            // Copy to array
            var result = new Fish[fishes.Count];
            fishes.CopyTo(result);

            return result;
        }

        /// <summary>
        /// Finds all the unique enemies in the given territories
        /// </summary>
        /// <param name="territories">Territories to check</param>
        /// <returns>Enemies found</returns>
        public static Enemy[] GetEnemies(this IEnumerable<TerritoryScript> territories)
        {
            HashSet<Enemy> enemies = new();

            // Finds all the unique enemies
            foreach (TerritoryScript territory in territories)
            {
                foreach (GameObject item in territory.GetEnemies())
                {
                    if (!item.TryGetComponent(out Enemy enemy))
                        continue;

                    enemies.Add(enemy);
                }
            }

            // Copy to array
            var result = new Enemy[enemies.Count];
            enemies.CopyTo(result);

            return result;
        }
    }
}