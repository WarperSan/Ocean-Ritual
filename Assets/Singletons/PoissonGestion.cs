using Extensions;
using Fishing;
using ScriptableObjects;
using Singletons;
using System.Collections.Generic;
using UnityEngine;

namespace Singletons
{
    public class PoissonGestion : Singleton<PoissonGestion>
    {
        #region Singleton

        /// <inheritdoc/>
        protected override bool DestroyOnLoad => true;

        #endregion

        /// <inheritdoc/>
        private void Update() => this.UpdateCollection(Time.deltaTime);

        #region Probabilities

        private ProbabilityForLevel[] probabilities;
        private IEnumerable<Fish> fishes;

        /// <summary>
        /// Updates the probabilities for the given fishes
        /// </summary>
        public static void UpdateFishes(IEnumerable<Fish> fishes)
        {
            // Update odds
            IEnumerable<int> levels = fishes.GetUniques(f => (int)f.Rarity);
            Instance.probabilities = GestionProbabiliter.ProbabilitiesForLevels(levels);

            // Update fishes
            Instance.fishes = fishes;

            // Display self
            //Instance.enabled = false;
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
            List<Fish> fishesWithLevel = new();

            // Find all with given level
            foreach (Fish item in fishes)
            {
                if (item.Rarity != rarity)
                    continue;

                fishesWithLevel.Add(item);
            }

            // If no ennemy found
            if (fishesWithLevel.Count == 0)
                return System.Array.Empty<Fish>();

            var selected = new Fish[amount];

            // Select random fishes
            for (int i = 0; i < amount; i++)
                selected[i] = fishesWithLevel[Random.Range(0, fishesWithLevel.Count)];

            return selected;
        }

        /// <summary>
        /// Finds a random level
        /// </summary>
        private Rarity GetRandomLevel()
        {
            float randomValue = Random.Range(0f, 100f);

            float cumulativeProbability = 0f;
            foreach (ProbabilityForLevel prob in this.probabilities)
            {
                cumulativeProbability += prob.Probability;

                if (randomValue <= cumulativeProbability)
                    return (Rarity)prob.Level;
            }

            // If no level found,
            return default;
        }


        #endregion

        #region Collect

        [SerializeField]
        private FishingBuoy FishingBuoy;

        [SerializeField]
        private int Quantity = 5;

        [SerializeField]
        private float StartDelay = 0.01f;

        [SerializeField]
        private float delay = 0.01f;

        /// <summary>
        /// Updates the collection of the fishes
        /// </summary>
        /// <param name="elapsed">Time passed since the last frame</param>
        private void UpdateCollection(float elapsed)
        {
            // If delay not finished
            if (this.StartDelay > 0)
            {
                this.StartDelay -= elapsed;
                return;
            }

            // Reset delay
            this.StartDelay = this.delay;

            // If quantity invalid
            if (this.Quantity <= 0)
            {
                Debug.Log("The number of fishes available is less or equal to 0.");
                return;
            }

            this.CollectFishes(this.Quantity);
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
            this.FishingBuoy.AddFishes(fishes);
        }

        #endregion
    }
}