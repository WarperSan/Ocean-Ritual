using Extensions;
using System.Collections.Generic;
using UnityEngine;
using static Utils.Random;

namespace Fishing
{
    public class FishingFishManager : MonoBehaviour
    {
        #region Probabilities

        private ProbabilityForLevel[] probabilities;
        private IEnumerable<Fish> fishes;

        /// <summary>
        /// Updates the probabilities for the given fishes
        /// </summary>
        public void UpdateFishes(IEnumerable<Fish> fishes)
        {
            // Update odds
            IEnumerable<int> levels = fishes.GetUniques(f => (int)f.Rarity);
            this.probabilities = ProbabilitiesForLevels(levels);

            // Update fishes
            this.fishes = fishes;
        }

        /// <summary>
        /// Finds the given amount of fishes 
        /// </summary>
        /// <param name="amount">How many fishes to pick</param>
        /// <returns>Fishes picked</returns>
        private Fish[] GetRandomFishes(int amount) => FindFishesForRarity(
            this.fishes,
            this.GetRandomLevel(),
            amount
        );

        /// <summary>
        /// Finds the given amount of fishes of the given rarity in the given range
        /// </summary>
        /// <param name="fishes">Fishes to pick from</param>
        /// <param name="rarity">Rarity to have</param>
        /// <param name="amount">How many fishes to pick</param>
        /// <returns>Fishes picked</returns>
        private static Fish[] FindFishesForRarity(IEnumerable<Fish> fishes, Rarity rarity, int amount)
        {
            // If fishes invalid, skip
            if (fishes == null)
                return System.Array.Empty<Fish>();

            // Find all with given level
            Fish[] fishesWithLevel = fishes.Where(f => f.Rarity == rarity);

            // If no ennemy found
            if (fishesWithLevel.Length == 0)
                return fishesWithLevel;

            var selected = new Fish[amount];

            // Select random fishes
            for (int i = 0; i < amount; i++)
                selected[i] = fishesWithLevel.Random(out _);

            return selected;
        }

        /// <summary>
        /// Finds a random level
        /// </summary>
        private Rarity GetRandomLevel()
        {
            if (this.probabilities == null)
                return default;

            int level = this.probabilities.GetRandomLevel();
            return level != -1 ? (Rarity)level : default;
        }

        #endregion

        #region Buoy

        private FishingBuoy buoy;

        /// <summary>
        /// Sets the target buoy of this manager
        /// </summary>
        public void SetBuoy(FishingBuoy buoy) => this.buoy = buoy;

        #endregion

        #region Collect

        private int quantity;

        private float cooldown;
        private float delay;

        /// <summary>
        /// Sets the different stats of the current session
        /// </summary>
        /// <param name="quantity">How many fishes to pick per call</param>
        /// <param name="cooldown">How many seconds to wait between each call</param>
        public void SetCollectionStats(int quantity, float cooldown)
        {
            this.quantity = quantity;
            this.cooldown = cooldown;
            this.delay = cooldown;
        } 

        /// <summary>
        /// Updates the collection of the fishes
        /// </summary>
        /// <param name="elapsed">Time passed since the last frame</param>
        private void UpdateCollection(float elapsed)
        {
            // If delay not finished
            if (this.delay > 0)
            {
                this.delay -= elapsed;
                return;
            }

            // Reset delay
            this.delay = this.cooldown;

            // If quantity invalid
            if (this.quantity <= 0)
            {
                Debug.Log("The number of fishes available is less or equal to 0.");
                return;
            }

            this.CollectFishes(this.quantity);
        }

        /// <summary>
        /// Collects the given amount of fishes
        /// </summary>
        /// <param name="amount">How many fishes to get</param>
        private void CollectFishes(int amount)
        {
            // Get random fishes
            Fish[] fishes = this.GetRandomFishes(amount);

            // Add to buoy
            this.buoy.AddFishes(fishes);
        }

        #endregion
    
        #region MonoBehaviour

        /// <inheritdoc/>
        private void Update() => this.UpdateCollection(Time.deltaTime);

        #endregion
    }
}