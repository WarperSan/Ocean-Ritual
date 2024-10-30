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
        private GameObject targetBoat;

        [HideInInspector]
        public FishingManager manager;

        #region Target

        private Transform target;
        private Vector3 startPosition;

        public void SetTarget(Transform target) => this.target = target;

        #endregion

        #region Rings

        [Header("Rings")]
        //[SerializeField, Tooltip("Determines the radius of the harvest zone")]
        private float harvestRadius = 25f;

        //[SerializeField, Tooltip("Determines the radius of the aggressiveness zone")]
        private float aggroRadius = 50f;

        //[SerializeField, Tooltip("Determines the radius of the flee zone")]
        private float fleeRadius = 75f;

        private void CheckForRings(Vector3 origin, Vector3 target)
        {
            // Put on the same plane
            origin.y = target.y;

            // Calculate the distance between them
            float distance = Vector3.Distance(origin, target);

            // If player is in the harvest zone
            if (distance <= this.harvestRadius)
            {
                this.Buoy.KeepCollecting = true;
                return;
            }

            this.Buoy.KeepCollecting = false;


            // todo
            // If player is in the aggro zone
            if (distance <= this.aggroRadius)
            {
                TargetGeneral.Instance.Target = targetBoat.transform;
                return;
            }

            // <Untarget player>

            // If player is in the flee zone
            if (distance <= this.fleeRadius)
            {
                // <Damage buoy>
                TargetGeneral.Instance.Target = Buoy.transform;
                return;
            }

            // End fishing as a failure
            this.manager.EndFishing();
        }

        #endregion

        #region MonoBehaviour

        /// <inheritdoc/>
        private void Start()
        {
            this.Buoy = this.GetComponent<FishingBuoy>();
            this.startPosition = this.Buoy.transform.position;
            this.targetBoat = GameObject.FindWithTag("Boat");
        }

        /// <inheritdoc/>
        private void Update()
        {
            // If target invalid, skip
            if (this.target == null)
                return;

            this.CheckForRings(this.target.position, this.startPosition);
        }

        #endregion

        #region Editor
#if UNITY_EDITOR
        /// <inheritdoc/>
        private void OnDrawGizmos()
        {
            Vector3 origin = this.Buoy == null ? this.transform.position : this.Buoy.transform.position;

            // Draw harvest radius
            Handles.color = Color.green;
            Handles.DrawWireDisc(origin, Vector3.up, this.harvestRadius, 5);

            // Draw aggro radius
            Handles.color = Color.red;
            Handles.DrawWireDisc(origin, Vector3.up, this.aggroRadius, 5);

            // Draw flee radius
            Handles.color = Color.gray;
            Handles.DrawWireDisc(origin, Vector3.up, this.fleeRadius, 5);
        }

        /// <inheritdoc/>
        private void OnValidate()
        {
            if (this.harvestRadius >= this.aggroRadius)
            {
                Debug.LogWarning($"The value for '{nameof(this.harvestRadius)}' has to be smaller than the value for {nameof(this.aggroRadius)}.");
                this.harvestRadius = this.aggroRadius * 0.75f;
            }

            if (this.aggroRadius >= this.fleeRadius)
            {
                Debug.LogWarning($"The value for '{nameof(this.aggroRadius)}' has to be smaller than the value for {nameof(this.fleeRadius)}.");
                this.aggroRadius = this.fleeRadius * 0.75f;
            }
        }
#endif
        #endregion
    }
}