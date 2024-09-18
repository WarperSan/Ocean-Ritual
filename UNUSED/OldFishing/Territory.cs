using UnityEngine;

namespace Map
{
    public class Territory : MonoBehaviour
    {
        private GameObject[] Enemies;

        public GameObject[] GetEnemies() => this.Enemies;
    }
}