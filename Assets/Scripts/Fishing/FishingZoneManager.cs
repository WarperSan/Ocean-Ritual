using Singletons;
using UnityEngine;

namespace Fishing
{
    public class FishingZoneManager : MonoBehaviour
    {
        #region Positions

        private Transform Player;
        private Transform Buoy;

        #endregion

        public bool FollowPlayer = false;

        public void StartTracking(Transform player, Transform buoy)
        {
            this.Player = player;
            this.Buoy = buoy;
        }

        #region Zones

        [Header("Zones")]
        [SerializeField, Tooltip("Determines the radius of the fishing zone")]
        private float fishingRadius = 5f;

        [SerializeField, Tooltip("Determines the radius of the hunting zone")]
        private float huntRadius = 10f;

        [SerializeField, Tooltip("Determines the radius of the end of the hunt")]
        private float huntEndRadius = 15f;

        /// <param name="distance">Distance to compare</param>
        /// <param name="distances">Sorted distances</param>
        /// <returns>Index of the distance that is the closest to the given distance</returns>
        private static int FindClosestZone(float distance, params float[] distances)
        {
            // Return first if only one
            if (distances.Length == 1)
                return 0;

            for (int i = 1; i < distances.Length; i++)
            {
                if (distance <= distances[i])
                    return i - 1;
            }

            return -1;
        }

        private void CheckZones(Vector3 origin, Vector3 target)
        {
            // Put on the same plane
            origin.y = target.y;

            // Calculate closest zone
            float distance = Vector3.Distance(origin, target);
            int zoneIndex = FindClosestZone(distance, fishingRadius, huntRadius, huntEndRadius);

            switch (zoneIndex)
            {
                // Fishing zone
                case 0:
                    this.FishingZoneEnter();
                    break;
                // Hunt limit
                case 1:
                    this.HuntLimitEnter();
                    break;
                // End hunt
                case 2:
                    this.HuntEndEnter();
                    break;
                // If outside
                default:
                    this.OutsideEnter();
                    break;
            }
        }

        private void FishingZoneEnter() => this.FollowPlayer = true;
        private void HuntLimitEnter() => this.FollowPlayer = true;
        private void HuntEndEnter() => this.FollowPlayer = false;
        private void OutsideEnter()
        {
            this.FollowPlayer = false;
            FishingManager.Instance.End();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (fishingRadius >= huntRadius)
            {
                Debug.LogWarning("The fishing radius has to be smaller than the hunt radius.");
                fishingRadius = huntRadius * 0.75f;
            }

            if (huntRadius >= huntEndRadius)
            {
                Debug.LogWarning("The hunt radius has to be smaller than the end hunt radius.");
                huntRadius = huntEndRadius * 0.75f;
            }
        }
#endif

        #endregion

        #region MonoBehaviour

        /// <inheritdoc/>
        private void Update()
        {
            // If target invalid, skip
            if (this.Player == null)
                return;

            this.CheckZones(this.Player.position, this.Buoy.position);
        }

        #endregion

        #region Gizmos
#if UNITY_EDITOR
        /// <inheritdoc/>
        private void OnDrawGizmos()
        {
            Vector3 origin = this.Buoy == null ? this.transform.position : this.Buoy.position;

            UnityEditor.Handles.color = Color.green;
            UnityEditor.Handles.DrawWireDisc(origin, Vector3.up, fishingRadius, 5);

            UnityEditor.Handles.color = Color.yellow;
            UnityEditor.Handles.DrawWireDisc(origin, Vector3.up, huntRadius, 5);

            UnityEditor.Handles.color = Color.red;
            UnityEditor.Handles.DrawWireDisc(origin, Vector3.up, huntEndRadius, 5);
        }
#endif
        #endregion
    }
}