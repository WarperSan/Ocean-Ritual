using UnityEngine;

namespace Fishing
{
    /// <summary>
    /// Object that represents the informations for a fish
    /// </summary>
    public class Fish : MonoBehaviour
    {
        [Tooltip("Name to display when this fish is caught")]
        public string DisplayName;

        [Tooltip("Rarity of this fish")]
        public Rarity Rarity;

        [Tooltip("Weight of this fish")]
        public uint Weight;
    }

    /// <summary>
    /// Rarity of the fish
    /// </summary>
    public enum Rarity
    { 
        Common, 
        Uncommon, 
        Rare,
        Epic,
        Legendary,
        Mythic
    }
}
