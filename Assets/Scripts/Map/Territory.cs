using Fishing;
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
    }
}