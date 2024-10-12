using System.Collections.Generic;
using UnityEngine;

namespace FishingModule.UI
{
    public class FishNewsManager : MonoBehaviour
    {
        #region Parameters

        [Header("Parameters")]
        [SerializeField, Tooltip("Prefab of an entry")]
        private GameObject fishNewsEntryPrefab;

        [SerializeField, Tooltip("Parent of the entries")]
        private Transform entryContainer;

        #endregion

        private void AddNews(Dictionary<FishSO, uint> fishes)
        {
            foreach (KeyValuePair<FishSO, uint> item in fishes)
            {
                GameObject newEntry = Instantiate(
                    this.fishNewsEntryPrefab,
                    this.entryContainer
                );

                // If missing script, delete and cancel
                if (!newEntry.TryGetComponent(out FishNewsEntry entry))
                {
                    Destroy(newEntry);
                    return;
                }

                // Set up
                entry.Set(item.Key, item.Value);
            }
        }

        #region MonoBehaviour

        /// <inheritdoc/>
        private void Awake()
        {
            if (this.fishNewsEntryPrefab == null)
                return;

            if (this.entryContainer == null)
                return;

            FishingManager manager = FindObjectOfType<FishingManager>();

            if (manager != null)
                manager.OnFishCaught += this.AddNews;
        }

        #endregion
    }
}