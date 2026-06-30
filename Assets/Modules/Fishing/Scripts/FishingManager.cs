using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FishingModule
{
    public class FishingManager : MonoBehaviour
    {
        #region Fishing

        public void StartFishing()
        {
            // If buoy exists, skip
            if (_buoy != null)
                return;

            List<Territory> territories = Territory.Near(
                GetTerritoryCheckOrigin(),
                territoryCheckRadius,
                territoryLayer
            );

            Dictionary<FishSO, float> fishes = Territory.Fishes(territories);
            Dictionary<GameObject, float> enemies = Territory.Enemies(territories);

            //foreach (KeyValuePair<FishSO, float> item in fishes)
            //    Debug.Log(item.Key.name + ": " + item.Value + "%");

            StartBuoy(fishes, enemies);
            SetFishingModel(true);

            TargetGeneral.Instance.Target = TargetGeneral.Instance.BoatTarget;
        }

        public void EndFishing()
        {
            SetFishingModel(false);
            bool isCollected = _buoy != null && _buoy.isCollected;

            // <Success>
            if (isCollected)
            {
                foreach (KeyValuePair<FishSO, uint> fish in _buoy.GetFishCaught())
                    Inventory.Instance.AddItem(new FishData(fish.Key, (int)fish.Value));
            }
            else
            {
                // <Failure>
                Debug.Log("Player has failed the fishing!");
            }

            if (_buoy.gameObject.TryGetComponent(out SpawnManager spawnManager))
                spawnManager.DespawnEnemies();

            // Destroy buoy
            Destroy(_buoy.gameObject);
        }

        #endregion

        #region IInteractable

        public void OnInteraction()
        {
            if (_buoy == null)
                StartFishing();
            else if (Vector3.Distance(transform.position, _buoy.transform.position) <= 20f)
                CollectBuoy();
        }

        #endregion

        #region Buoy

        [Header("Buoy")]
        [SerializeField]
        [Tooltip("Prefab for the buoy")]
        private GameObject buoyPrefab;

        [SerializeField]
        private Transform buoyHolder;

        public event FishingBuoy.FishCaught OnFishCaught;

        private FishingBuoy _buoy;

        private bool StartBuoy(Dictionary<FishSO, float> fishesToCatch, Dictionary<GameObject, float> enemies)
        {
            // Spawn buoy
            if (_buoy == null)
            {
                _buoy = Instantiate(buoyPrefab).GetComponent<FishingBuoy>();
                _buoy.transform.position = GetTerritoryCheckOrigin() + new Vector3(0, 5, 0);

                // Add callback
                _buoy.OnFishCaught += f => OnFishCaught?.Invoke(f);

                // Set up ZoneManager
                if (_buoy.TryGetComponent(out ZoneManager zoneManager))
                {
                    zoneManager.SetTarget(transform);
                    zoneManager.manager = this;
                }

                // Set up SpawnManager
                if (_buoy.TryGetComponent(out SpawnManager spawnManager))
                {
                    Vector3 pos = _buoy.transform.position;
                    pos.y = 0;
                    spawnManager.SetUp(pos, enemies);
                }
            }

            // If invalid, skip
            if (_buoy == null)
            {
                Debug.LogError("Could not find nor create the buoy.");
                return false;
            }

            // Set up buoy
            return _buoy.SetFishesAvailable(fishesToCatch);
        }

        private void CollectBuoy()
        {
            if (_buoy == null)
                return;

            _buoy.Collect();
            _buoy.transform.SetParent(buoyHolder, false);
            _buoy.transform.localPosition = Vector3.zero;
        }

        #endregion

        #region Territories

        [Header("Territories")]
        [SerializeField]
        [Tooltip("Determines the layers on which the territories' colliders are")]
        private LayerMask territoryLayer;

        [SerializeField]
        [Tooltip("Determines how far from the check goes")]
        private float territoryCheckRadius;

        [SerializeField]
        private float territoryCheckOffset;

        /// <returns>From where the territory check starts</returns>
        private Vector3 GetTerritoryCheckOrigin()
        {
            Vector3 pos = transform.position + transform.forward * territoryCheckOffset;

            // Remove Y
            pos.y = 0;

            return pos;
        }

        #endregion

        #region 3D Model

        [Header("3D Model")]
        [SerializeField]
        private GameObject activeFishingModel;

        [SerializeField]
        private GameObject inactiveFishingModel;

        private void SetFishingModel(bool isFishingActive)
        {
            activeFishingModel.SetActive(isFishingActive);
            inactiveFishingModel.SetActive(!isFishingActive);
        }

        #endregion

        #region Editor

        #if UNITY_EDITOR
        /// <inheritdoc/>
        private void OnDrawGizmosSelected()
        {
            Handles.color = Color.cyan;
            Handles.DrawSolidDisc(GetTerritoryCheckOrigin(), Vector3.up, territoryCheckRadius);
        }
        #endif

        #endregion
    }
}