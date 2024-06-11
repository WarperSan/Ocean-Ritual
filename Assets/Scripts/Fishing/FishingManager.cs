using Attributes;
using Extensions;
using Fishing;
using Map;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing
{
    /// <summary>
    /// Class that manages the process of fishing
    /// </summary>
    public class FishingManager : Singletons.Singleton<FishingManager>
    {
        [SerializeField]
        private FishingZoneManager ZoneManager;

        [SerializeField]
        private FishingBuoy Buoy;

        [SerializeField]
        private Transform Player;

        public void Create()
        {
            IEnumerable<Territory> territories = GetTerritories(
                this.GetTerritoryCheckOrigin(), 
                this.territoryCheckRadius, 
                this.territoryLayer
            );

            this.SetUpEnemyManager(territories);
            this.SetUpFishManager(territories);

            this.ZoneManager.StartTracking(this.Player, this.Buoy.transform);
            this.Buoy.StartNew(10, 2);
        }

        public void End()
        {
            // Clear all enemies
            // Collect buoy
            Save.SaveManager.Load(1);

            Save.SaveData save = Save.SaveManager.LoadFromCache();

            save.Fishes.Combine(this.Buoy.GetInventory());

            Save.SaveManager.SaveToCache(save);
            Save.SaveManager.Save(1, true);

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

        #region Territories

        [Header("Territories")]
        [SerializeField, Layer, Tooltip("Determines the layers on which the territories' colliders are")]
        private LayerMask territoryLayer;

        [SerializeField, Tooltip("Determines how far from the check goes")]
        private float territoryCheckRadius;

        /// <summary>
        /// Finds the territories near the given point
        /// </summary>
        /// <param name="origin">Origin of the check</param>
        /// <param name="radius">Maximum distance of the check</param>
        /// <returns>Territories found</returns>
        private static IEnumerable<Territory> GetTerritories(Vector3 origin, float radius, int layerMask)
        {
            var territories = new List<Territory>();

            // Fetch near colliders
            Collider[] colliders = Physics.OverlapSphere(origin, radius, 1 << layerMask);

            if (colliders == null)
                return territories;

            // Fetch near territories
            foreach (Collider collider in colliders)
            {
                Territory territory = collider.GetComponentInParent<Territory>();

                // If no territoryscript found, skip
                if (territory == null)
                    continue;

                territories.Add(territory);
            }

            return territories;
        }

        /// <returns>From where the territory check starts</returns>
        private Vector3 GetTerritoryCheckOrigin() => this.transform.position;

        #endregion

        #region Fish Manager

        [Header("Fish Manager")]
        [SerializeField]
        private FishingFishManager fishManager;

        [SerializeField, Tooltip("How many fishes to pick per call")]
        private int quantity = 5;

        [SerializeField, Tooltip("How many seconds to wait between each call")]
        private float cooldown = 0.01f;

        private void SetUpFishManager(IEnumerable<Territory> territories)
        {
            this.fishManager.UpdateFishes(territories.GetFishes());
            this.fishManager.SetBuoy(this.Buoy);
            this.fishManager.SetCollectionStats(this.quantity, this.cooldown);
        }

        #endregion

        #region Enemy Manager

        [Header("Enemy Manager")]
        [SerializeField]
        private FishingEnemyManager enemyManager;

        [SerializeField, Tooltip("Determines how fast the probabilities transfer between levels")] 
        private float transferRate = 1f;
        
        [SerializeField] 
        private float retentionRate = 0.3f;

        [SerializeField, Tooltip("Determines the time between the spawning attempts")] 
        private float spawnDelay = 0.01f;

        [SerializeField, Tooltip("Determines the radius of the spawn origin")]
        private float spawnRadius = 5f;

        private void SetUpEnemyManager(IEnumerable<Territory> territories)
        {
            this.enemyManager.UpdateEnemies(territories.GetEnemies());
            this.enemyManager.SetRates(this.transferRate, this.retentionRate);
            this.enemyManager.SetSpawnSettings(this.spawnDelay, this.spawnRadius);
        }

        #endregion

        #region Singleton

        /// <inheritdoc/>
        protected override bool DestroyOnLoad => true;

        #endregion
    
        #region Gizmos

        /// <inheritdoc/>
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(this.GetTerritoryCheckOrigin(), this.territoryCheckRadius);
        }

        #endregion
    }
}