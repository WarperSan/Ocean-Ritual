using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FishingModule
{
    public class FishingBuoy : MonoBehaviour
    {
        #region Inventory

        [Header("Inventory")]
        [SerializeField, Min(0), Tooltip("Maximum amount of fishes this buoy can hold")]
        private int maxFishCount;

        [SerializeField, Min(0), Tooltip("Determines how many fishes the buoy can catch at once")]
        private Vector2Int maxFishAtOnce;

        private readonly Dictionary<FishSO, int> fishesCaught = new();
        private List<(FishSO fish, float percent)> fishesToCatch = new();

        /// <summary>
        /// Catches a fish
        /// </summary>
        private void CatchFish()
        {
            // Random amount
            int amount = Random.Range(maxFishAtOnce.x, maxFishAtOnce.y);
            Dictionary<FishSO, int> fishCaught = new();

            for (; amount > 0; amount--)
            {
                // Add fish
                FishSO caught = this.GetRandom();

                if (caught == null)
                    return;

                if (!fishesCaught.ContainsKey(caught))
                    fishesCaught.Add(caught, 0);

                if (!fishCaught.ContainsKey(caught))
                    fishCaught.Add(caught, 0);

                fishesCaught[caught]++;
                fishCaught[caught]++;

                // If became full, enable indicator
                if (this.IsFull())
                {
                    this.BecomeFull();
                    break;
                }
            }

            if (fishCaught.Count > 0)
            {
                this.OnFishCaught(fishCaught);
                onCaughtEffects.Play();
            }
        }

        /// <summary>
        /// Determines if the buoy is full
        /// </summary>
        private bool IsFull() => fishesCaught.Sum(f => f.Value) > maxFishCount;

        private void BecomeFull()
        {
            fullIndicator.SetActive(true);
        }

        /// <summary>
        /// Obtains the fishes caught in this buoy
        /// </summary>
        public Dictionary<FishSO, int> GetFishCaught() => this.fishesCaught;

        #endregion

        #region Effects

        [Header("Effects")]
        [SerializeField]
        private ParticleSystem onCaughtEffects;

        [SerializeField]
        private GameObject fullIndicator;

        /// <summary>
        /// Called when fishes have been caught
        /// </summary>
        /// <param name="fishCaught">Fishes caught</param>
        public void OnFishCaught(Dictionary<FishSO, int> fishCaught) { }

        #endregion

        #region Delay

        [Header("Delay")]
        [SerializeField, Tooltip("Determines how long the buoy has to wait between catch")]
        private Vector2 delayRange;
        private float delayRemaining;

        /// <summary>
        /// Processes the current delay of the buoy
        /// </summary>
        /// <returns>The delay has ended</returns>
        private bool ProcessDelay(float elapsed)
        {
            if (delayRemaining <= elapsed)
                delayRemaining = 0;
            else
                delayRemaining -= elapsed;

            return delayRemaining <= 0;
        }

        /// <summary>
        /// Resets the current delay
        /// </summary>
        private void ResetDelay() => delayRemaining = Random.Range(delayRange.x, delayRange.y);

        #endregion

        #region Fishes

        [Header("Chance")]
        [SerializeField, Min(1f)]
        private float ChanceDividor = 1000f;
        private float ChanceFactor = 1;

        /// <summary>
        /// Sets the fishes catchable
        /// </summary>
        /// <returns>
        /// Are the given fish valid?
        /// </returns>
        public bool SetFishesAvailable(Dictionary<FishSO, float> fishes)
        {
            this.fishesToCatch.Clear();

            foreach (KeyValuePair<FishSO, float> item in fishes)
                this.fishesToCatch.Add((item.Key, item.Value));

            this.fishesToCatch = this.fishesToCatch.OrderBy(f => f.percent).ToList();

            return this.fishesToCatch.Count > 0;
        }

        private FishSO GetRandom()
        {
            float chance = UtilsModule.Random.RandomPercent() / this.ChanceFactor;

            for (int i = 0; i < this.fishesToCatch.Count; i++)
            {
                (FishSO fish, float percent) item = this.fishesToCatch[i];

                if (i != this.fishesToCatch.Count - 1 && item.percent < chance)
                {
                    chance -= item.percent;
                    continue;
                }

                return item.fish;
            }

            return null;
        }

        #endregion

        #region MonoBehaviour

        /// <inheritdoc/>
        private void Update()
        {
            // If the buoy is already collected, skip the update
            if (this.isCollected)
                return;

            this.UpdateCollecting(Time.deltaTime);
        }

        #endregion

        #region Collecting

        [Header("Collecting")]
        public bool KeepCollecting;
        public bool isCollected;

        /// <summary>
        /// Updates the process of collecting fishes
        /// </summary>
        private void UpdateCollecting(float elapsed)
        {
            // If the buoy should not keep collecting, skip the update
            if (!this.KeepCollecting)
                return;

            // If the buoy is full, skip the update
            if (this.IsFull())
                return;

            // Increase chance for rare fishes
            this.ChanceFactor += elapsed / this.ChanceDividor;

            // If the delay is not finished, skip the update
            if (!this.ProcessDelay(elapsed))
                return;

            // Catch a fish
            this.ResetDelay();
            this.CatchFish();
        }

        #endregion
    }
}