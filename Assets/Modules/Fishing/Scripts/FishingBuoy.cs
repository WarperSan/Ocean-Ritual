using ExtensionsModule;
using System.Collections.Generic;
using UnityEngine;

namespace FishingModule
{
    public class FishingBuoy : MonoBehaviour
    {
        #region Statistics

        [Header("Statistics")]
        [SerializeField]
        private int maxFishCount;

        #endregion

        #region Inventory

        [Header("Inventory")]
        public List<FishSO> fishesCaught = new();
        private List<FishSO> fishCatalog = new();

        private bool IsFull() => fishesCaught.Count >= maxFishCount;
        private void CatchFish()
        {
            // Add fish
            FishSO caught = fishCatalog.Random(out _);

            if (caught == null)
                return;

            fishesCaught.Add(caught);
            onCaughtEffects.Play();

            // If became full, enable indicator
            if (this.IsFull())
                fullIndicator.SetActive(true);
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

        private void ResetDelay() => delayRemaining = Random.Range(delayRange.x, delayRange.y);


        #endregion

        #region Set up

        public void SetFishesAvailable(List<FishSO> fishes)
        {
            this.fishCatalog = fishes;
        }

        #endregion

        #region MonoBehaviour

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

        private void OnEnable() => fullIndicator.SetActive(false);

        #endregion
    }
}