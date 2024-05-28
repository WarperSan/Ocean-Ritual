using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Territory", menuName = "Create Territory")]
    public class Territory : ScriptableObject
    {
        [Tooltip("All the fishes available in this zone")]
        public Fish[] Fishes;

        [Tooltip("All the enemies available in this zone")]
        public GameObject[] Enemies;
    }
}
