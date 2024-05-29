using Extensions;
using Fishing;
using Map;
using System.Collections.Generic;
using UnityEngine;

namespace Singletons
{
    /// <summary>
    /// Class that manages the process of fishing
    /// </summary>
    public class FishingManager : Singleton<FishingManager>
    {
        [SerializeField]
        private FishingEnemyManager EnemyManager;

        [SerializeField]
        private FishingFishManager FishManager;

        [SerializeField]
        private FishingZoneManager ZoneManager;

        [SerializeField]
        private FishingBuoy Buoy;

        [SerializeField]
        private Transform Player;

        public void Create()
        {
            IEnumerable<Territory> territories = GetTerritories(this.transform.position, 5);

            this.EnemyManager.UpdateEnemies(territories.GetEnemies());
            this.FishManager.UpdateFishes(territories.GetFishes());
            this.ZoneManager.StartTracking(this.Player, this.Buoy.transform);
            this.Buoy.StartNew(10, 2);
        }

        public void End()
        {
            // Clear all enemies
            // Collect buoy
            List<Fish> fishes = this.Buoy.ObtainInventory();

            foreach (Fish item in fishes)
                Debug.Log(item.DisplayName);

            // Disable self
            this.gameObject.SetActive(false);
        }

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        private void Start()
        {
            this.Create();
        }

        // Manage zones

        #region Territories

        /// <summary>
        /// Finds the territories near the given point
        /// </summary>
        /// <param name="origin">Origin of the check</param>
        /// <param name="radius">Maximum distance of the check</param>
        /// <returns>Territories found</returns>
        private static IEnumerable<Territory> GetTerritories(Vector3 origin, float radius)
        {
            var territories = new List<Territory>();

            // Fetch near colliders
            Collider[] colliders = Physics.OverlapSphere(origin, radius);

            if (colliders == null)
                return territories;

            // Fetch near territories
            foreach (Collider collider in colliders)
            {
                // If no territoryscript found, skip
                if (!collider.TryGetComponent(out Territory territory))
                    continue;

                territories.Add(territory);
            }

            return territories;
        }

        #endregion

        #region Singleton

        /// <inheritdoc/>
        protected override bool DestroyOnLoad => true;

        #endregion
    }
}