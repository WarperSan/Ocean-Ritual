using UnityEngine;

namespace FishingModule
{
    [CreateAssetMenu(fileName = "Fish", menuName = "ScriptableObjects/Fish")]
    public class FishSO : ScriptableObject
    {
        [Tooltip("Internal name of this fish")]
        public string Name;

        [Tooltip("Icon of this fish")]
        public Sprite Icon;

        [Tooltip("Rarity of this fish")]
        public Rarity Rarity;

        [Min(0), Tooltip("How many entries does this take in the loot table")]
        public float Chance;
    }

    /// <summary>
    /// Rarity of the fish
    /// </summary>
    public enum Rarity
    {
        Common = 0,
        Uncommon = 100,
        Rare = 200,
        Epic = 300,
        Legendary = 400,
        Mythic = 500
    }
}