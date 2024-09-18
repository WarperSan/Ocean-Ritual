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

        /// <returns>Colored name of this fish</returns>
        public string GetColoredName()
        {
            string color = this.Rarity switch
            {
                FishRarity.Uncommon => "green",
                FishRarity.Rare => "#066",
                FishRarity.Epic => "purple",
                FishRarity.Legendary => "orange",
                FishRarity.Mythic => "pink",
                _ => "white"
            };

            return $"<color={color}>{this.DisplayName}</color>";
        }
    }
}