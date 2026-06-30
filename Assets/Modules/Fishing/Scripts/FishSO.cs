using UnityEngine;

namespace FishingModule
{
    [CreateAssetMenu(fileName = "Fish", menuName = "ScriptableObjects/Fish")]
    public class FishSO : ScriptableObject
    {
        [Tooltip("Name to display when showing this fish")]
        public string DisplayName;

        [Tooltip("Icon of this fish")]
        public Sprite Icon;

        [Tooltip("Rarity of this fish")]
        public FishRarity Rarity;

        [Tooltip("Description of this fish")]
        public string Description;

        /// <returns>Colored name of this fish</returns>
        public string GetColoredName()
        {
            string color = Rarity switch
            {
                FishRarity.Common    => "#000000", // Black
                FishRarity.Uncommon  => "#0b6100", // Dark green
                FishRarity.Rare      => "#004a91", // Dark blue 
                FishRarity.Epic      => "#570091", // Dark purple
                FishRarity.Legendary => "#c74c00", // Orange
                FishRarity.Mythic    => "#ff2957", // Redish pink
                _                    => "#000000", // No color (black by default)
            };

            return $"<color={color}>{DisplayName}</color>";
        }

        public int GetPrice()
        {
            int cash = Rarity switch
            {
                FishRarity.Uncommon  => 5,
                FishRarity.Rare      => 10,
                FishRarity.Epic      => 15,
                FishRarity.Legendary => 20,
                FishRarity.Mythic    => 25,
                _                    => 0,
            };
            return cash;
        }
    }
}