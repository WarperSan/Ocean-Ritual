using ExtensionsModule;
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

        [SerializeField]
        private List<FishSO> fishesCaught = new();
        private readonly Dictionary<float, List<FishSO>> fishCatalog = new();

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
                FishSO caught = this.GetRandom();

                if (caught == null)
                    return;

                fishesCaught.Add(caught);
                caughtSomething = true;

                // If became full, enable indicator
                if (this.IsFull())
                {
                    fullIndicator.SetActive(true);

                    for (int i = fishCatalog.Keys.Count - 1; i >= 0; i--)
                    {
                        Debug.Log(fishCatalog.Keys.ElementAt(i));
                        foreach (FishSO v in fishCatalog[fishCatalog.Keys.ElementAt(i)])
                        {
                            Debug.Log(v.Name + ": " + fishesCaught.Where(f => f == v).Count());
                        }
                    }

                    break;
                }
            }

            if (caughtSomething)
            {
                onCaughtEffects.Play();
            }
        }

        private FishSO GetRandom()
        {
            float chance = Random.Range(0, fishCatalog.Keys.Sum());

            foreach (KeyValuePair<float, List<FishSO>> item in fishCatalog)
            {
                if (item.Key >= chance)
                    return item.Value.Random(out _);

                chance -= item.Key;
            }

            return fishCatalog.Last().Value.Random(out _);
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
            IEnumerable<IGrouping<float, FishSO>> groups = fishes.GroupBy(f => f.Chance).OrderByDescending(g => g.Key);

            this.fishCatalog.Clear();

            foreach (IGrouping<float, FishSO> group in groups)
            {
                List<FishSO> f = new();
                foreach (FishSO item in group)
                    f.Add(item);

                this.fishCatalog.Add(group.Key, f);
            }
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