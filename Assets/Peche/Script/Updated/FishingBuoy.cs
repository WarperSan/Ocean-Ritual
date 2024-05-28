using ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing
{
    public class FishingBuoy : MonoBehaviour
    {
        [SerializeField]
        private uint RemainingWeight = 0;

        [SerializeField]
        private List<Fish> BuoyInventory = new();

        [SerializeField]
        private int amountToKeep = 2;

        /// <summary>
        /// Resets the buoy to start a new session
        /// </summary>
        /// <param name="weight">Total weight available</param>
        public void StartNew(uint weight)
        {
            this.BuoyInventory.Clear();
            this.RemainingWeight = weight;
        }

        #region Catch

        /// <summary>
        /// Tries to add the given fishes to the buoy
        /// </summary>
        public void AddFishes(params Fish[] fishes)
        {
            if (!this.UpdateQuantity(fishes, out List<Fish> validFishes))
                return;

            foreach (Fish fish in validFishes)
                BuoyInventory.Add(fish);
        }

        /// <summary>
        /// Filters the fishes that can enter the buoy
        /// </summary>
        /// <param name="fishes">All the fishes picked up</param>
        /// <param name="validFishes">Fishes obtained</param>
        /// <returns>Succeed to fish something</returns>
        private bool UpdateQuantity(Fish[] fishes, out List<Fish> validFishes)
        {
            validFishes = new();

            uint validTotalWeight = 0;

            int count = fishes.Length;

            for (int i = 0; i < this.amountToKeep; i++)
            {
                int randomIndex = Random.Range(0, count);
                Fish fish = fishes[randomIndex];

                if (RemainingWeight - (validTotalWeight - fish.Weight) >= 0)
                {
                    validFishes.Add(fish);
                    validTotalWeight += fish.Weight;
                }

                count--;

                // Swap last with selected
                (fishes[count], fishes[randomIndex]) = (fishes[randomIndex], fishes[count]);

                if (count <= 0)
                    break;
            }

            if (validTotalWeight != 0)
            {
                RemainingWeight -= validTotalWeight;
                return true;
            }

            return false;
        }

        /// <returns>Current inventory of the buoy</returns>
        public List<Fish> ObtainInventory() => this.BuoyInventory;

        #endregion
    }
}