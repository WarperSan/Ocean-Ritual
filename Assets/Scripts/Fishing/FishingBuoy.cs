using Extensions;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing
{
    public class FishingBuoy : MonoBehaviour
    {
        [SerializeField]
        private uint RemainingWeight = 0;

        [SerializeField]
        private Inventory.Inventory BuoyInventory = new();

        [SerializeField]
        private int Efficiency;

        /// <summary>
        /// Resets the buoy to start a new session
        /// </summary>
        /// <param name="weight">Total weight available</param>
        /// <param name="efficiency">Total weight available</param>
        public void StartNew(uint weight, int efficiency)
        {
            this.BuoyInventory.Clear();
            this.RemainingWeight = weight;
            this.Efficiency = efficiency;
        }

        #region Catch

        /// <summary>
        /// Tries to add the given fishes to the buoy
        /// </summary>
        public void AddFishes(params Fish[] fishes)
        {
            // Fetch the fishes that can fit
            List<Fish> validFishes = GetValids(fishes, this.RemainingWeight, this.Efficiency);

            // Add fishes to self
            foreach (Fish fish in validFishes)
            {
                this.BuoyInventory.Add(fish);
                this.RemainingWeight -= fish.Weight;
            }
        }

        /// <summary>
        /// Filters the fishes that can enter the buoy
        /// </summary>
        /// <param name="fishes">All the fishes picked up</param>
        /// <param name="maxWeight">Maximum weight</param>
        /// <param name="attemptCount">How many attempts to pick up a fish</param>
        /// <returns>Fishes obtained</returns>
        private static List<Fish> GetValids(Fish[] fishes, uint maxWeight, int attemptCount)
        {
            List<Fish> validFishes = new();

            uint validTotalWeight = 0;

            int count = fishes.Length;

            // Try to pick up X fishes
            for (int i = 0; i < attemptCount; i++)
            {
                Fish fish = fishes.Random(out int index);

                if (fish == null)
                    continue;

                // If fish can fit, add
                if (maxWeight >= (validTotalWeight + fish.Weight))
                {
                    validFishes.Add(fish);
                    validTotalWeight += fish.Weight;
                }

                count--;

                // Swap last with selected
                (fishes[count], fishes[index]) = (fishes[index], fishes[count]);

                if (count <= 0)
                    break;
            }

            return validFishes;
        }

        /// <returns>Current inventory of the buoy</returns>
        public Inventory.Inventory GetInventory() => this.BuoyInventory;

        #endregion
    }
}