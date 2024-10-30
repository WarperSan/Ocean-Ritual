using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FishingModule
{
    public class FishingManager : MonoBehaviour
    {
        [SerializeField]
        private SpawnManager spawnManager;

        #region Fishing

        public void StartFishing()
        {
            // If buoy exists, skip
            if (this._buoy != null)
                return;

            List<Territory> territories = Territory.Near(
                this.GetTerritoryCheckOrigin(),
                this.territoryCheckRadius,
                this.territoryLayer
            );

            Dictionary<FishSO, float> fishes = Territory.Fishes(territories);
            Dictionary<GameObject, float> enemies = Territory.Enemies(territories);

            //foreach (KeyValuePair<FishSO, float> item in fishes)
            //    Debug.Log(item.Key.name + ": " + item.Value + "%");

            this.StartBuoy(fishes, enemies);
        }

        public void EndFishing()
        {
            bool isCollected = this._buoy != null && this._buoy.isCollected;

            // <Success>
            if (isCollected)
            {
              foreach (KeyValuePair<FishSO, uint> fish in this._buoy.GetFishCaught())
                    Inventory.Instance.AddItem(new FishData(fish.Key, (int) fish.Value));
            }
            else
            {
                // <Failure>
                Debug.Log("Player has failed the fishing!");
            }

            spawnManager.DespawnEnemies();
            // Destroy buoy
            Destroy(this._buoy.gameObject);
        }

        #endregion

        #region IInteractable

        public void OnInteraction()
        {
            if (this._buoy == null)
            {
                this.StartFishing();
            }
            else if (Vector3.Distance(this.transform.position, this._buoy.transform.position) <= 20f)
            {
                this.CollectBuoy();
            }
        }

        #endregion

        #region Buoy

        [Header("Buoy")]
        [SerializeField, Tooltip("Prefab for the buoy")]
        private GameObject buoyPrefab;

        [SerializeField]
        private Transform buoyHolder;

        public event FishingBuoy.FishCaught OnFishCaught;

        private FishingBuoy _buoy = null;

        private bool StartBuoy(Dictionary<FishSO, float> fishesToCatch, Dictionary<GameObject, float> enemies)
        {
            // Spawn buoy
            if (this._buoy == null)
            {
                this._buoy = Instantiate(this.buoyPrefab).GetComponent<FishingBuoy>();
                this._buoy.transform.position = this.GetTerritoryCheckOrigin();

                // Add callback
                this._buoy.OnFishCaught += f => this.OnFishCaught?.Invoke(f);

                // Set up ZoneManager
                if (this._buoy.TryGetComponent(out ZoneManager zoneManager))
                {
                    zoneManager.SetTarget(this.transform);
                    zoneManager.manager = this;
                }

                // Set up SpawnManager
                if (this._buoy.TryGetComponent(out SpawnManager spawnManager))
                {
                    spawnManager.SetUp(this._buoy.transform.position, enemies);
                }
            }

            // If invalid, skip
            if (this._buoy == null)
            {
                Debug.LogError("Could not find nor create the buoy.");
                return false;
            }

            // Set up buoy
            return this._buoy.SetFishesAvailable(fishesToCatch);
        }

        private void CollectBuoy()
        {
            if (this._buoy == null)
                return;

            this._buoy.isCollected = true;
            this._buoy.transform.SetParent(this.buoyHolder, false);
            this._buoy.transform.localPosition = Vector3.zero;
        }

        #endregion

        #region Territories

        [Header("Territories")]
        [SerializeField, Tooltip("Determines the layers on which the territories' colliders are")]
        private LayerMask territoryLayer;

        [SerializeField, Tooltip("Determines how far from the check goes")]
        private float territoryCheckRadius;

        [SerializeField]
        private Vector3 territoryCheckOffset;

        /// <returns>From where the territory check starts</returns>
        private Vector3 GetTerritoryCheckOrigin()
        {
            Vector3 pos = this.transform.position + this.territoryCheckOffset;

            // Remove Y
            pos.y = 0;

            return pos;
        }

        #endregion

        #region Editor

#if UNITY_EDITOR
        /// <inheritdoc/>
        private void OnDrawGizmosSelected()
        {
            Handles.color = Color.cyan;
            Handles.DrawSolidDisc(this.GetTerritoryCheckOrigin(), Vector3.up, this.territoryCheckRadius);
        }

        /// <inheritdoc/>
        private void OnValidate()
        {
            // Prevent vertical offset
            if (this.territoryCheckOffset.y != 0)
            {
                Debug.LogWarning("The territory check is executed at Y = 0. You cannot put a vertical offset.");
                this.territoryCheckOffset.y = 0;
            }
        }

#endif

        #endregion
    }
}