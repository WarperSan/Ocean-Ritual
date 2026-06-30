using UnityEditor;
using UnityEngine;

namespace FishingModule
{
    /// <summary>
    /// Script that manages the different behaviors depending on the ring
    /// </summary>
    [RequireComponent(typeof(FishingBuoy))]
    public class ZoneManager : MonoBehaviour
    {
        private FishingBuoy Buoy;

        [HideInInspector]
        public FishingManager manager;

        #region Target

        private Transform target;
        private Vector3 startPosition;

        public void SetTarget(Transform target) => this.target = target;

        #endregion

        #region Rings

        [Header("Rings")]
        [SerializeField]
        [Tooltip("Determines the radius of the harvest zone")]
        private float harvestRadius = 25f;

        [SerializeField]
        [Tooltip("Determines the radius of the aggressiveness zone")]
        private float aggroRadius = 50f;

        [SerializeField]
        [Tooltip("Determines the radius of the flee zone")]
        private float fleeRadius = 75f;

        private void CheckForRings(Vector3 origin, Vector3 target)
        {
            // Put on the same plane
            origin.y = target.y;

            // Calculate the distance between them
            float distance = Vector3.Distance(origin, target);

            // If player is in the harvest zone
            if (distance <= harvestRadius)
            {
                Buoy.KeepCollecting = true;
                return;
            }

            Buoy.KeepCollecting = false;

            // If player is in the aggro zone
            if (distance <= aggroRadius)
            {
                TargetGeneral.Instance.Target = TargetGeneral.Instance.BoatTarget;
                return;
            }

            // <Untarget player>

            // If player is in the flee zone
            if (distance <= fleeRadius)
            {
                // <Damage buoy>
                TargetGeneral.Instance.Target = Buoy.transform;
                return;
            }

            // End fishing as a failure
            manager.EndFishing();
        }

        #endregion

        #region MonoBehaviour

        /// <inheritdoc/>
        private void Start()
        {
            Buoy = GetComponent<FishingBuoy>();
            startPosition = Buoy.transform.position;
        }

        /// <inheritdoc/>
        private void Update()
        {
            // If target invalid, skip
            if (target == null)
                return;

            CheckForRings(target.position, startPosition);
        }

        #endregion

        #region Editor

        #if UNITY_EDITOR
        /// <inheritdoc/>
        private void OnDrawGizmos()
        {
            Vector3 origin = Buoy == null ? transform.position : Buoy.transform.position;

            // Draw harvest radius
            Handles.color = Color.green;

            Handles.DrawWireDisc(origin,
                Vector3.up,
                harvestRadius,
                5);

            // Draw aggro radius
            Handles.color = Color.red;

            Handles.DrawWireDisc(origin,
                Vector3.up,
                aggroRadius,
                5);

            // Draw flee radius
            Handles.color = Color.gray;

            Handles.DrawWireDisc(origin,
                Vector3.up,
                fleeRadius,
                5);
        }

        /// <inheritdoc/>
        private void OnValidate()
        {
            if (harvestRadius >= aggroRadius)
            {
                Debug.LogWarning($"The value for '{nameof(harvestRadius)}' has to be smaller than the value for {nameof(aggroRadius)}.");
                harvestRadius = aggroRadius * 0.75f;
            }

            if (aggroRadius >= fleeRadius)
            {
                Debug.LogWarning($"The value for '{nameof(aggroRadius)}' has to be smaller than the value for {nameof(fleeRadius)}.");
                aggroRadius = fleeRadius * 0.75f;
            }
        }
        #endif

        #endregion
    }
}