using System.Collections.Generic;
using UnityEngine;

namespace FishingModule
{
    class FishingManager : MonoBehaviour
    {
        // Start fishing
        // End fishing

        private void Start() {
            this.StartFishing();
        }

        public void StartFishing()
        {
            List<Territory> territories = Territory.Near(
                this.GetTerritoryCheckOrigin(),
                this.territoryCheckRadius,
                this.territoryLayer
            );

            this.StartBuoy(Territory.Fishes(territories));
        }

        #region Buoy

        [Header("Buoy")]
        [SerializeField, Tooltip("Prefab for the buoy")]
        private GameObject buoyPrefab;

        private FishingBuoy _buoy = null;

        private bool StartBuoy(List<Territory.FishPercent> fishesToCatch)
        {
            // Spawn buoy
            if (_buoy == null)
            {
                _buoy = Instantiate(buoyPrefab).GetComponent<FishingBuoy>();
            }

            // If invalid, skip
            if (_buoy == null)
            {
                Debug.LogError("Could not find nor create the buoy.");
                return false;
            }

            // Set up buoy
            _buoy.SetFishesAvailable(fishesToCatch);
            return true;
        }

        #endregion

        #region Territories

        [Header("Territories")]
        [SerializeField, Tooltip("Determines the layers on which the territories' colliders are")]
        private LayerMask territoryLayer;

        [SerializeField, Tooltip("Determines how far from the check goes")]
        private float territoryCheckRadius;

        /// <returns>From where the territory check starts</returns>
        private Vector3 GetTerritoryCheckOrigin() => this.transform.position;

        #endregion

        #region Editor

#if UNITY_EDITOR
        /// <inheritdoc/>
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(this.GetTerritoryCheckOrigin(), this.territoryCheckRadius);
        }
#endif

        #endregion
    }
}