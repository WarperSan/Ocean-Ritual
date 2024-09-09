using FishingModule;
using UnityEngine;

namespace Map
{
    public class Territory : MonoBehaviour
    {
        [SerializeField, Tooltip("All the fishes available in this zone")]
        private Fish[] Fishes;

        [SerializeField, Tooltip("All the enemies available in this zone")]
        private GameObject[] Enemies;

        public Fish[] GetFishes() => this.Fishes;
        public GameObject[] GetEnemies() => this.Enemies;

        #region Gizmos
#if UNITY_EDITOR
        [Header("Gizmos")]
        public string DisplayName;

        /// <inheritdoc/>
        private void OnValidate() => this.DisplayName = string.IsNullOrEmpty(this.DisplayName) ? this.gameObject.name : this.DisplayName;

        /// <inheritdoc/>
        private void OnDrawGizmos() => UnityEditor.Handles.Label(this.transform.position, this.DisplayName);
#endif
        #endregion
    }
}