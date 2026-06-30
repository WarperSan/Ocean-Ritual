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
        private FishPercent[] _fishes = new FishPercent[]
        {
        };

        [SerializeField]
        private GameObject[] enemies = new GameObject[]
        {
        };

        private void Start()
        {
            _collider = GetComponent<Collider>();

            #if UNITY_EDITOR
            if (!_collider.isTrigger)
            {
                Debug.LogWarning($"The object '{name}' has a collider that is not trigger. Please mark the collider as trigger.");
                _collider.isTrigger = true;
            }

            if (transform.position.y != 0)
                Debug.LogWarning($"The territory '{name}' is not located on the Y = 0. Please put it's Y position as 0.");
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
                // If position invalid, skip
                if (collider.transform.position.y != 0)
                    continue;

                // If not a territory, skip
                if (!collider.TryGetComponent(out Territory territory))
                    continue;

                territories.Add(territory);
            }

            return territories;
        }

        /// <summary>
        /// Finds all the unique fishes in the given territories and compiles their percent
        /// </summary>
        /// <param name="territories">Territories to check</param>
        /// <returns>Fishes found</returns>
        public static Dictionary<FishSO, float> Fishes(IEnumerable<Territory> territories)
        {
            Dictionary<FishSO, float> fishes = new();
            float total = 0;

            // Compile percentages
            foreach (Territory territory in territories)
            {
                foreach (FishPercent item in territory._fishes)
                {
                    if (!fishes.ContainsKey(item.fish))
                        fishes.Add(item.fish, 0);

                    fishes[item.fish] += item.percent;
                    total += item.percent;
                }
            }

            // Rectifie amounts
            for (int i = 0; i < fishes.Keys.Count; i++)
            {
                FishSO key = fishes.Keys.ElementAt(i);

                fishes[key] = fishes[key] / total * 100;
            }

            return fishes;
        }

        /// <summary>
        /// Finds all the unique enemies
        /// </summary>
        /// <returns>Enemies found</returns>
        public static Dictionary<GameObject, float> Enemies(IEnumerable<Territory> territories)
        {
            Dictionary<GameObject, float> enemies = new();

            // Compile percentages
            foreach (Territory territory in territories)
            {
                foreach (GameObject item in territory.enemies)
                    enemies[item] = 1;
            }

            return enemies;
        }

        #endregion

        #region Editor

        #if UNITY_EDITOR

        /// <inheritdoc/>
        private void OnValidate()
        {
            // Check for fish
            const float total = 100;

            float sum = _fishes.Sum(f => f.percent);

            // If equal to 100, skip
            if (Mathf.Approximately(sum, total))
                return;

            if (sum > total)
                Debug.LogWarning($"The territory '{name}' does not add up to {total}. Please add the missing {sum - total}.");
            else
                Debug.LogWarning($"The territory '{name}' does not add up to {total}. Please remove the missing '{total - sum}'.");

            // Check for position
            if (transform.position.y != 0)
            {
                transform.position = new Vector3(
                    transform.position.x,
                    0,
                    transform.position.z
                );
                Debug.LogWarning("A territory cannot be on a Y different from 0.");
            }
        }

        #endif

        #endregion
    }
}