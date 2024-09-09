using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FishingModule
{
    [RequireComponent(typeof(Collider))]
    public class Territory : MonoBehaviour
    {
        private Collider _collider;

        [SerializeField]
        private FishSO[] _fishes = new FishSO[] { };

        private void Start()
        {
            _collider = this.GetComponent<Collider>();

#if UNITY_EDITOR
            if (!_collider.isTrigger)
            {
                Debug.LogWarning($"The object '{this.name}' has a collider that is not trigger. Please mark the collider as trigger.");
                _collider.isTrigger = true;
            }
#endif
        }

        #region Static

        /// <summary>
        /// Finds the territories near the given point
        /// </summary>
        /// <param name="origin">Origin of the check</param>
        /// <param name="radius">Maximum distance of the check</param>
        /// <returns>Territories found</returns>
        public static List<Territory> Near(Vector3 origin, float radius, int layerMask)
        {
            List<Territory> territories = new();

            Collider[] colliders = Physics.OverlapSphere(origin, radius, layerMask);

            foreach (Collider collider in colliders)
            {
                if (!collider.TryGetComponent(out Territory territory))
                    continue;

                territories.Add(territory);
            }

            return territories;
        }

        /// <summary>
        /// Finds all the unique fishes in the given territories
        /// </summary>
        /// <param name="territories">Territories to check</param>
        /// <returns>Fishes found</returns>
        public static List<FishSO> Fishes(IEnumerable<Territory> territories)
        {
            List<FishSO> fishes = new();

            // Finds all the unique fishes
            foreach (Territory territory in territories)
            {
                foreach (FishSO fish in territory._fishes)
                    fishes.Add(fish);
            }

            return fishes;
        }

        #endregion
    }
}