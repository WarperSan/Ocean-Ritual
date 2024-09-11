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
        private FishPercent[] _fishes = new FishPercent[] { };

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

        [System.Serializable]
        public class FishPercent
        {
            public FishSO fish;
            public float percent;
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
        public static List<FishPercent> Fishes(IEnumerable<Territory> territories)
        {
            List<FishPercent> fishes = new();

            // Finds all the unique fishes
            foreach (Territory territory in territories)
                fishes.AddRange(territory._fishes);

            return fishes;
        }

        #endregion

        #region Editor
#if UNITY_EDITOR

        private void OnValidate()
        {
            const float total = 100;

            float sum = _fishes.Sum(f => f.percent);

            // If equal to 100, skip
            if (Mathf.Approximately(sum, total))
                return;

            if (sum > total)
                Debug.LogWarning($"The territory '{name}' does not add up to {total}. Please add the missing {sum - total}.");
            else
                Debug.LogWarning($"The territory '{name}' does not add up to {total}. Please remove the missing '{total - sum}'.");
        }

#endif
        #endregion
    }
}