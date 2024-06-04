using UnityEngine;

namespace Fishing
{
    /// <summary>
    /// Object that represents the informations for a fish
    /// </summary>
    public class Fish : Inventory.Item<FishSOData>
    {
        [Tooltip("Name to display when this fish is caught")]
        public string DisplayName;

        [Tooltip("Rarity of this fish")]
        public Rarity Rarity;

        [Tooltip("Weight of this fish")]
        public uint Weight;

        public uint Amount;

        protected override bool SetData(FishSOData data) 
        {
            this.Amount = data.Amount;
            return true;
        }
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

    [System.Serializable]
    public struct FishSOData
    {
        public uint Amount;
    }
}
