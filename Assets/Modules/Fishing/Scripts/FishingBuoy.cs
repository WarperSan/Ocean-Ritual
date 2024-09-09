using ExtensionsModule;
using System.Collections.Generic;
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

        private readonly List<FishSO> fishesCaught = new();
        private readonly List<FishSO> fishCatalog = new();

        /// <summary>
        /// Determines if the buoy is full
        /// </summary>
        private bool IsFull() => fishesCaught.Count >= maxFishCount;

        /// <summary>
        /// Catches a fish
        /// </summary>
        private void CatchFish()
        {
            // Random amount
            int amount = Random.Range(maxFishAtOnce.x, maxFishAtOnce.y);
            bool caughtSomething = false;

            for (; amount > 0; amount--)
            {
                // Add fish
                FishSO caught = fishCatalog.Random(out _);

                if (caught == null)
                    return;

                fishesCaught.Add(caught);
                caughtSomething = true;

                // If became full, enable indicator
                if (this.IsFull())
                {
                    fullIndicator.SetActive(true);
                    break;
                }
            }

            if (caughtSomething)
            {
                onCaughtEffects.Play();
            }
        }

        #endregion

        #region Effects

        [Header("Effects")]
        [SerializeField]
        private ParticleSystem onCaughtEffects;

        [SerializeField]
        private GameObject fullIndicator;

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

        #region Set up

        /// <summary>
        /// Sets the fishes catchable
        /// </summary>
        public void SetFishesAvailable(List<FishSO> fishes)
        {
            this.fishCatalog.Clear();
            this.fishCatalog.AddRange(fishes);
        }

        #endregion

        #region MonoBehaviour

        /// <inheritdoc/>
        private void Update()
        {
            // If full, skip
            if (this.IsFull())
                return;

            // If delay not finished, skip
            if (!this.ProcessDelay(Time.deltaTime))
                return;

            this.ResetDelay();
            this.CatchFish();
        }

        /// <inheritdoc/>
        private void OnEnable() => fullIndicator.SetActive(false);

        #endregion
    }
}